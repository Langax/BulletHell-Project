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
            Vector3 targetPos = player.transform.position;
            targetPos.y = transform.position.y;
            transform.LookAt(targetPos);
        }
    }
}
