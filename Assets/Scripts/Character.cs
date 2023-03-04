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
        if (other.gameObject.tag==Tags.BULLET)
        {
            if(decideDamage(other.gameObject.GetComponent<Bullet>())) takeDamage();
        }
    }

    protected abstract bool decideDamage(Bullet bullet);

    protected void takeDamage(){

    }

    protected void InstanciaArmas()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i] = Instantiate(weapons[i].gameObject, transform.position, Quaternion.identity, transform).GetComponent<Weapon>();
            weapons[i].gameObject.SetActive(false);
        }
        weapons[0].gameObject.SetActive(true);
    }

}
