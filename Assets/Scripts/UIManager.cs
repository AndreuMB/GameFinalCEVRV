using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    TMP_Text vidaPlayer;
    Slider vidaPlayerBar;
    TMP_Text vidaNexo;
    Slider vidaNexoBar;
    Slider pocionesBar;
    TMP_Text dinero;
    TMP_Text numeroOleada;
    TMP_Text municion;
    Image reloadProgressBar;

    //Variables para la funcionalidad de la rueda de carga
    float indicatorTimer;
    float maxIndicatorTimer;
    const int MAX_PROGRESS_BAR = 1;

    // Start is called before the first frame update
    void Awake()
    {
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        Nexus nexusRef = GameObject.FindObjectOfType<Nexus>();
        WaveManager waveRef = GameObject.FindObjectOfType<WaveManager>();
        
        playerController.OnWeaponStateChange.AddListener(UpdateWeaponUI);
        playerController.OnPlayerLifeStateChange.AddListener(UpdatePlayerLifeUI);
        playerController.OnReloadWeapon.AddListener(UpdateReloadProgressBar);
        playerController.OnPotionsStateChange.AddListener(UpdatePotionsUI);
        nexusRef.OnNexusLifeChange.AddListener(UpdateNexusLifeUI);
        nexusRef.OnNexusMoneyChange.AddListener(UpdateNexusMoneyUI);
        waveRef.OnWaveChange.AddListener(UpdateWaveUI);

        vidaPlayer = transform.Find("LifeContainer").Find("VidaPlayer").GetComponent<TMP_Text>();
        vidaPlayerBar = transform.Find("LifeContainer").Find("PlayerHpContainer").GetComponent<Slider>();

        pocionesBar = transform.Find("LifeContainer").Find("PotionContainer").GetComponent<Slider>();

        //vidaNexo = transform.Find("NexoContainer").Find("VidaNexo").GetComponent<TMP_Text>();
        vidaNexoBar = transform.Find("LifeContainer").Find("NexusHpContainer").GetComponent<Slider>();

        municion = transform.Find("AmmoContainer").Find("Municion").GetComponent<TMP_Text>();

        numeroOleada = transform.Find("InfoContainer").Find("WaveContainer").Find("NumOleada").GetComponent<TMP_Text>();
        dinero = transform.Find("InfoContainer").Find("MoneyContainer").Find("Dinero").GetComponent<TMP_Text>();

        reloadProgressBar = transform.Find("ReloadProgressBar").GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateWeaponUI(Weapon arma)
    {
        municion.text = arma.ammo.ToString();
    }
    void UpdatePotionsUI(int pociones, int maxPociones)
    {
        pocionesBar.maxValue = maxPociones;
        pocionesBar.value = pociones;
    }
    void UpdateNexusLifeUI(float life, float maxLife)
    {
        //vidaNexo.text = life.ToString();
        vidaNexoBar.maxValue = maxLife;
        vidaNexoBar.value = life;
    }
    void UpdateNexusMoneyUI(int money)
    {
        dinero.text = money.ToString();
    }
    void UpdatePlayerLifeUI(float life, float maxLife)
    {
        vidaPlayer.text = life.ToString();
        vidaPlayerBar.maxValue = maxLife;
        vidaPlayerBar.value = life;
    }
    void UpdateWaveUI(string wave)
    {
        numeroOleada.text = wave;
    }
    void UpdateReloadProgressBar(Weapon arma)
    {        
        StartCoroutine(ReloadProgress(arma));
    }

    IEnumerator ReloadProgress(Weapon arma)
    {
        indicatorTimer = 0;
        maxIndicatorTimer = arma.weaponData.loadTime;
        while (indicatorTimer < MAX_PROGRESS_BAR && arma.isActiveAndEnabled)
        {
            indicatorTimer += (MAX_PROGRESS_BAR / maxIndicatorTimer) * Time.deltaTime;
            reloadProgressBar.enabled = true;
            reloadProgressBar.fillAmount = indicatorTimer;

            if (indicatorTimer >= MAX_PROGRESS_BAR)
            {
                indicatorTimer = maxIndicatorTimer;
                reloadProgressBar.fillAmount = indicatorTimer;
                reloadProgressBar.enabled = false;
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
        reloadProgressBar.enabled = false;
        yield break;
    }
}
