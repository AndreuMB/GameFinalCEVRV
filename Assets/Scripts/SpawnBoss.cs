using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        WaveManager.OnBossTime.AddListener(DoSpawnBoss);
    }

    void DoSpawnBoss()
    {
        Instantiate(boss, transform.position, Quaternion.identity);
    }
}
