using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float life = 2;
    // Start is called before the first frame update
    void Start()
    {
        Bullet.hit.AddListener(damage);
    }

    void damage(){
        // life--;
        // if (life <= 0)
        // {
        //     Destroy(gameObject);
        // }
    }

    void OnCollisionEnter(Collision other){
        if (other.gameObject.tag=="Bullet")
        {
            life--;
            print("life = " + life);
            if (life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
