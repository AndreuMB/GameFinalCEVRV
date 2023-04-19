using UnityEngine.Events;

[System.Serializable]
public class Product {
    public string name;
    public int price;
    public int value;
    public bool specialSlot;
    public UnityEvent<int> triggerEvent;

    // public Product(int setPrice, float setValue, string setName = "Product"){
    //     name = setName;
    //     price = setPrice;
    //     value = setValue;
    // }
}