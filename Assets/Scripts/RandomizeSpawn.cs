using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            foreach (Transform child2 in child.transform)
            {
                bool random = Random.Range(0,2) == 0 ? false : true;
                child2.gameObject.SetActive(random);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
