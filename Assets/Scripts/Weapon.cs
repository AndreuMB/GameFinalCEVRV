using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    public WeaponSO weaponData;
    float loaderAmmo;
    float fireStart;
    bool loadSw;
    bool enemyFire;
    [System.NonSerialized] public Character owner;

    // Start is called before the first frame update
    void Start()
    {
        loaderAmmo = weaponData.loaderMaxAmmo;
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
        if (!loadSw)
        {
            GameObject instance = Instantiate(weaponData.bullet, transform.position + transform.forward*OFFSET_BULLET, transform.rotation);
            // instance.GetComponent<Bullet>().setWeaponDamage(damage);
            instance.GetComponent<Bullet>().weapon = this;
            instance.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.VelocityChange);
            loaderAmmo--;
        }
    }

    IEnumerator load(){
        if (!loadSw && loaderAmmo!=weaponData.loaderMaxAmmo)
        {
            loadSw = true;
            yield return new WaitForSeconds(weaponData.loadTime);
            loaderAmmo = weaponData.loaderMaxAmmo;
            loadSw = false;
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
}
