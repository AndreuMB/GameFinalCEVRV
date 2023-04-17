using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMachine : MonoBehaviour
{
    [SerializeField] GameObject canvasShop;
    Shop shop;
    // Start is called before the first frame update
    void Start()
    {
        shop = FindObjectOfType<Shop>();
        shop.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = FindObjectOfType<PlayerController>().transform.position;
        float playerDistance = Vector3.Distance(gameObject.transform.position,playerPosition);
        if (Input.GetKeyDown(KeyCode.F) && playerDistance < 3)
        {
            canvasShop.SetActive(!canvasShop.activeInHierarchy);
        }
    }
}
