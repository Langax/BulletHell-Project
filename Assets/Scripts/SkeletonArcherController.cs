using System.Collections;
using UnityEngine;

public class SkeletonArcherController : BaseEnemyAI
{
    /* Class derived from BaseEnemyAI, overrises range & movementSpeed variables */
    private float attackCooldown;
    public GameObject arrowPrefab;
    protected override float range => 10f;
    protected override float movementSpeed => 2f;

    /* Decrease the attack cooldown and check the distance, if in range & attack is not on cooldown, start the attack function. Otherwise just move to the player */
    void Update()
    {
        attackCooldown -= Time.deltaTime;
        
        if (CheckDistance() && attackCooldown <= 0)
        {
            agent.updatePosition = false;
            StartCoroutine(Attack());
            attackCooldown = 3f;
            
            Vector3 targetPos = player.transform.position;
            targetPos.y = transform.position.y;
            
            transform.LookAt(targetPos);
        }
        else if (!CheckDistance())
        {
            agent.updatePosition = true;
            MoveToPlayer();
        }
    }
    
    /* Stop the agent and trigger the animation, then shoot an arrow towards the player */
    protected override IEnumerator Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(1);
 

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        Quaternion spawnRot = transform.rotation;
        Instantiate(arrowPrefab, spawnPos, spawnRot);

        agent.isStopped = false;
    }
}
