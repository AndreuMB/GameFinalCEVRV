using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] List<GameObject> weapons;
    protected GameObject selectedWeapon => weapons[selectedIndex];
    int selectedIndex = 0;

    [Header("Stats")]
    [SerializeField] float life;
    [SerializeField] float speed;

    void AddWeapon(GameObject weapon)
    {
        // Comprobar max armas
        weapons.Add(weapon);
    }

    protected void Fire()
    {
        selectedWeapon.GetComponent<Weapon>().Fire();
    }

    protected void OnCollisionEnter(Collision other){
        if (other.gameObject.tag==Tags.BULLET)
        {
            if(decideDamage(other.gameObject.GetComponent<Bullet>())) takeDamage();
        }
    }

    protected abstract bool decideDamage(Bullet bullet);

    protected void takeDamage(){

    }

}
