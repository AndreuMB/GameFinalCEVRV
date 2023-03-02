using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    public static UnityEvent hit = new UnityEvent();

    // GameObject owner;

    // public bool isFromEnemy => owner.TryGetComponent<Enemy>(out Enemy _);

    // PROIPIEDADES COMPLETO
    // float _weaponDamage;
    // public float weaponDamage
    // {
    //     get
    //     {
    //         return _weaponDamage;
    //     }

    //     set
    //     {
    //         _weaponDamage = value;
    //     }
    // }

    // PROPIEDADES ABREVIADA
    // public float weaponDamage { get; private set; }

    public float weaponDamage;

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
        // if (other.gameObject.tag==Tags.ENEMY && gameObject.tag != Tags.BULLET)
        if (other.gameObject.tag==Tags.ENEMY || other.gameObject.tag==Tags.PLAYER)
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
