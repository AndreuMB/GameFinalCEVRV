using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitPlane : MonoBehaviour
{
    [SerializeField] GameObject playerSpawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag == TagsEnum.Player.ToString())
        {
            collision.gameObject.transform.position = playerSpawn.transform.position;
        }else if(collision.gameObject.tag == TagsEnum.Enemy.ToString()){
            collision.gameObject.GetComponent<Enemy>().enemyDeath();
        }else{
            Destroy(collision.gameObject);
        }
    }

}
