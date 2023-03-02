using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField]
    protected List<Weapon> weapons = new List<Weapon>(2);

    protected int selectedIndex = 0;

    protected Weapon selectedWeapon => weapons[selectedIndex];

    void Start()
    {
        // GameObject weaponObj = Instantiate(selectedWeapon);
        // GameObject player = GameObject.Find(Tags.PLAYER);
        // weaponObj.transform.parent = player.transform;
        // weaponObj.transform.localPosition = new Vector3(1,0,0);
        // weaponObj.transform.localRotation=Quaternion.identity;
    }

    void AddWeapon(Weapon weapon)
    {
        // Comprobar max armas
        weapons.Add(weapon);
    }

    protected void Fire()
    {
        selectedWeapon.GetComponent<Weapon>().Fire();
    }

}
