using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveManager : MonoBehaviour
{
    [SerializeField] int enemiesWaveStart;
    // number of enemies extra x round
    [SerializeField] int enemiesWaveScale;
    [SerializeField] int maxEnemiesScene;
    [SerializeField] public static int wave;
    [SerializeField] float secBtwWaves = 5;
    [SerializeField] public static bool spawn;
    public static UnityEvent waveChange = new UnityEvent();
    int enemiesWave;
    int enemiesScene;
    // Start is called before the first frame update
    void Start()
    {
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
        waveChange.Invoke();
        yield return new WaitForSeconds(secBtwWaves);
        enemiesWaveStart+=enemiesWaveScale;
        print("asdfasdfas");
        enemiesWave = enemiesWaveStart;
        spawnCheck();
        yield break;
    }

    public void checkWave(){
        print("enter check wave " + enemiesWave);
        enemiesWave--;
        if (enemiesWave<=0)
        {
            StartCoroutine(nextWave());
        }
    }
}
