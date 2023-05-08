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
    bool swPS = true;
    public ShopTypeEnum shopType;
    public Product[] products;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        // GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        // GameObject spSlot = GameObject.FindGameObjectWithTag(TagsEnum.SlotProductSpecial.ToString());

        // foreach (GameObject productSlot in productSlots)
        // {
        //     // if not from this shop return
        //     print("gameObject.name 0 = " + gameObject.name);
        //     if (!productSlot.transform.IsChildOf(transform)) return;
        //     print("gameObject.name 1 = " + gameObject.name);
        //     productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
        //     productSlot.GetComponent<Button>().interactable = false;
        // }

        foreach(Transform productSlot in transform.Find("Products"))
        {
            productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
            productSlot.GetComponent<Button>().interactable = false;
        }


        // if (spSlot)
        // {
        //     spSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
        //     spSlot.GetComponent<Button>().interactable = false;
        // }

        // setSpSlot();

        // gameObject.SetActive(false);
        
    }

    void OnEnable(){
        switch (shopType)
        {
            case ShopTypeEnum.weapons:
                if(WaveManager.shopRandomize){
                    randomizeWeapons();
                }
            break;
            case ShopTypeEnum.player:
                if (swPS) setPlayerShop();
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
        
        GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
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

            // if not from this shop return
            if (!weaponSlot.transform.IsChildOf(transform)) return;

            do
            {
                inx = Random.Range(0,products.Length);
                // weapon = weapons[inx];
                // weapon = products[inx].productGameobject;
                product = products[inx];
            } while (weaponsShop.Exists(x => x == product));

            if (product.specialSlot) return;

            weaponsShop.Add(product);
            
            // weaponsLink.Add(weapon);
            weaponSlot.GetComponentsInChildren<Text>()[0].text = product.name;
            weaponSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
            weaponSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            weaponSlot.GetComponent<Button>().onClick.AddListener(() => buyWeapon(product));
            weaponSlot.GetComponent<Button>().interactable = true;

        }
        WaveManager.shopRandomize = false;
        
    }

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
        // weaponObj.layer = LayerMask.NameToLayer("Weapon");
        
        Destroy(GetEquipedWeapon().gameObject);
        weaponObj.GetComponent<Weapon>().owner = player.GetComponent<PlayerController>();
        player.weapons[player.selectedIndex] = weaponObj.GetComponent<Weapon>();
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
        GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        int i = 0;
        GameObject productSlot;
        foreach (Product product in products)
        {
            if (i >= productSlots.Length) return;
            if (product.specialSlot) return;

            productSlot = productSlots[i];
            // if not from this shop return
            if (!productSlot.transform.IsChildOf(transform)) return;

            print("product.name = " + product.name);
            productSlot.GetComponentsInChildren<Text>()[0].text = product.name;
            productSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
            
            productSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            productSlot.GetComponent<Button>().onClick.AddListener(() => buyProduct(product));
            productSlot.GetComponent<Button>().interactable = true;

            i++;
        }
        swPS = false;
    }

    void setSpSlot(){
        GameObject productSlot;
        foreach (Product product in products){
            if (product.specialSlot)
            {
                productSlot = GameObject.FindGameObjectWithTag(TagsEnum.SlotProductSpecial.ToString());
                if(!productSlot) return;
                // if not from this shop return
                if (!productSlot.transform.IsChildOf(transform)) return;
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

    Weapon GetEquipedWeapon(){
        GameObject slotArma = GameObject.FindGameObjectWithTag(TagsEnum.SlotArma.ToString());
        int weaponIndex = 0;

        // get player weapons
        for (int i = slotArma.transform.childCount-1; i >= 0; i--)
        {
            // find enabled weapon
            if (slotArma.transform.GetChild(i).gameObject.activeInHierarchy)
            {
                weaponIndex = i;
            }
        }
        return slotArma.transform.GetChild(weaponIndex).GetComponent<Weapon>();
    }

    public void UpgradeWeapon(){
        GetEquipedWeapon().weaponData.damage += GetEquipedWeapon().weaponData.damage*70/100;
        GetEquipedWeapon().weaponData.loadTime = GetEquipedWeapon().weaponData.loadTime/2;
        print($"damage {GetEquipedWeapon().weaponData.name} = {GetEquipedWeapon().weaponData.damage}");
    }
}
