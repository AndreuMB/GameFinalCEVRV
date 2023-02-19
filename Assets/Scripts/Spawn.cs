using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesArray;
    public static UnityEvent spawnEnemyEvent = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy(){
        while (isActiveAndEnabled)
        {
            yield return new WaitForSeconds(Random.Range(1,3));
            if (WaveManager.spawn)
            {
                GameObject enemy = enemiesArray[Random.Range(0,enemiesArray.Length)];
                Instantiate(enemy, transform.position, Quaternion.identity);
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
