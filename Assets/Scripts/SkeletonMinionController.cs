using UnityEngine;
using UnityEngine.AI;

public class SkeletonMinionController : BaseEnemyAI
{
    private float attackCooldown = 0f;
    
    
    private void Update()
    {
        attackCooldown -= Time.deltaTime;
        
        MoveToPlayer();
        if (CheckDistance() && attackCooldown <= 0f)
        {
            StartCoroutine(Attack());
            attackCooldown = 3f;
        }
    }
    
    private void OnDrawGizmos()
    {
        Vector3 attackCenter = transform.position + transform.forward * 1.5f;
        float attackRadius = 1.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCenter, attackRadius);
    }
}
