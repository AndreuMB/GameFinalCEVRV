using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float life = 2;
    Transform player;
    [SerializeField] float speed = 10;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        // Bullet.hit.AddListener(damage);
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody>();
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
        transform.LookAt(player);
        float distance = Vector3.Distance (gameObject.transform.position, player.transform.position);
        if (distance<5)
        {
            rb.velocity = Vector3.zero;
            print("fire");
        }else{
            rb.velocity = transform.forward * speed;
        }
    }

    void FixedUpdate(){

    }
}
