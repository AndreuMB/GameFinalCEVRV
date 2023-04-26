using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Product: MonoBehaviour {
    public new string name;
    public int price;
    public int value;
    public bool specialSlot;
    public UnityEvent<int> triggerEvent;
    public GameObject productGameobject;

    // public Product(int setPrice, float setValue, string setName = "Product"){
    //     name = setName;
    //     price = setPrice;
    //     value = setValue;
    // }
}