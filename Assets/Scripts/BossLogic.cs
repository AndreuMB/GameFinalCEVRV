using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossLogic : Character
{
    public float followDistance = 5f; // Distancia de seguimiento
    public GameObject attackPrefab;
    public GameObject warningPrefab;// Prefab del ataque
    public Animator animator; // Referencia al Animator

    private Transform player; // Referencia al jugador
    private bool isAttacking = false; // Variable para controlar el estado de ataque

    NavMeshAgent agent;
    GameObject aviso;
    GameObject ataque;
    float tiempoVisual = 1.5f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Buscar al jugador por la etiqueta "Player"
        agent = GetComponent<NavMeshAgent>();
        death.AddListener(BossDeath);
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isAttacking)
        {
            transform.LookAt(player); // Mirar hacia el jugador
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

    // Método llamado desde el script de animación de ataque
    public void AttackEvent()
    {
        
        if (aviso != null)
        {
            ataque = Instantiate(attackPrefab, aviso.transform.position, Quaternion.identity); // Instanciar el Prefab del ataque
            Destroy(aviso);
        }
        Destroy(ataque, tiempoVisual);

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > followDistance)
        {
            // Si estamos fuera del rango de ataque, volver a la animación Idle
            
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsIdle", true);
            agent.isStopped = false;
        }
        else
        {
            // Si seguimos en rango, repetir la animación de ataque
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
        // remove tag for spawn to work
        tag = Tags.UNTAGGED;
        // stop movement enemy
        agent.isStopped = true;
        // disable enemy collision for bullets and other enemies
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject);
    }
}
