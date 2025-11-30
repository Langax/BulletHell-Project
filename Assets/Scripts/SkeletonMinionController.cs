using UnityEngine;
using UnityEngine.AI;

public class SkeletonMinionController : BaseEnemyAI
{
    /* Basic class derived from BaseEnemyAI */

    private float attackCooldown;
    
    /* Move to the player, once in range and the attack is off cooldown, trigger the inherited attack */
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
