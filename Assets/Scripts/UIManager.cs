using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{

    TMP_Text VidaPlayer;
    TMP_Text VidaNexo;
    TMP_Text Dinero;
    TMP_Text NumeroOleada;
    TMP_Text municion;



    // Start is called before the first frame update
    void Awake()
    {
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        
        playerController.OnWeaponStateChange.AddListener(UpdateWeaponUI);
        playerController.OnPlayerLifeStateChange.AddListener(UpdatePlayerLifeUI);
        WaveManager.waveChange.AddListener(UpdateWaveUI);

        VidaPlayer = transform.Find("VidaPlayer").GetComponent<TMP_Text>();
        municion = transform.Find("Municion").GetComponent<TMP_Text>();
        NumeroOleada = transform.Find("NumOleada").GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateWeaponUI(Weapon arma)
    {
        municion.text = arma.ammo.ToString();
    }
    void UpdatePlayerLifeUI(float life)
    {
        VidaPlayer.text = life.ToString();
    }
    void UpdateWaveUI()
    {
        NumeroOleada.text = WaveManager.wave.ToString();
    }


}
