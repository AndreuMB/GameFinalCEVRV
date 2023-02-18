using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pistol : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float chargerMaxAmmo = 3;
    float chargerAmmo;
    [SerializeField] float loadTime = 2;
    bool loadSw;
    [SerializeField] float zoom = 1;
    [SerializeField] float fireRate = 0.2f;
    float fireStart;
    [SerializeField] float damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        chargerAmmo = chargerMaxAmmo;
        loadSw = false;
        // PlayerX.triggerFire.AddListener(fire); trigger from player
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            if (chargerAmmo <= 0){
                StartCoroutine(load());
            }else{
                if (Time.time > fireStart + fireRate) {
                    fireStart = Time.time;
                    fire();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R)){
            StartCoroutine(load());
        }
    }

    void fire(){
        if (!loadSw)
        {
            GameObject instance = Instantiate(bullet, transform.position, transform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.VelocityChange);
            chargerAmmo--;
        }
    }

    IEnumerator load(){
        if (!loadSw && chargerAmmo!=chargerMaxAmmo)
        {
            loadSw = true;
            yield return new WaitForSeconds(loadTime);
            chargerAmmo = chargerMaxAmmo;
            loadSw = false;
        }
        yield break;
    }

    public float getZoom(){
        return zoom;
    }
}
