using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] int enemiesWaveStart;
    // number of enemies extra x round
    [SerializeField] int enemiesWaveScale;
    [SerializeField] int maxEnemiesScene;
    [SerializeField] public static int wave;
    [SerializeField] float secBtwWaves = 5;
    [SerializeField] public static bool spawn;
    int enemiesWave;
    int enemiesScene;
    // Start is called before the first frame update
    void Start()
    {
        Spawn.spawnEnemyEvent.AddListener(spawnCheck);
        Enemy.death.AddListener(checkWave);
        StartCoroutine(nextWave());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void spawnCheck(){
        enemiesScene = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemiesScene>=maxEnemiesScene || enemiesWave<=enemiesScene)
        {
            spawn = false;
        }else{
            spawn = true;
        }
        print(spawn);
    }

    // void nextWave(){
    //     wave++;
    //     enemiesWaveStart+=enemiesWaveScale;
    //     enemiesWave = enemiesWaveStart;
    // }

    IEnumerator nextWave(){
        yield return new WaitForSeconds(secBtwWaves);
        wave++;
        enemiesWaveStart+=enemiesWaveScale;
        enemiesWave = enemiesWaveStart;
        spawnCheck();
        yield break;
    }

    void checkWave(){
        print("enter check wave " + enemiesWave);
        enemiesWave--;
        if (enemiesWave<=0)
        {
            StartCoroutine(nextWave());
        }
    }
}
