using System.Collections;
using UnityEngine;

public class SkeletonWarriorController : BaseEnemyAI
{
    private float attackCooldown = 0f;
    protected override int damage => 30;
    protected override float movementSpeed => 5f;
    
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
