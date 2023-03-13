using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public WeaponSO weaponData;
    //TODO cambiar float to int
    float loaderAmmo;
    public float ammo => loaderAmmo;

    float fireStart;
    bool loadSw;
    bool enemyFire;
    [System.NonSerialized] public Character owner;
    Vector3 prevPos;
    Vector3 actualPos;
    Animator animator;
    // Vector3 weaponOffset;
    public UnityEvent hitEnemyEv = new UnityEvent();
    public UnityEvent hitPlayerEv = new UnityEvent();
    [SerializeField] ParticleSystem ps;

    void Awake()
    {
        loaderAmmo = weaponData.loaderMaxAmmo;

    }

    // Start is called before the first frame update
    void Start()
    {
        print(loaderAmmo);
        loadSw = false;

        if (transform.parent.GetComponent<Enemy>())
        {
            transform.parent.GetComponent<Enemy>().fireEvent.AddListener(swAutoFire);
        }
    }

    void OnEnable(){
        if (!gameObject) return;
        StartCoroutine(checkPlayerMovement());
    }

    // Update is called once per frame
    void Update()
    {
        // if ((Input.GetMouseButtonDown(0) && auto) || (Input.GetMouseButtonDown(0) && !auto) || enemyFire){
        //     if (loaderAmmo <= 0){
        //         StartCoroutine(load());
        //     }else{
        //         if (Time.time > fireStart + fireRate) {
        //             fireStart = Time.time;
        //             Fire();
        //         }
        //     }
        // }


        //TODO: Hacer recarga
        // if (Input.GetKeyDown(KeyCode.R)){
        //     StartCoroutine(load());
        // }


    }

    public void ReLoad(){
        StartCoroutine(load());
    }

    public void Fire()
    {

        if (loaderAmmo <= 0){
            StartCoroutine(load());
        }
        else
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
        const int STRENGHT = 200;
        if (!loadSw)
        {
            Animator animator = GetComponent<Animator>();
            animator.SetTrigger("fire");
            GameObject instance = Instantiate(weaponData.bullet, transform.position + transform.forward*OFFSET_BULLET, transform.rotation);
            instance.GetComponent<Bullet>().weapon = this;
            instance.GetComponent<Rigidbody>().AddForce(transform.forward * STRENGHT, ForceMode.VelocityChange);
            loaderAmmo--;
            WeaponStateChanged();
            HitEnemy();
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
                if (Physics.Raycast(owner.transform.position, fwd, out RaycastHit hit, 50))
                {
                    print(hit.collider.gameObject.name + " was hit by enemy!");
                    // hitPlayerEv.Invoke();
                    hit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController player);
                    hit.collider.gameObject.TryGetComponent<Nexus>(out Nexus nexus);
                    if (player)
                    {
                        player.takeDamageRayCast(this);
                        player.OnPlayerLifeStateChange.Invoke(player.actualLife);
                    } 
                    if (nexus) nexus.takeDamageRayCast(this);

                }
 
        }

        if (owner.GetType() == typeof(PlayerController))
        {
            if(Physics.Raycast(ray, out RaycastHit hit)){
                print(hit.collider.gameObject.name + " was hit by player!");
                hit.collider.gameObject.TryGetComponent<Enemy>(out Enemy enemy);
                if (enemy) enemy.takeDamageRayCast(this);
                // instantiate particles when hit raycast
                instantiateParticles(hit);
            }
        }

                
    }

    void instantiateParticles(RaycastHit hit){
        // set and choose color of particles
        Color color = Color.blue;
        if (hit.collider.GetComponentInChildren<MeshRenderer>()){
            Renderer renderer = hit.collider.GetComponentInChildren<MeshRenderer>();
            Texture2D texture2D = renderer.material.mainTexture as Texture2D;
            Vector2 pCoord = hit.textureCoord;
            if (!texture2D) return;
            pCoord.x *= texture2D.width;
            pCoord.y *= texture2D.height;

            if (texture2D.isReadable)
            {
                Vector2 tiling = renderer.material.mainTextureScale;
                color = texture2D.GetPixel(Mathf.FloorToInt(pCoord.x * tiling.x) , Mathf.FloorToInt(pCoord.y * tiling.y));
            }
        }
       

        ParticleSystem psi=Instantiate(ps,hit.point,Quaternion.identity);
        var main = psi.main;
        main.startColor = color;
    }

    IEnumerator load(){
        //TODO: extraer el if, y las variables fuera, ya que la variable al cambiar el arma se bugea y hace que no se pueda volver a usar el arma
        //Quitar !loadSW del if, no tiene sentido que est� ahi
        if (!loadSw && loaderAmmo!=weaponData.loaderMaxAmmo)
        {
            loadSw = true;
            yield return new WaitForSeconds(weaponData.loadTime);
            loaderAmmo = weaponData.loaderMaxAmmo;
            loadSw = false;
            WeaponStateChanged();
        }
        yield break;
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

    void WeaponStateChanged()
    {
        if (owner is PlayerController player)
            {
                player.OnWeaponStateChange.Invoke(this);
            }
    }
    IEnumerator checkPlayerMovement() {
        Animator animator = GetComponent<Animator>();
        const float MIN_MOVEMENT = 0.5f;
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
        // if(owner.transform.hasChanged) print("character move");
        // if(!owner.transform.hasChanged) print("character STOP");
    }
}
