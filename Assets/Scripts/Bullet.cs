using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Bullet : MonoBehaviour
{
    // public static UnityEvent hit = new UnityEvent();
    [System.NonSerialized] public Weapon weapon;
    public Character owner => weapon.owner;
    public bool firstCollision = false;

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

    // public float weaponDamage;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyBullet());
        Physics.IgnoreCollision(GameObject.FindGameObjectWithTag(TagsEnum.Player.ToString()).GetComponent<Collider>(), GetComponent<Collider>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        // if (other.gameObject.tag==Tags.ENEMY && gameObject.tag != Tags.BULLET)
        if (collision.gameObject.tag==TagsEnum.Enemy.ToString() || collision.gameObject.tag==TagsEnum.Player.ToString() || collision.gameObject.tag == TagsEnum.Nexus.ToString())
        {
            Destroy(gameObject);
            // hit.Invoke();
        }else{
            Rigidbody rb = GetComponent<Rigidbody>();
            if (!firstCollision)
            {
                Vector3 d = rb.velocity.normalized;
                Vector3 n = collision.contacts[0].normal;
                Vector3 r = d - 2 * (Vector3.Dot(d, n) * n);
                rb.velocity = r * 10;
            }
            firstCollision = true;
        }
        
    }

    IEnumerator destroyBullet(){
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
        yield break;
    }

    // public void setWeaponDamage(float weaponDamageSet){
    //     weaponDamage = weaponDamageSet;
    // }

    // public float getWeaponDamage(){
    //     return weaponDamage;
    // }
}
