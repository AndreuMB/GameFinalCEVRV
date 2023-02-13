using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] GameObject[] enemiesArray;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy(){
        while (isActiveAndEnabled)
        {
            GameObject enemy = enemiesArray[Random.Range(0,enemiesArray.Length)];
            yield return new WaitForSeconds(Random.Range(1,3));
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
