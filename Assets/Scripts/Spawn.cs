using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesArray;
    // min seconds enemies wait to appear in a spawn
    [SerializeField] float minSecondsSpawn;
    // max seconds enemies wait to appear in a spawn
    [SerializeField] float maxSecondsSpawn;
    public static UnityEvent spawnEnemyEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy(){
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(Random.Range(minSecondsSpawn,maxSecondsSpawn));
            if (WaveManager.spawn)
            {
                GameObject enemy = enemiesArray[Random.Range(0,enemiesArray.Length)];
                WaveManager waveManager = GameObject.FindObjectOfType<WaveManager>();
                GameObject enemyGO = Instantiate(enemy, transform.position, Quaternion.identity);
                // event death in character class
                // enemyGO.GetComponentInChildren<Enemy>().death.AddListener(waveManager.checkWave);
                spawnEnemyEvent.Invoke();
            }
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
