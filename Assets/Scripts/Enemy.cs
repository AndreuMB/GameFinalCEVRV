using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : Character
{
    // [SerializeField] float life = 20;
    Transform player;
    Transform nexus;
    Transform target;
    // [SerializeField] float speed = 10;
    Rigidbody rb;
    NavMeshAgent agent;
    float rangeToFire;
    RaycastHit raycast;
    // public static UnityEvent death = new UnityEvent();
    public UnityEvent fireEvent = new UnityEvent();
    Transform lastTarget;
    Animator animator;
    bool deathBool = false;
    // [SerializeField] float weaponDamage = 10;
    // Start is called before the first frame update
    void Start()
    {
        InstanciaArmas();
        player = GameObject.FindWithTag("Player").transform;
        nexus = GameObject.FindWithTag("Nexus").transform;
        agent = GetComponent<NavMeshAgent>();
        death.AddListener(enemyDeath);
        rb = GetComponent<Rigidbody>();
        // StartCoroutine(randomRange());
        rangeToFire = Random.Range(15,30);
        setTarget();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if enemy doing death animation not fire
        if (deathBool) return;
        transform.LookAt(target);

        // move check

        // const float OFFSET_DISTANCE_ENEMY_TARGET = 20f;

        // float distanceTarget = Vector3.Distance (gameObject.transform.position, target.transform.position);

        // El jugador no está en rango
        // if (distanceTarget >= rangeToFire + OFFSET_DISTANCE_ENEMY_TARGET)
        // {
            // get closer target and move to it ALWAYS
            setTarget();
            // return;
        // }

        manageBehaviour();
       
    }

    void setTarget(){
        const float PLAYER_PRIORITY = 0.2f;
        // const float OFFSET_DISTANCE_ENEMY_PLAYER = 20f;
        float distancePlayer = Vector3.Distance (gameObject.transform.position, player.transform.position);
        float distanceNexus = Vector3.Distance (gameObject.transform.position, nexus.transform.position);

        target = (distancePlayer>distanceNexus+(distanceNexus * PLAYER_PRIORITY)) ? nexus : player;
        
        // must be in update because target can move
        agent.SetDestination(target.position);

        // when target change do
        // if (target != lastTarget)
        // {
        //     lastTarget = target;
        //     manageBehaviour();
        // }
    }

    void manageBehaviour()
    {
        bool directContact = Physics.Linecast(transform.position,target.position, out raycast);
        float distanceTarget = Vector3.Distance (gameObject.transform.position, target.transform.position);

        if (distanceTarget>rangeToFire)
        {
            agent.isStopped=false;
            animator.SetBool("run",true);
            return;
        }

        // NO tenemos contacto directo || no es con el target
        if(!directContact || raycast.transform.tag != target.tag)
        {
            agent.isStopped=false;
            animator.SetBool("run",true);
            return;
        }

        // all conditions done can stop and shoot
        agent.isStopped=true;
        animator.SetBool("run",false);
        Fire();

    }

    protected override bool decideDamage(Bullet bullet)
    {
        // if is a player return true
        return (bullet.owner.GetType() == typeof(PlayerController));
    }

    IEnumerator randomRange(){
        while (isActiveAndEnabled)
        {
            rangeToFire = Random.Range(15,30);
            yield return new WaitForSeconds(Random.Range(10,20));
        }
        yield break;
    }
    // run animation
    void enemyDeath(){
        // bool to turn off enemy fire in update
        deathBool = true;
        // stop movement enemy
        agent.isStopped=true;
        // disable enemy collision for bullets and other enemies
        GetComponent<Collider>().enabled=false;
        // remove tag for spawn to work
        tag = Tags.UNTAGGED;
        // run animation death
        animator.SetTrigger("death");
    }

    // event trigger when death animation end
    public void endDeathAnimation()
    {
        Destroy(gameObject);
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
