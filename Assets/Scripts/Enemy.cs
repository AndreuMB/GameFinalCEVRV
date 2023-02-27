using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : Character
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
    // [SerializeField] float weaponDamage = 10;
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
        if (other.gameObject.tag==Tags.BULLET)
        {


            // Bullet bullet = other.gameObject.GetComponent<Bullet>();

            // if (bullet.isFromEnemy)
            // {
                
            // }

            // GameObject playerWeapon = GameObject.FindWithTag("PlayerWeapon");
            // float damageWeapon = playerWeapon.GetComponent<Weapon>().getDamage();
            float damageWeapon = other.gameObject.GetComponent<Bullet>().getWeaponDamage();
            life = life-damageWeapon;
            print("life = " + life);
            if (life <= 0)
            {
                death.Invoke();
                Destroy(gameObject);
                return;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        const float PLAYER_PRIORITY = 0.2f;
        // const float OFFSET_DISTANCE_ENEMY_PLAYER = 20f;
        float distancePlayer = Vector3.Distance (gameObject.transform.position, player.transform.position);
        float distanceNexus = Vector3.Distance (gameObject.transform.position, nexus.transform.position);

        target = (distancePlayer>distanceNexus+(distanceNexus * PLAYER_PRIORITY)) ? nexus : player;

        transform.LookAt(target);

        manageBehaviour();

        Fire();

        
        // if (distancePlayer<rangeToFire+OFFSET_DISTANCE_ENEMY_PLAYER)
        // {
        //     // agent.isStopped=true;
        //     // rb.velocity = Vector3.zero;
        //     // rb.constraints = RigidbodyConstraints.FreezePosition;
        //     if(Physics.Linecast(transform.position,target.position, out raycast)){
        //         if (raycast.transform.tag == target.tag) // if not object between
        //         {
        //             // rb.velocity = Vector3.zero;
        //             // rb.constraints = RigidbodyConstraints.FreezePosition;
        //             fireEvent.Invoke();
        //             // if (Time.time > fireStart + fireRate) {
        //             //     fireStart = Time.time;
        //             //     fire();
        //             // }
        //             if (distancePlayer<rangeToFire){
        //                 agent.isStopped=true;
        //             }
        //         }
        //         }else{
        //             fireEvent.Invoke();
        //             agent.isStopped=false;
        //         }
        //     // }
           
        // }else{
        //     agent.SetDestination(target.position);
        //     // rb.constraints = RigidbodyConstraints.None;
        //     // rb.constraints = RigidbodyConstraints.FreezeRotationX;
        //     // rb.velocity = transform.forward * speed;

        // }
    }

    void manageBehaviour()
    {
        const float OFFSET_DISTANCE_ENEMY_PLAYER = 20f;

        float distancePlayer = Vector3.Distance (gameObject.transform.position, player.transform.position);

        // El jugador no está en rango
        if (distancePlayer >= rangeToFire + OFFSET_DISTANCE_ENEMY_PLAYER)
        {
            agent.SetDestination(target.position);
            return;
        }

        // directContact => Tenemos contacto directo con el objetivo / not object between
        bool directContact = Physics.Linecast(transform.position,target.position, out raycast);
        // NO tenemos contacto directo
        if(!directContact)
        {
            fireEvent.Invoke();
            agent.isStopped=false;
            return;
        }

        // Si no es el jugador no hacemos nada
        if (raycast.transform.tag != target.tag) return;

        // Se cumplen todas las condiciones

        // rb.velocity = Vector3.zero;
        // rb.constraints = RigidbodyConstraints.FreezePosition;
        fireEvent.Invoke();
        // if (Time.time > fireStart + fireRate) {
        //     fireStart = Time.time;
        //     fire();
        // }
        if (distancePlayer<rangeToFire) {
            agent.isStopped=true;
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

    // void fire(){
    //     // if (!loadSw)
    //     // {
    //         GameObject instance = Instantiate(bullet, transform.position, transform.rotation);
    //         instance.GetComponent<Rigidbody>().AddForce(transform.forward * 200, ForceMode.VelocityChange);
    //         // chargerAmmo--;
    //     // }
    // }

    // public float getWeaponDamage(){
    //     return weaponDamage;
    // }
}
