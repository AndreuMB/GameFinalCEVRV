using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    bool swR;
    // Start is called before the first frame update
    void Start()
    {
        // WaveManager.waveChange.AddListener(randomizeWeapons);
        print("enter start shop");
        randomizeWeapons();
    }

    public void randomizeWeapons(){
        bool WeaponUMR = GameObject.Find("WeaponUM").GetComponent<Machine>().randomizeWeapon;
        if(WeaponUMR){
            GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag("WeaponSlot");
            print("weaponSlots.Length = "+weaponSlots.Length);
            GameObject weapon;
            foreach (GameObject weaponSlot in weaponSlots)
            {
                weapon = weapons[Random.Range(0,weapons.Length)];
                weaponSlot.GetComponentInChildren<Text>().text = weapon.name;
                weaponSlot.GetComponent<Button>().onClick.AddListener(() => buyWeapon(weapon));
            }
            WeaponUMR = false;
        }
    }

    
    void OnEnable(){
        // if (!swR) return;
        // randomizeWeapons();
    }

    void buyWeapon(GameObject weapon){
        GameObject weaponObj = Instantiate(weapon);
        weaponObj.transform.parent = GameObject.Find("Player").transform;
        weaponObj.transform.localPosition = new Vector3(-1,0,0);
        weaponObj.transform.localRotation=Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
