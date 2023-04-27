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
    [SerializeField] GameObject[] weapons;
    bool swR;
    public ShopTypeEnum shopType;
    // [SerializeField] Product upgradeHealth = new Product(2500, 20);
    // [SerializeField] Product upgradeHealth = new Product();
    // [SerializeField] Product potions = new Product();
    public Product[] products;
    PlayerController player;
    // public bool shopRandomize;
    // List<GameObject> weaponsLink = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        // WaveManager.waveChange.AddListener(randomizeWeapons);
        
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
        if (weapons.Length == 0) return;            
        List<GameObject> weaponsShop = new List<GameObject>();
        foreach (GameObject weaponSlot in weaponSlots)
        {
            int inx;
            GameObject weapon;
            // weaponsShop = weapon;

            do
            {
                inx = Random.Range(0,weapons.Length);
                weapon = weapons[inx];
            } while (weaponsShop.Find(x => x == weapon));

            weaponsShop.Add(weapon);
            
            // weaponsLink.Add(weapon);
            weaponSlot.GetComponentsInChildren<Text>()[0].text = weapon.name;
            weaponSlot.GetComponentsInChildren<Text>()[1].text = weapon.GetComponent<Weapon>().weaponData.price.ToString();
            weaponSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            weaponSlot.GetComponent<Button>().onClick.AddListener(() => buyWeapon(weapon));

        }
        WaveManager.shopRandomize = false;
        
    }

    
    // void OnEnable(){
    //     // if (!swR) return;
    //     // randomizeWeapons();
    // }

    void buyWeapon(GameObject weapon){
        if (weapon.GetComponent<Weapon>().weaponData.price > Nexus.money) return;
        Nexus.money -= weapon.GetComponent<Weapon>().weaponData.price;
        // string wname = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        GameObject weaponObj = Instantiate(weapon);
        GameObject slotArma = GameObject.FindGameObjectWithTag(TagsEnum.SlotArma.ToString());
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
        weaponObj.transform.parent = slotArma.transform;
        weaponObj.transform.localPosition = new Vector3(1,0,0);
        weaponObj.transform.localRotation=Quaternion.identity;
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

    

    // Update is called once per frame
    // void Update()
    // {
    //     GameObject player = FindObjectOfType<PlayerController>().gameObject;
    //     float playerDistance = Vector3.Distance(gameObject.transform.position, player.transform.position);
    //     if (Input.GetKey(KeyCode.F) && playerDistance < 5)
    //     {
            
    //     }
    // }

    // void addButtonsListener(){
    //     GameObject[] WButtons = GameObject.FindGameObjectsWithTag(TagsEnum.SlotWeaponShop.ToString());
    //     foreach (GameObject WButton in WButtons)
    //     {
    //         print("name"+WButton.name);
    //     }
    // }

    void setPlayerShop(){
        GameObject[] productSlots = GameObject.FindGameObjectsWithTag(TagsEnum.SlotProduct.ToString());
        int i = 0;
        foreach (GameObject productSlot in productSlots)
        {
            if (i >= products.Length) {
                productSlot.GetComponentsInChildren<Text>()[0].text = "No Stock";
                productSlot.GetComponent<Button>().interactable = false;
                return;
            }

            Product product = products[i];

            // if (product.specialSlot)
            // {
            //     GameObject specialSlot = GameObject.FindGameObjectWithTag("SlotProductSpecial");
            // }

            productSlot.GetComponentsInChildren<Text>()[0].text = product.name;
            productSlot.GetComponentsInChildren<Text>()[1].text = product.price.ToString();
            // UnityEvent prodFunction = products[i].triggerEvent;
            
            productSlot.GetComponent<Button>().onClick.RemoveAllListeners();
            // productSlot.GetComponent<Button>().onClick.AddListener(() => prodFunction.Invoke());
            productSlot.GetComponent<Button>().onClick.AddListener(() => buyProduct(product));
            productSlot.GetComponent<Button>().interactable = true;

            i++;
        }
        // productSlots[0].GetComponentsInChildren<Text>()[0].text = upgradeHealth.name;
        // productSlots[0].GetComponentsInChildren<Text>()[1].text = upgradeHealth.price.ToString();
    }
}
