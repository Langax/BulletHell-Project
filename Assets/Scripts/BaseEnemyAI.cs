using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    protected GameStateController gamestate;
    protected virtual float range => 4f;
    protected virtual int damage => 10;
    protected int hitsUntilDeath = 3;
    protected virtual float movementSpeed => 2f;

    protected Animator animator;
    public int expValue = 10;

    void Start()
    {
        player = GameObject.Find("Player");
        gamestate = GameObject.Find("GameState").GetComponent<GameStateController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = movementSpeed;
        animator = GetComponent<Animator>();

    }

    protected void MoveToPlayer()
    {
        if (player)
        {
            if (!CheckDistance())
            {
                Vector3 targetPos = player.transform.position;
                targetPos.y = transform.position.y;
            
                transform.LookAt(targetPos);
                
                agent.SetDestination(player.transform.position);
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    protected bool CheckDistance()
    {
        if (player)
        {
            Vector3 distanceDelta = player.transform.position - transform.position;
            if (distanceDelta.sqrMagnitude < range*range)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual IEnumerator Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(1);
        
        float radius = 1.5f;
        Vector3 center = transform.position + transform.forward * 1.5f;
        Collider[] hits = Physics.OverlapSphere(center, radius);
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                player.GetComponent<PlayerController>().takeDamage(damage);
            }
        }
        
        agent.isStopped = false;
    }

    public void TakeDamage()
    {
        hitsUntilDeath -= 1;

        if (hitsUntilDeath <= 0)
        {
            player.GetComponent<PlayerController>().IncreaseExp(expValue);
            gameObject.SetActive(false);
            gamestate.EnemyDied();
        }
    }


}
