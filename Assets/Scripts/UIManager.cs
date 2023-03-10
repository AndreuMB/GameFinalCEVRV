using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class UIManager : MonoBehaviour
{

    TMP_Text vidaPlayer;
    TMP_Text vidaNexo;
    TMP_Text dinero;
    TMP_Text numeroOleada;
    TMP_Text municion;



    // Start is called before the first frame update
    void Awake()
    {
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        Nexus nexusRef = GameObject.FindObjectOfType<Nexus>();
        
        playerController.OnWeaponStateChange.AddListener(UpdateWeaponUI);
        playerController.OnPlayerLifeStateChange.AddListener(UpdatePlayerLifeUI);
        nexusRef.OnNexusLifeChange.AddListener(UpdateNexusLifeUI);
        WaveManager.waveChange.AddListener(UpdateWaveUI);

        vidaPlayer = transform.Find("LifeContainer").Find("VidaPlayer").GetComponent<TMP_Text>();
        municion = transform.Find("AmmoContainer").Find("Municion").GetComponent<TMP_Text>();
        numeroOleada = transform.Find("WaveContainer").Find("NumOleada").GetComponent<TMP_Text>();
        vidaNexo = transform.Find("NexoContainer").Find("VidaNexo").GetComponent<TMP_Text>();
        dinero = transform.Find("DineroContainer").Find("Dinero").GetComponent<TMP_Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateWeaponUI(Weapon arma)
    {
        municion.text = arma.ammo.ToString();
    }
    void UpdateNexusLifeUI(float life)
    {
        vidaNexo.text = life.ToString();
    }
    void UpdatePlayerLifeUI(float life)
    {
        vidaPlayer.text = life.ToString();
    }
    void UpdateWaveUI()
    {
        numeroOleada.text = WaveManager.wave.ToString();
    }


}
