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

        foreach(Transform productSlot in transform.Find("Products"))
        {
            productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
            productSlot.GetComponent<Button>().interactable = false;
        }
        
    }

    void OnEnable(){
        setSpSlot();
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
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void OnDisable(){
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void randomizeWeapons(){
        // bool WeaponUMR = GameObject.Find("WeaponUM").GetComponent<Machine>().randomizeWeapon;
        // foreach(Transform productSlot in transform.Find("Products"))
        GameObject[] weaponSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        if (weaponSlots.Length == 0) return;
        if (products.Length == 0) return;       
        // List<GameObject> weaponsShop = new List<GameObject>();
        List<Product> weaponsShop = new List<Product>();
        foreach (Transform weaponSlot in transform.Find("Products"))
        {
            int inx;
            // GameObject weapon;
            Product product;
            if (weaponSlot.tag == TagsEnum.SlotProductSpecial.ToString()) return;

            do
            {
                inx = Random.Range(0,products.Length);
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
        // GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        Transform productSlots = transform.Find("Products");
        int i = 0;
        GameObject productSlot;
        foreach (Product product in products)
        {
            if (i >= productSlots.childCount) return;
            if (product.specialSlot) return;

            productSlot = productSlots.GetChild(i).gameObject;
            if (productSlots.tag == TagsEnum.SlotProductSpecial.ToString()) return;
            // if not from this shop return
            // if (!productSlot.transform.IsChildOf(transform)) return;

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
