using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField] float life = 20;
    Transform player;
    Transform nexus;
    Transform target;
    // [SerializeField] float speed = 10;
    Rigidbody rb;
    [SerializeField] GameObject bullet;
    [SerializeField] float fireRate = 0.5f;
    NavMeshAgent agent;
    float fireStart;
    float rangeToFire;
    RaycastHit raycast;
    public static UnityEvent death = new UnityEvent();
    public UnityEvent fireEvent = new UnityEvent();
    [SerializeField] float weaponDamage = 10;
    // Start is called before the first frame update
    void Start()
    {
        // Bullet.hit.AddListener(damage);
        player = GameObject.Find("Player").transform;
        nexus = GameObject.Find("Nexus").transform;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        // StartCoroutine(randomRange());
        rangeToFire = Random.Range(15,30);
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
            // GameObject playerWeapon = GameObject.FindWithTag("PlayerWeapon");
            // float damageWeapon = playerWeapon.GetComponent<Weapon>().getDamage();
            float damageWeapon = other.gameObject.GetComponent<Bullet>().getWeaponDamage();
            life = life-damageWeapon;
            print("life = " + life);
            if (life <= 0)
            {
                Destroy(gameObject);
                death.Invoke();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distancePlayer = Vector3.Distance (gameObject.transform.position, player.transform.position);
        float distanceNexus = Vector3.Distance (gameObject.transform.position, nexus.transform.position);

        target = (distancePlayer>distanceNexus+(distanceNexus*20/100)) ? nexus : player;

        transform.LookAt(target);
        
        if (distancePlayer<rangeToFire+20)
        {
            // agent.isStopped=true;
            // rb.velocity = Vector3.zero;
            // rb.constraints = RigidbodyConstraints.FreezePosition;
            if(Physics.Linecast(transform.position,target.position, out raycast)){
                if (raycast.transform.tag == target.tag) // if not object between
                {
                    // rb.velocity = Vector3.zero;
                    // rb.constraints = RigidbodyConstraints.FreezePosition;
                    fireEvent.Invoke();
                    // if (Time.time > fireStart + fireRate) {
                    //     fireStart = Time.time;
                    //     fire();
                    // }
                    if (distancePlayer<rangeToFire){
                        agent.isStopped=true;
                    }
                }
                }else{
                    fireEvent.Invoke();
                    agent.isStopped=false;
                }
            // }
           
        }else{
            agent.SetDestination(target.position);
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

    public float getWeaponDamage(){
        return weaponDamage;
    }
}
