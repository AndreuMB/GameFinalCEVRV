using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] GameObject[] bullets;
    [SerializeField] GameObject bullet;
    // 6 pistol
    // 10 auto
    [SerializeField] float loaderMaxAmmo;
    // 1 pistol
    // 2 auto
    [SerializeField] float loadTime;
    // 0.5f pistol
    // 0.2f auto
    [SerializeField] float fireRate;
    // 15 pistol
    // 10 auto
    [SerializeField] float damage;
    // 0 pistol
    // 1 auto
    [SerializeField] public bool auto;
    [SerializeField] float zoom = 1;
    public float loaderAmmo;
    float fireStart;
    bool loadSw;
    bool enemyFire;
    public Character owner;

    // Start is called before the first frame update
    void Start()
    {
        loaderAmmo = loaderMaxAmmo;
        loadSw = false;

        if (transform.parent.GetComponent<Enemy>())
        {
            transform.parent.GetComponent<Enemy>().fireEvent.AddListener(swAutoFire);
            // bullet = bullets[1];
        }else{
            // bullet = bullets[0];
        }
        // PlayerController.triggerFire.AddListener(fire); trigger from player
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0) && auto) || (Input.GetMouseButtonDown(0) && !auto) || enemyFire){
            if (loaderAmmo <= 0){
                StartCoroutine(load());
            }else{
                if (Time.time > fireStart + fireRate) {
                    fireStart = Time.time;
                    Fire();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(load());
        }
    }

    public void Fire()
    {

        if (loaderAmmo <= 0){
            StartCoroutine(load());
        }
        else
        {
            if (Time.time > fireStart + fireRate)
            {
                fireStart = Time.time;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        const int OFFSET_BULLET = 2;
        if (!loadSw)
        {
            GameObject instance = Instantiate(bullet, transform.position + transform.forward*OFFSET_BULLET, transform.rotation);
            instance.GetComponent<Bullet>().setWeaponDamage(damage);
            instance.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.VelocityChange);
            loaderAmmo--;
        }
    }

    IEnumerator load(){
        if (!loadSw && loaderAmmo!=loaderMaxAmmo)
        {
            loadSw = true;
            yield return new WaitForSeconds(loadTime);
            loaderAmmo = loaderMaxAmmo;
            loadSw = false;
        }
        yield break;
    }

    public float getZoom(){
        return zoom;
    }

    public float getDamage(){
        return damage;
    }

    void swAutoFire(){
        enemyFire = !enemyFire;
    }
}
