using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Weapon : MonoBehaviour
{
    //TODO cambiar float to int
    float loaderAmmo;
    public float ammo => loaderAmmo;

    float fireStart;
    bool enemyFire;
    [System.NonSerialized] public Character owner;
    Vector3 prevPos;
    Vector3 actualPos;
    // Vector3 weaponOffset;
    [System.NonSerialized] public UnityEvent hitEnemyEv = new UnityEvent();
    [System.NonSerialized] public UnityEvent hitPlayerEv = new UnityEvent();
    [SerializeField] ParticleSystem ps;

    //Metodo por si queremos parar la corrutina desde fuera
    //public void StopReload() => StopCoroutine(nameof(load));

    Coroutine reloadingCoroutine;
    bool isReloading => reloadingCoroutine != null;

    Transform iniTransform;
    AudioManager am;
    Animator animator;
    public GameObject upgrades;
    public WeaponSO weaponDataBase;
    [System.NonSerialized] public WeaponSO weaponData;
    
    public UnityEvent customShoot;

    Vector3 bulletInstantiatePosition;

    void Awake()
    {
        weaponData = Instantiate(weaponDataBase);
        loaderAmmo = weaponData.loaderMaxAmmo;
        iniTransform = transform;
        
        if (!upgrades) return;
        foreach (Transform weaponUpgrade in upgrades.transform)
        {
            if (!weaponUpgrade) break;
            // weaponData.upgrades.Add(weaponUpgrade);
            weaponUpgrade.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.GetComponent<Enemy>())
        {
            transform.parent.GetComponent<Enemy>().fireEvent.AddListener(swAutoFire);
        }
        am = FindObjectOfType<AudioManager>();
        // weaponData.upgrades = new List<GameObject>();

        
    }

    void OnEnable(){
        //Se activa la corrutina de movimiento del arma
        StartCoroutine(checkPlayerMovement());
        animator = GetComponentInParent<Animator>();
        animator.runtimeAnimatorController = weaponData.animatorController;
        setCrossHair();
    }

    void OnDisable() {
        //Se desactiva la corrutina de movimiento del arma
        StopCoroutine(checkPlayerMovement());

        //Como unity no quiere parar las corutinas con un Yield return Wait For Secons, lo paro manualmente, son las 4:19 de la ma�ana y estoy hasta el huevo
        if (reloadingCoroutine != null)
        {
            //En teoria este StopCoroutine hace menos que yo un dia de resaca
            // StopCoroutine(load());
            reloadingCoroutine = null;
        }

        foreach (AudioSource item in GetComponents<AudioSource>())
        {
            Destroy(item);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReLoad()
    {
        if (!isReloading)
        {
            reloadingCoroutine = StartCoroutine(load());
        }
    }

    public void Fire()
    {
        if (loaderAmmo <= 0 && !isReloading){
            am.Play("OutAmmo", gameObject);
            reloadingCoroutine = StartCoroutine(load());

        }
        else if (!isReloading)
        {
            if (Time.time > fireStart + weaponData.fireRate)
            {
                fireStart = Time.time;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        const int OFFSET_BULLET = 2;
        const int STRENGHT = 300;
        if (!isReloading)
        {
            am.Play(weaponData.audioFire,gameObject);
            animator.SetTrigger("fire");
            Transform slotArma = owner.GetComponent<Character>().slotWeapon;
            if (owner.GetComponent<PlayerController>())
            {
                bulletInstantiatePosition = Camera.main.transform.position;   
            }else{
                bulletInstantiatePosition = transform.position + slotArma.forward * OFFSET_BULLET;
            }
            if(customShoot.GetPersistentEventCount()>0){
                customShoot.Invoke();
            }else{
                GameObject instance = Instantiate(weaponData.bullet, bulletInstantiatePosition, slotArma.transform.rotation);
                instance.GetComponent<Bullet>().weapon = this;
                instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * STRENGHT, ForceMode.VelocityChange);
                loaderAmmo--;
                HitEnemy();
            }
            WeaponStateChanged();
        }
    }

    void HitEnemy(){
        // DEFAULT raycast player camera
        Vector3 cameraCenter = Camera.main.ViewportToScreenPoint(Vector3.one * .5f);
        Ray ray = Camera.main.ScreenPointToRay(cameraCenter);

        // fire player -> enemy
        // fire enemy -> player, nexus

        if (owner.GetType() == typeof(Enemy))
        {
                Vector3 fwd = owner.transform.TransformDirection(Vector3.forward);
                if (Physics.Raycast(owner.transform.position, fwd, out RaycastHit hit, weaponData.maxDistance))
                {
                    hit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController player);
                    hit.collider.gameObject.TryGetComponent<Nexus>(out Nexus nexus);
                    if (player)
                    {
                        player.takeDamageRayCast(this);
                        player.OnPlayerLifeStateChange.Invoke(player.actualLife, player.maxActualPlayerLife);
                    }
                    if (nexus) nexus.takeDamageRayCast(this);

                }

        }

        if (owner.GetType() == typeof(PlayerController))
        {
            if(Physics.Raycast(ray, out RaycastHit hit, weaponData.maxDistance)){
                print(hit.collider.gameObject.name + " was hit by player!");
                hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy);
                if (enemy) enemy.takeDamageRayCast(this);
                // instantiate particles when hit raycast
                instantiateParticles(hit);
            }
        }
        if (owner.GetType() == typeof(PlayerController))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, weaponData.maxDistance))
            {
                print(hit.collider.gameObject.name + " was hit by player!");
                hit.collider.gameObject.TryGetComponent<BossLogic>(out BossLogic enemy);
                if (enemy) enemy.takeDamageRayCast(this);
                // instantiate particles when hit raycast
                instantiateParticles(hit);
            }
        }

    }

    void instantiateParticles(RaycastHit hit){
        // set and choose color of particles
        Color color;

        if(!Enum.TryParse<TagsEnum>(hit.collider.tag, out TagsEnum tagEnum)) return;

        switch (tagEnum)
        {
            case TagsEnum.Enemy:
                color = Color.blue;
                break;
            case TagsEnum.Terrain:
                color = Color.green;
                break;
            default:
                color = Color.blue;
                break;
        }


        if (hit.collider.GetComponentInChildren<MeshRenderer>()){
            Renderer renderer = hit.collider.GetComponentInChildren<MeshRenderer>();
            Texture2D texture2D = renderer.material.mainTexture as Texture2D;
            if (texture2D){
                Vector2 pCoord = hit.textureCoord;
                pCoord.x *= texture2D.width;
                pCoord.y *= texture2D.height;

                if (texture2D.isReadable)
                {
                    Vector2 tiling = renderer.material.mainTextureScale;
                    color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x) , Mathf.FloorToInt(pCoord.y * tiling.y));
                }else{
                    texture2D = duplicateTexture(texture2D);
                    Vector2 tiling = renderer.material.mainTextureScale;
                    color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x) , Mathf.FloorToInt(pCoord.y * tiling.y));
                }
            }
        }


        ParticleSystem psi=Instantiate(ps,hit.point,Quaternion.identity);
        var main = psi.main;
        main.startColor = color;
        foreach (ParticleSystem ps in psi.GetComponentsInChildren<ParticleSystem>())
        {
            main = ps.main;
            main.startColor = color;
        }
    }

    IEnumerator load(){
        //Si ya existe la recarga o tenemos la balas maximas salimos de la corutina
        if (isReloading) yield break;
        if (loaderAmmo==weaponData.loaderMaxAmmo) yield break;
        am.Play(weaponData.audioReload,gameObject);
        WeaponReload();
        yield return new WaitForSeconds(weaponData.loadTime);
        loaderAmmo = weaponData.loaderMaxAmmo;
        WeaponStateChanged();
        reloadingCoroutine = null;
    }

    public float getZoom(){
        return weaponData.zoom;
    }

    public float getDamage(){
        return weaponData.damage;
    }

    void swAutoFire(){
        enemyFire = !enemyFire;
    }
    void WeaponReload()
    {
        if(owner is PlayerController player)
        {
            // Animator animator = GetComponent<Animator>();
            player.OnReloadWeapon.Invoke(this);
            animator.SetFloat("reloadSpeed", 1/weaponData.loadTime);
            animator.SetTrigger("reload");
        }
    }

    void WeaponStateChanged()
    {
        if (owner is PlayerController player)
        {
            player.OnWeaponStateChange.Invoke(this);
        }
    }
    IEnumerator checkPlayerMovement() {
        const float MIN_MOVEMENT = 0.1f;
        while (isActiveAndEnabled)
        {
            if (!owner) break;
            prevPos = owner.transform.position;
            yield return new WaitForSeconds(0.1f);
            actualPos = owner.transform.position;
            float distance = Vector3.Distance(prevPos,actualPos);
            if (distance<MIN_MOVEMENT) animator.SetBool("run",false);
            if (distance>=MIN_MOVEMENT) animator.SetBool("run",true);
        }
        yield break;
    }

    public void ShootgunShoot(){
        const int STRENGHT = 300;
        Transform slotArma = owner.GetComponent<Character>().slotWeapon;
        for (int i = 0; i < weaponData.bulletsNumber; i++)
        {
            float limitRotate = 3 + weaponData.radius*2f;
            Vector3 randomBullet = new Vector3(UnityEngine.Random.Range(-limitRotate,limitRotate),UnityEngine.Random.Range(-limitRotate,limitRotate),UnityEngine.Random.Range(-limitRotate,limitRotate));
            GameObject instance = Instantiate(weaponData.bullet, bulletInstantiatePosition, slotArma.transform.rotation);
            instance.GetComponent<Bullet>().weapon = this;
            instance.transform.Rotate(randomBullet);
            instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward  * (STRENGHT-200), ForceMode.VelocityChange);
        }
        
        loaderAmmo=loaderAmmo-2;

        HitEnemyShotGun();
    }

    void HitEnemyShotGun(){
        // DEFAULT raycast player camera
        Vector3 cameraCenter = Camera.main.transform.position;
        Vector3 ray = Camera.main.transform.forward;
        RaycastHit[] hits = Physics.SphereCastAll(cameraCenter, weaponData.radius, ray, weaponData.maxDistance);
        foreach (RaycastHit hit in hits)
        {
            hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy);
            if (enemy) enemy.takeDamageRayCast(this);
            // instantiate particles when hit raycast
            instantiateParticles(hit);
        }
    }

    public void setCrossHair(){
        // if owner not exist or isn't player don't change crosshair
        if (!owner) return;
        if (!owner.GetComponent<PlayerController>()) return;
        GameObject crossAir = GameObject.FindGameObjectWithTag(TagsEnum.CrossAir.ToString());
        if (weaponData.customCrossAir) {
            crossAir.GetComponent<Image>().sprite = weaponData.customCrossAir;
            float defaultSize = 45;
            float crossHairSize = defaultSize * weaponData.radius;
            crossAir.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(crossHairSize,crossHairSize);
        }else{
            GameObject player = GameObject.FindGameObjectWithTag(TagsEnum.Player.ToString());
            crossAir.GetComponent<Image>().sprite = player.GetComponent<PlayerController>().originalCrossAir;
            crossAir.GetComponent<Image>().rectTransform.sizeDelta = new Vector2(25,25);
        }
    }

    public void CrossbowShoot(){
        const int STRENGHT = 300;
        Transform slotArma = owner.GetComponent<Character>().slotWeapon;
        GameObject instance = Instantiate(weaponData.bullet, bulletInstantiatePosition, slotArma.transform.rotation);
        instance.GetComponent<Bullet>().weapon = this;
        instance.GetComponent<Rigidbody>().AddForce(instance.transform.forward * STRENGHT, ForceMode.VelocityChange);
        loaderAmmo--;
        HitEnemyCrossbow();
    }

    void HitEnemyCrossbow(){
        // DEFAULT raycast player camera
        Vector3 cameraCenter = Camera.main.transform.position;
        Vector3 ray = Camera.main.transform.forward;
        RaycastHit[] hits = Physics.RaycastAll(cameraCenter, ray, weaponData.maxDistance);
        foreach (RaycastHit hit in hits)
        {
            hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy);
            if (enemy) enemy.takeDamageRayCast(this);
            // instantiate particles when hit raycast
            instantiateParticles(hit);
        }
    }

    Texture2D duplicateTexture(Texture2D source){
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
 }
}
