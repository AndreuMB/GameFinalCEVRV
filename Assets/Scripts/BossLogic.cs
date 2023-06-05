using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossLogic : Character
{
    public float followDistance = 5f;
    public GameObject attackPrefab;
    public GameObject warningPrefab;
    public Animator animator; 

    private Transform player;
    private bool isAttacking = false; 

    NavMeshAgent agent;
    GameObject aviso;
    GameObject ataque;
    float tiempoVisual = 1.5f;
    public int moneyDrop = 500;
    bool deathBool = false;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        death.AddListener(BossDeath);
    }

    private void Update()
    {
        if (deathBool){
            if (aviso != null)
            {
                Destroy(aviso);
            }
            return;
        }
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            transform.LookAt(player);
            transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
            agent.SetDestination(player.position);
            // Seguir al jugador hasta la distancia establecida
            if (distanceToPlayer <= followDistance)
            {
                agent.isStopped = true;
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsAttacking", true);

                if(aviso == null)
                {
                    aviso = Instantiate(warningPrefab, player.transform.position, Quaternion.identity);
                }
                
            }
        }
    }

    public void AttackEvent()
    {
        
        if (aviso != null)
        {
            ataque = Instantiate(attackPrefab, aviso.transform.position, Quaternion.identity);
            Destroy(aviso);
        }
        Destroy(ataque, tiempoVisual);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > followDistance)
        {
            // Si estamos fuera del rango de ataque, volver a la animaci�n Idle
            
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsIdle", true);
            agent.isStopped = false;
        }
        else
        {
            // Si seguimos en rango, repetir la animaci�n de ataque
            animator.SetBool("IsAttacking", true);
        }
    }

    protected override bool decideDamage(Bullet bullet)
    {
        // if is a player return true
        return (bullet.owner.GetType() == typeof(PlayerController));
    }

    void BossDeath()
    {
        deathBool = true;
        // remove tag for spawn to work
        tag = Tags.UNTAGGED;
        // stop movement enemy
        agent.isStopped = true;
        // disable enemy collision for bullets and other enemies
        GetComponent<Collider>().enabled = false;
        // Destroy(gameObject);
        animator.SetTrigger("death");
        // FindObjectOfType<WaveManager>().checkWave();
    }

    // event trigger when death animation end
    public void endDeathAnimation()
    {
        Destroy(gameObject);
    }
}
