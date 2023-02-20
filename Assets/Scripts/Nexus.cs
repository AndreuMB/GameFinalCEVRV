using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nexus : MonoBehaviour
{
    [SerializeField] float life = 10000;
    // life recover x second
    [SerializeField] float regRate = 10;
    [SerializeField] float farmRate = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // IEnumerator moneyProduction(){
    //     while (isActiveAndEnabled)
    //     {
            
    //     }
    //     yield break;
    // }

    public float getFarmRate(){
        return farmRate;
    }

    IEnumerator regenerationLife(){
        while (isActiveAndEnabled){
            if (life <= life - regRate)
            {
                life+= regRate;
            }
            yield return new WaitForSeconds(1);
        }
        yield break;
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag=="EnemyBullet")
        {
            float damage = other.gameObject.GetComponent<Enemy>().getWeaponDamage();
        }
    }
}
