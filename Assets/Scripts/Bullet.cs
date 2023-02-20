using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public static UnityEvent hit = new UnityEvent();
    float weaponDamage;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag=="Enemy" && gameObject.tag != "EnemyBullet")
        {
            Destroy(gameObject);
            hit.Invoke();
        }
    }

    IEnumerator destroyBullet(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        yield break;
    }

    public void setWeaponDamage(float weaponDamageSet){
        weaponDamage = weaponDamageSet;
    }

    public float getWeaponDamage(){
        return weaponDamage;
    }
}
