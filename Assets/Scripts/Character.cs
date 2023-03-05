using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected List<Weapon> weapons;
    protected Weapon selectedWeapon => weapons[selectedIndex];
    protected int selectedIndex = 0;

    [Header("Stats")]
    [SerializeField] float life;
    [SerializeField] float speed;

    void AddWeapon(Weapon weapon)
    {
        // Comprobar max armas
        weapons.Add(weapon);
    }

    protected void Fire()
    {
        selectedWeapon.Fire();
    }

    protected void OnCollisionEnter(Collision other){
        // if GO is a bullet
        if (other.gameObject.tag==Tags.BULLET)
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            // if isn't friendly fire takeDamage
            if(decideDamage(bullet)) takeDamage(bullet);
        }
    }

    protected abstract bool decideDamage(Bullet bullet);

    protected void takeDamage(Bullet bullet){
        // update character life
        life = life - bullet.weapon.getDamage();
        print("life = " + life);
        if (life <= 0) Destroy(gameObject);
    }

    protected void InstanciaArmas()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i] = Instantiate(weapons[i].gameObject, transform.GetChild(0).transform.GetChild(0).transform.position, Quaternion.identity, transform.GetChild(0).transform.GetChild(0).transform).GetComponent<Weapon>();
            weapons[i].owner = this;
            weapons[i].gameObject.SetActive(false);
        }
        weapons[0].gameObject.SetActive(true);
    }

}
