using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    List<Weapon> weapons;

    int selectedIndex = 0;

    protected Weapon selectedWeapon => weapons[selectedIndex];

    void AddWeapon(Weapon weapon)
    {
        // Comprobar max armas
        weapons.Add(weapon);
    }

    protected void Fire()
    {
        selectedWeapon.Fire();
    }

}
