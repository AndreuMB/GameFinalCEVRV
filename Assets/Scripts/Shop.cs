using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    [SerializeField] GameObject[] weapons;
    bool swR;
    // List<GameObject> weaponsLink = new List<GameObject>();
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
            
            
            foreach (GameObject weaponSlot in weaponSlots)
            {
                int inx = Random.Range(0,weapons.Length);
                GameObject weapon = weapons[inx];
                // weaponsLink.Add(weapon);
                print("weapon = " + weapon);
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
        // string wname = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        GameObject weaponObj = Instantiate(weapon);
        GameObject player = GameObject.Find("Player");
        for (var i = player.transform.childCount - 1; i >= 0; i--)
        {
            if (player.transform.GetChild(i).tag == "Weapon")
            {
                Destroy(player.transform.GetChild(i).gameObject);
            }
        }
        weaponObj.transform.parent = player.transform;
        weaponObj.transform.localPosition = new Vector3(1,0,0);
        weaponObj.transform.localRotation=Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
