using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    int enemiesWaveStart = 0;
    // number of enemies extra x round
    [SerializeField] int enemiesWaveScale;
    [SerializeField] int maxEnemiesScene;
    [SerializeField] public static int wave;
    [SerializeField] float secBtwWaves = 5;
    [SerializeField] public static bool spawn;
    public static UnityEvent waveChange = new UnityEvent();
    int enemiesWave;
    int enemiesScene;
    public static float enemiesLife = 10;
    // public static int moneyDropByEnemies = 50;
    public static bool shopRandomize = true;

    //Evento para actualizar el HUD
    [System.NonSerialized] public UnityEvent<string> OnWaveChange = new UnityEvent<string>();
    public static UnityEvent OnBossTime = new UnityEvent();

    NumbersToRoman romanConverter = new NumbersToRoman();
    // Start is called before the first frame update
    void Start()
    {
        wave = 0;
        shopRandomize = true;
        Spawn.spawnEnemyEvent.AddListener(spawnCheck);
        // foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        // {
        //     enemy.GetComponent<Enemy>().death.AddListener(checkWave);
        // }
        // Enemy.death.AddListener(checkWave);
        StartCoroutine(nextWave());
    }

    // Update is called once per frame
    void Update()
    {
                
    }

    void spawnCheck(){
        enemiesScene = GameObject.FindGameObjectsWithTag(Tags.ENEMY).Length;

        if (enemiesScene>=maxEnemiesScene || enemiesWave<=enemiesScene)
        {
            spawn = false;
        }else{
            spawn = true;
        }
        
    }

    // void nextWave(){
    //     wave++;
    //     enemiesWaveStart+=enemiesWaveScale;
    //     enemiesWave = enemiesWaveStart;
    // }

    IEnumerator nextWave(){
        wave++;
        if (wave%5 == 0) shopRandomize = true;
        if (wave%2 == 0) OnBossTime.Invoke();
        enemiesLife+=10;
        OnWaveChange.Invoke(romanConverter.IntToRoman(wave));
        yield return new WaitForSeconds(secBtwWaves);
        waveChange.Invoke();
        enemiesWaveStart += enemiesWaveScale;
        enemiesWave = enemiesWaveStart;
        spawnCheck();
        yield break;
    }

    public void checkWave(){
        // enter check wave when enemy dies
        // remaining enemies
        enemiesWave--;
        if (enemiesWave<=0)
        {
            StartCoroutine(nextWave());
        }else{
            spawnCheck();
        }
    }
}

internal class NumbersToRoman
{
    public string IntToRoman(int wave)
    {
        string romanResult = "";
        Dictionary<string, int> romanNumbersDictionary = new() {
            {
                "I",
                1
            }, {
                "IV",
                4
            }, {
                "V",
                5
            }, {
                "IX",
                9
            }, {
                "X",
                10
            }, {
                "XL",
                40
            }, {
                "L",
                50
            }, {
                "XC",
                90
            }, {
                "C",
                100
            }, {
                "CD",
                400
            }, {
                "D",
                500
            }, {
                "CM",
                900
            }, {
                "M",
                1000
            }
        };
        foreach (KeyValuePair<string, int> romanNumber in romanNumbersDictionary.Reverse())
        {
            if (wave <= 0) break;
            while (wave >= romanNumber.Value)
            {
                romanResult += romanNumber.Key;
                wave -= romanNumber.Value;
            }
        }
        return romanResult;
    }
}
