using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    GameObject shopUI;
    GameObject player;
    public bool randomizeWeapon;
    // Start is called before the first frame update
    void Start()
    {
        shopUI = GameObject.Find("ShopUI");
        // shopUI.GetComponent<Image>().enabled = false;
        shopUI.SetActive(false);
        player = GameObject.FindWithTag("Player");
        // PlayerController.openMachine.AddListener(openMachine());
        WaveManager.waveChange.AddListener(checkRandomize);
    }

    void openMachine(){
        float playerDistance = Vector3.Distance(transform.position,player.transform.position);
        if (playerDistance <= 5)
        {
            shopUI.SetActive(!shopUI.activeInHierarchy);
            // shopUI.GetComponent<Image>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // move to playercontroller add event "openMachine"
        {
            openMachine();
        }
    }

    void checkRandomize(){
        if(WaveManager.wave%5==0) randomizeWeapon = true;
    }
}
