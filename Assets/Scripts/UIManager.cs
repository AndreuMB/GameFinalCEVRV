using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{

    TMP_Text balas;



    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindObjectOfType<PlayerController>().OnWeaponStateChange.AddListener(UpdateWeaponUI);

        balas = transform.Find("Balas").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateWeaponUI(Weapon arma)
    {
        balas.text = arma.ammo.ToString();
    }



}
