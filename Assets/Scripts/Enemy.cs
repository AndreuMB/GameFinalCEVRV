using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] float life = 20;
    Transform player;
    // [SerializeField] float speed = 10;
    Rigidbody rb;
    [SerializeField] GameObject bullet;
    [SerializeField] float fireRate = 0.5f;
    NavMeshAgent agent;
    float fireStart;
    float rangeToFire;
    RaycastHit raycast;
    // Start is called before the first frame update
    void Start()
    {
        // Bullet.hit.AddListener(damage);
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        // StartCoroutine(randomRange());
        rangeToFire = Random.Range(15,30);
        agent.SetDestination(player.position);
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
            GameObject playerWeapon = GameObject.FindWithTag("PlayerWeapon");
            // playerWeapon.GetComponent<Auto>()
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
        float distancePlayer = Vector3.Distance (gameObject.transform.position, player.transform.position);
        
        if (distancePlayer<rangeToFire+20)
        {
            // agent.isStopped=true;
            // rb.velocity = Vector3.zero;
            // rb.constraints = RigidbodyConstraints.FreezePosition;
            if(Physics.Linecast(transform.position,player.position, out raycast)){
                if (raycast.transform.tag == "Player") // if not object between
                {
                    // rb.velocity = Vector3.zero;
                    // rb.constraints = RigidbodyConstraints.FreezePosition;
                    
                    if (Time.time > fireStart + fireRate) {
                        fireStart = Time.time;
                        fire();
                    }
                    if (distancePlayer<rangeToFire){
                        agent.isStopped=true;
                    }
                }
                }else{
                    agent.isStopped=false;
                }
            // }
           
        }else{
            agent.SetDestination(player.position);
            // rb.constraints = RigidbodyConstraints.None;
            // rb.constraints = RigidbodyConstraints.FreezeRotationX;
            // rb.velocity = transform.forward * speed;

        }
    }

    IEnumerator randomRange(){
        while (isActiveAndEnabled)
        {
            rangeToFire = Random.Range(15,30);
            yield return new WaitForSeconds(Random.Range(10,20));
        }
        yield break;
    }

    void FixedUpdate(){

    }

    void fire(){
        // if (!loadSw)
        // {
            GameObject instance = Instantiate(bullet, transform.position, transform.rotation);
            instance.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.VelocityChange);
            // chargerAmmo--;
        // }
    }
}
