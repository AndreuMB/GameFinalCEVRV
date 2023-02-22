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
        randomizeWeapons();
    }

    public void randomizeWeapons(){
        if(WaveManager.wave%5!=0) return;
        GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag("WeaponSlot");
        print("weaponSlots.Length = "+weaponSlots.Length);
        foreach (GameObject weaponSlot in weaponSlots)
        {
            weaponSlot.GetComponentInChildren<Text>().text = weapons[Random.Range(0,weapons.Length)].name;
        }
        // if (weaponSlots.Length != 0)
        // {
        //     swR = false; 
        // }
    }

    
    void OnEnable(){
        // if (!swR) return;
        // randomizeWeapons();
    }

    void buyWeapon(){
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
