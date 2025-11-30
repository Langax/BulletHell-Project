using System.Collections;
using UnityEngine;

public class SkeletonMageController : BaseEnemyAI
{
    /* Class derived from BaseEnemyAI, overrises range, movementSpeed & damage variables */

    private float attackCooldown = 0f;
    public GameObject meteorPrefab;
    protected override float range => 20f;
    protected override int damage => 20;
    protected override float movementSpeed => 1f;

    /* Decrease the attack cooldown and check the distance, if in range & attack is not on cooldown, start the attack function. Otherwise just move to the player */
    void Update()
    {
        attackCooldown -= Time.deltaTime;
        
        if (CheckDistance() && attackCooldown <= 0)
        {
            agent.updatePosition = false;
            StartCoroutine(Attack());
            attackCooldown = 4f;
            
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

    /* Stop the agent, trigger the attack animation and spawn a meteor above the player */
    protected override IEnumerator Attack()
    {
        
        agent.isStopped = true;
        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(2);
        
        /* OLD CODE that would instead fire a beam that damages anything in the radius
         
        Vector3 halfextents = new Vector3(1f, 1f, range/2);
        Vector3 center = transform.position + transform.forward * (range * 0.5f);
        Collider[] hits = Physics.OverlapBox(center, halfextents, transform.rotation);
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit the player!");
                player.GetComponent<PlayerController>().takeDamage(damage);
            }
        }*/

        Vector3 spawnPos = new Vector3(player.transform.position.x, player.transform.position.y + 50, player.transform.position.z);
        Quaternion spawnRot = Quaternion.identity;
        Instantiate(meteorPrefab, spawnPos, spawnRot);

        agent.isStopped = false;
    }
    

}
