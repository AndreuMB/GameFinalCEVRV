using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpawn : MonoBehaviour
{
    [SerializeField] bool singleInstance;
    [SerializeField] bool respawn;
    // Start is called before the first frame update
    void Start()
    {
        SpawnRandom();
        WaveManager.waveChange.AddListener(checkWave);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DisableAllChilds(){
        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                child2.gameObject.SetActive(false);
            }
        }
    }
    
    void SpawnRandom(){
        DisableAllChilds();
        foreach (Transform child in gameObject.transform)
        {
            bool random = false;
            do
            {
                foreach (Transform child2 in child.transform)
                {
                    random = Random.Range(0,2) == 0 ? false : true;
                    child2.gameObject.SetActive(random);
                    if (singleInstance && random) break;
                }
            } while (!random);
        }
    }

    void checkWave(){
        if(WaveManager.wave%5==0) SpawnRandom();
    }

}
