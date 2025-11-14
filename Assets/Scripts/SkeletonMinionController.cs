using UnityEngine;
using UnityEngine.AI;

public class SkeletonMinionController : BaseEnemyAI
{
    private void Update()
    {
        MoveToPlayer();
        if (CheckDistance())
        {
            Debug.Log("Player is in range, attacking!");
            StartCoroutine(Attack());
        }
    }
}
