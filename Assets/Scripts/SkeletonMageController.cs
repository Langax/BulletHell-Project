using System.Collections;
using UnityEngine;

public class SkeletonMageController : BaseEnemyAI
{
    private float attackCooldown = 0f;
    protected override float range => 20f;
    void Update()
    {
        attackCooldown -= Time.deltaTime;
        
        if (CheckDistance() && attackCooldown <= 0)
        {
            agent.updatePosition = false;
            StartCoroutine(Attack());
            attackCooldown = 4f;
        }
        else if (!CheckDistance())
        {
            agent.updatePosition = true;
            MoveToPlayer();
        }
    }

    protected override IEnumerator Attack()
    {
        Debug.Log("Mage attacking.");
        agent.isStopped = true;
        
        yield return new WaitForSeconds(2);

        Vector3 halfextents = new Vector3(1f, 1f, range/2);
        Vector3 center = transform.position + transform.forward * (range * 0.5f);
        Collider[] hits = Physics.OverlapBox(center, halfextents, transform.rotation);
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit the player!");
            }
        }
        
        agent.isStopped = false;
    }
    
    private void OnDrawGizmos()
    {
        Vector3 halfExtents = new Vector3(1f, 1f, range/2);
        Vector3 center = transform.position + transform.forward * (range * 0.5f);

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
    }

}
