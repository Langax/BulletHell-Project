using System.Collections;
using UnityEngine;

public class SkeletonWarriorController : BaseEnemyAI
{
    /* Class derived from BaseEnemyAI, overrides damage & movementSpeed */ 
    private float attackCooldown = 0f;
    protected override int damage => 30;
    protected override float movementSpeed => 5f;
    
    /* Move towards the player, once in range and attack is off cooldown, trigger the inherited attack with overriden damage */
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
