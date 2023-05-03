using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public enum ShopTypeEnum
{
    weapons,
    player
}

public class Shop : MonoBehaviour
{
    bool swR;
    public ShopTypeEnum shopType;
    public Product[] products;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        GameObject spSlot = GameObject.FindGameObjectWithTag(TagsEnum.SlotProductSpecial.ToString());
        foreach (GameObject productSlot in productSlots)
        {
            productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
            productSlot.GetComponent<Button>().interactable = false;
        }
        if (spSlot)
        {
            spSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
            spSlot.GetComponent<Button>().interactable = false;
        }

        setSpSlot();
        
    }

    void OnEnable(){
        switch (shopType)
        {
            case ShopTypeEnum.weapons:
                if(WaveManager.shopRandomize){
                    randomizeWeapons();
                }
                // addButtonsListener();
            break;
            case ShopTypeEnum.player:
                setPlayerShop();
                // addButtonsListener();
                break;
        }
        // Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void OnDisable(){
        // Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void randomizeWeapons(){
        // bool WeaponUMR = GameObject.Find("WeaponUM").GetComponent<Machine>().randomizeWeapon;
        
        GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotWeaponShop.ToString());
        if (weaponSlots.Length == 0) return;
        if (products.Length == 0) return;            
        // List<GameObject> weaponsShop = new List<GameObject>();
        List<Product> weaponsShop = new List<Product>();
        foreach (GameObject weaponSlot in weaponSlots)
        {
            int inx;
            // GameObject weapon;
            Product product;
            // weaponsShop = weapon;

            // do
            // {
            //     inx = Random.Range(0,weapons.Length);
            //     weapon = weapons[inx];
            // } while (weaponsShop.Find(x => x == weapon));

            do
            {
                inx = Random.Range(0,products.Length);
                // weapon = weapons[inx];
                // weapon = products[inx].productGameobject;
                product = products[inx];
            } while (weaponsShop.Exists(x => x == product));

            weaponsShop.Add(product);
            
            // weaponsLink.Add(weapon);
            weaponSlot.GetComponentsInChildren<Text>()[0].text = product.name;
            weaponSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
            weaponSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            weaponSlot.GetComponent<Button>().onClick.AddListener(() => buyWeapon(product));

        }
        WaveManager.shopRandomize = false;
        
    }

    
    // void OnEnable(){
    //     // if (!swR) return;
    //     // randomizeWeapons();
    // }

    void buyWeapon(Product product){
        if (product.price > Nexus.money) return;
        Nexus.money -= product.price;
        // string wname = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        GameObject weaponPrefab = product.productGameobject;
        // GameObject weaponObj = Instantiate(product.productGameobject);
        GameObject slotArma = GameObject.FindGameObjectWithTag(TagsEnum.SlotArma.ToString());

        GameObject weaponObj = Instantiate(weaponPrefab, slotArma.transform.position, Quaternion.identity, slotArma.transform);
        weaponObj.transform.localPosition = weaponPrefab.transform.position;
        weaponObj.transform.localRotation = weaponPrefab.transform.rotation;
        
        int weaponIndex = 0;
        // get player weapons
        
        for (int i = slotArma.transform.childCount-1; i >= 0; i--)
        {
            // find enabled weapon
            if (slotArma.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                weaponIndex = i;
                // replace it for the new weapon                
                // player.GetComponent<PlayerController>(),
                // player.GetComponent<PlayerController>().OnWeaponStateChange.Invoke(weaponObj.GetComponent<Weapon>());
                // 
            }
        }
        Destroy(slotArma.transform.GetChild(weaponIndex).gameObject);
        // weaponObj.transform.parent = slotArma.transform;
        // weaponObj.transform.localPosition = new Vector3(1,0,0);
        // weaponObj.transform.localRotation=Quaternion.identity;
        weaponObj.GetComponent<Weapon>().owner = player.GetComponent<PlayerController>();
        player.weapons[player.selectedIndex] = weaponObj.GetComponent<Weapon>();
        // player.GetComponent<PlayerController>().OnWeaponStateChange.Invoke(weaponObj.GetComponent<Weapon>());


        // Transform slotWeapon = GameObject.FindGameObjectWithTag(TagsEnum.SlotArma.ToString()).transform;
        // Animator slotAnimator = slotWeapon.GetComponent<Animator>();
        // print("i shop = " + 1);
        // Animator weaponAnimator = weapons[1].GetComponent<Animator>();
        // slotAnimator.runtimeAnimatorController = weaponAnimator.runtimeAnimatorController;
        // weaponObj.transform.parent = slotArma.transform;
        // weaponObj.transform.localPosition = new Vector3(1,0,0);
        // weaponObj.transform.localRotation=Quaternion.identity;
        // weaponObj.GetComponent<Weapon>().owner = player.GetComponent<PlayerController>();
        // player.GetComponent<PlayerController>().weapons[1] = weaponObj.GetComponent<Weapon>();
    }

    public void buyProduct(Product product){
        if (Nexus.money < product.price) return;
        Nexus.money -= product.price;
        product.triggerEvent.Invoke(product.value);
    }

    public void UpgradeLife(int value){
        player.upgradeHealth(value);
    }

    public void BuyPotion(int value){
        player.addPotions(value);
    }
    public void UpgradeSpeed(int value){
        player.upgradeSpeed(value);
    }


    void setPlayerShop(){
        // GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        // int i = 0;
        // foreach (GameObject productSlot in productSlots)
        // {
        //     if (i >= products.Length) {
        //         productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
        //         productSlot.GetComponent<Button>().interactable = false;
        //         return;
        //     }

        //     Product product = products[i];

        //     // if (product.specialSlot)
        //     // {
        //     //     GameObject specialSlot = GameObject.FindGameObjectWithTag("SlotProductSpecial");
        //     // }

        //     productSlot.GetComponentsInChildren<Text>()[0].text = product.name;
        //     productSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
        //     // UnityEvent prodFunction = products[i].triggerEvent;
            
        //     productSlot.GetComponent<Button>().onClick.RemoveAllListeners();
        //     // productSlot.GetComponent<Button>().onClick.AddListener(() => prodFunction.Invoke());
        //     productSlot.GetComponent<Button>().onClick.AddListener(() => buyProduct(product));
        //     productSlot.GetComponent<Button>().interactable = true;

        //     i++;
        // }
        // productSlots[0].GetComponentsInChildren<Text>()[0].text = upgradeHealth.name;
        // productSlots[0].GetComponentsInChildren<Text>()[1].text = upgradeHealth.price.ToString();

        GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        int i = 0;
        GameObject productSlot;
        foreach (Product product in products)
        {
            if (i >= productSlots.Length) {
                return;
            }

            productSlot = productSlots[i];

            productSlot.GetComponentsInChildren<Text>()[0].text = product.name;
            productSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
            // UnityEvent prodFunction = products[i].triggerEvent;
            
            productSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            // productSlot.GetComponent<Button>().onClick.AddListener(() => prodFunction.Invoke());
            productSlot.GetComponent<Button>().onClick.AddListener(() => buyProduct(product));
            productSlot.GetComponent<Button>().interactable = true;

            i++;
        }
    }

    void setSpSlot(){
        GameObject productSlot;
        foreach (Product product in products){
            if (product.specialSlot)
            {
                productSlot = GameObject.FindGameObjectWithTag(TagsEnum.SlotProductSpecial.ToString());
                if(!productSlot) return;
                productSlot.GetComponentsInChildren<Text>()[0].text = product.name;
                productSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
                // UnityEvent prodFunction = products[i].triggerEvent;
                
                productSlot.GetComponent<Button>().onClick.RemoveAllListeners();
                // productSlot.GetComponent<Button>().onClick.AddListener(() => prodFunction.Invoke());
                productSlot.GetComponent<Button>().onClick.AddListener(() => buyProduct(product));
                productSlot.GetComponent<Button>().interactable = true;
            }

        }
    }
}
