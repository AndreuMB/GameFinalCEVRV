using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    GameObject shopUI;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        shopUI = GameObject.Find("ShopUI");
        shopUI.GetComponent<Image>().enabled = false;
        player = GameObject.FindWithTag("Player");
        // PlayerController.openMachine.AddListener(openMachine());
    }

    void openMachine(){
        float playerDistance = Vector3.Distance(transform.position,player.transform.position);
        if (playerDistance <= 5)
        {
            // shopUI.SetActive(true);
            shopUI.GetComponent<Image>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E)) // move to playercontroller add event "openMachine"
        {
            openMachine();
        }
    }
}
