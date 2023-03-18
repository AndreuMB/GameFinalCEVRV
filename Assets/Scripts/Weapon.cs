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
    bool enemyFire;
    [System.NonSerialized] public Character owner;
    Vector3 prevPos;
    Vector3 actualPos;
    Animator animator;
    // Vector3 weaponOffset;
    public UnityEvent hitEnemyEv = new UnityEvent();
    public UnityEvent hitPlayerEv = new UnityEvent();

    //Metodo por si queremos parar la corrutina desde fuera
    //public void StopReload() => StopCoroutine(nameof(load));



    void Awake()
    {
        loaderAmmo = weaponData.loaderMaxAmmo;
    }

    // Start is called before the first frame update
    void Start()
    {
        print(loaderAmmo);

        if (transform.parent.GetComponent<Enemy>())
        {
            transform.parent.GetComponent<Enemy>().fireEvent.AddListener(swAutoFire);
        }
    }

    void OnEnable(){
        //Se activa la corrutina de movimiento del arma
        StartCoroutine(checkPlayerMovement());
    }

    void OnDisable() {
        //Se desactiva la corrutina de movimiento del arma
        //StopCoroutine(checkPlayerMovement());

        //if(isReloading) StopCoroutine(reloadingCoroutine);
        StopAllCoroutines();
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
        reloadingCoroutine = StartCoroutine(load());
    }

    public void Fire()
    {

        if (loaderAmmo <= 0){
            reloadingCoroutine = StartCoroutine(load());
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
        print(reloadingCoroutine);
        if (!isReloading)
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
            }
        }

                
    }
    Coroutine reloadingCoroutine;
    bool isReloading => reloadingCoroutine != null;

    IEnumerator load(){
        print("PreRecarga");
        //Si ya existe la recarga o tenemos la balas maximas salimos de la corutina
        if (isReloading) yield break;
        if (loaderAmmo==weaponData.loaderMaxAmmo) yield break;
        print("recargando");
        WeaponReload();
        yield return new WaitForSeconds(weaponData.loadTime);
        loaderAmmo = weaponData.loaderMaxAmmo;
        WeaponStateChanged();
        yield break;
        print("no deberia estar aqui");
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
            player.OnReloadWeapon.Invoke(this);
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
