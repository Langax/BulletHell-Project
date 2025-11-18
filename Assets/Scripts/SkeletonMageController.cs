using System.Collections;
using UnityEngine;

public class SkeletonMageController : BaseEnemyAI
{
    private float attackCooldown = 0f;
    protected override float range => 20f;
    protected override int damage => 20;
    protected override float movementSpeed => 1f;

    
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

    protected override IEnumerator Attack()
    {
        agent.isStopped = true;
        animator.SetTrigger("Attacking");
        yield return new WaitForSeconds(2);

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
        }
        
        agent.isStopped = false;
    }
    

}
