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
    public ShopTypeEnum shopType;
    public Product[] products;
    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        foreach(Transform productSlot in transform.Find("Products"))
        {
            // if not from this shop return
            if (!productSlot.transform.IsChildOf(transform.Find("Products"))) continue;
            productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
            productSlot.GetComponent<Button>().interactable = false;
        }
        setSpSlot();
        if (shopType == ShopTypeEnum.player)
        {
            setPlayerShop();
        }
        
    }

    void OnEnable(){
        switch (shopType)
        {
            case ShopTypeEnum.weapons:
                if(WaveManager.shopRandomize){
                    randomizeWeapons();
                }
                checkUpgrade();
            break;
            case ShopTypeEnum.player:
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
            // if not from this shop return
            if (!weaponSlot.transform.IsChildOf(transform.Find("Products"))) continue;
            // if is a special/reserved slot return
            if (weaponSlot.tag == TagsEnum.SlotProductSpecial.ToString()) continue;

            do
            {
                inx = Random.Range(0,products.Length);
                product = products[inx];
            } while (weaponsShop.Exists(x => x == product));

            if (product.specialSlot) continue;

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
        GameObject weaponPrefab = product.productGameobject;
        GameObject slotArma = GetEquipedWeapon().transform.parent.gameObject;

        DestroyImmediate(GetEquipedWeapon().gameObject);

        GameObject weaponObj = Instantiate(weaponPrefab, slotArma.transform.position, Quaternion.identity, slotArma.transform);
        weaponObj.transform.localPosition = weaponPrefab.transform.position;
        weaponObj.transform.localRotation = weaponPrefab.transform.rotation;
        
        // for player weapon not go through terrain and objects
        foreach (MeshRenderer item in weaponObj.GetComponentsInChildren<MeshRenderer>())
        {
            item.gameObject.layer = LayerMask.NameToLayer("Weapon");
        }

        weaponObj.GetComponent<Weapon>().owner = player.GetComponent<PlayerController>();
        player.weapons[player.selectedIndex] = weaponObj.GetComponent<Weapon>();
        weaponObj.GetComponent<Weapon>().setCrossHair();
        checkUpgrade();
    }

    public void buyProduct(Product product){
        if (Nexus.money < product.price) return;
        if (product.name.Equals("Potions") && player.actualCuras >= player.maxActualCuras) return;
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
        Transform productSlots = transform.Find("Products");
        int i = 0;
        GameObject productSlot;
        foreach (Product product in products)
        {
            if (i >= productSlots.childCount) return;
            if (product.specialSlot) continue;

            productSlot = productSlots.GetChild(i).gameObject;
            print("productSlot = " + productSlot);
            // if not from this shop return
            if (!productSlot.transform.IsChildOf(transform.Find("Products"))) continue;

            if (productSlots.tag == TagsEnum.SlotProductSpecial.ToString()) continue;

            productSlot.GetComponentsInChildren<Text>()[0].text = product.name;
            productSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
            
            productSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            productSlot.GetComponent<Button>().onClick.AddListener(() => buyProduct(product));
            productSlot.GetComponent<Button>().interactable = true;
            print("end = " + productSlot);

            i++;
        }
    }

    void setSpSlot(){
        GameObject[] productSlots;
        GameObject productSlot = null;
        foreach (Product product in products){
            if (product.specialSlot)
            {
                productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProductSpecial.ToString());
                if (productSlots.Length == 0) return;
                foreach (GameObject productSlot2 in productSlots)
                {
                    if (!productSlot2.GetComponent<Button>().interactable){
                        productSlot = productSlot2;
                        break;
                    }
                }
                if(productSlot == null) return;
                // if not from this shop return
                if (!productSlot.transform.IsChildOf(transform.Find("Products"))) continue;
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
        return player ? player.GetComponentInChildren<Weapon>() : null;
    }

    public void UpgradeWeapon(){
        if (!GetEquipedWeapon().upgrades) return;

        GetEquipedWeapon().weaponData.damage += GetEquipedWeapon().weaponData.damage*70/100;
        GetEquipedWeapon().weaponData.loadTime = GetEquipedWeapon().weaponData.loadTime/2;
        GetEquipedWeapon().weaponData.maxDistance += GetEquipedWeapon().weaponData.maxDistance/2;
        print($"damage {GetEquipedWeapon().weaponData.name} = {GetEquipedWeapon().weaponData.damage}");
        if (GetEquipedWeapon().weaponData.wName == "Escopeta"){
            GetEquipedWeapon().weaponData.bulletsNumber += 3;
            GetEquipedWeapon().weaponData.radius -= GetEquipedWeapon().weaponData.radius*20/100;
            GetEquipedWeapon().setCrossHair();
        }
        foreach (Transform weaponUpgrade in GetEquipedWeapon().upgrades.transform)
        {
            if (!weaponUpgrade.gameObject.activeInHierarchy){
                weaponUpgrade.gameObject.SetActive(true);
                break;
            } 
        }
        checkUpgrade();
    }

    void checkUpgrade(){
        GameObject productSlot = GameObject.FindGameObjectWithTag(TagsEnum.SlotProductSpecial.ToString());
        // check spProductSlot exist
        if(!productSlot) return;
        // if not from this shop return
        // if (!productSlot.transform.IsChildOf(transform.Find("Products").transform)) return;
        // set to not interactable by default this way the player can't upgrade the weapon
        productSlot.GetComponentInChildren<Text>().text = "No Available";
        productSlot.GetComponent<Button>().interactable = false;
        // if no upgrades return
        if (!GetEquipedWeapon()) return;
        if (!GetEquipedWeapon().upgrades) return;
        //if any upgrade not active set btn to interactable
        foreach (Transform weaponUpgrade in GetEquipedWeapon().upgrades.transform)
        {
            if (!weaponUpgrade.gameObject.activeInHierarchy){
                productSlot.GetComponent<Button>().interactable = true;
                productSlot.GetComponentInChildren<Text>().text = "UPGRADES";
                return;    
            } 
        }
    }
}
