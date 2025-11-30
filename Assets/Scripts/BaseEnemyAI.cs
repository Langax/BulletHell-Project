using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    /* Variable declaration, certain variables may be overriden, this Base class is intended to be overriden but doesn't need to be */
    protected NavMeshAgent agent;
    protected GameObject player;
    protected GameStateController gamestate;
    protected virtual float range => 4f;
    protected virtual int damage => 10;
    public int hitsUntilDeath = 1;
    public GameObject blood;
    protected virtual float movementSpeed => 2f;

    protected Animator animator;
    public int expValue = 10;

    /* Set default values */
    void Start()
    {
        player = GameObject.Find("Player");
        gamestate = GameObject.Find("GameState").GetComponent<GameStateController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.speed = movementSpeed;
        animator = GetComponent<Animator>();
    }

    /* As long as the player exists and the enemy is out of range, face towards the player and move there */
    protected void MoveToPlayer()
    {
        if (player)
        {
            if (!CheckDistance())
            {
                /* Face only the Y to avoid looking down at the players feet when getting too close */
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

    /* Returns true or false depending on the distance between the enemy and player, if it's less than the range then return true */
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

    /* Default attack, creates an overlapSphere slightly in front of the enemy and causes damage on the player if it hits */
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

    /* Public function for when an enemy is hit, reduce their health (hitsUntilDeath) by 1, if they have no health left, give the player exp and destroy self
     then update gamestate to reflect the current enemy count */
    public void TakeDamage()
    {
        hitsUntilDeath -= 1;
        Instantiate(blood, transform.position, Quaternion.identity);
        if (hitsUntilDeath <= 0)
        {
            player.GetComponent<PlayerController>().increaseExp(expValue);
            gameObject.SetActive(false);
            gamestate.EnemyDied();
        }
    }
}
