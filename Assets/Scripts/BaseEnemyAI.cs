using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    protected virtual float range => 5f;
    public int expValue = 10;

    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    protected void MoveToPlayer()
    {
        if (player)
        {
            if (!CheckDistance())
            {
                Vector3 targetPos = player.transform.position;
                targetPos.y = transform.position.y;
            
                transform.LookAt(targetPos);
                agent.SetDestination(player.transform.position);
            }
        }
    }

    protected bool CheckDistance()
    {
        if (player)
        {
            Vector3 distanceDelta = player.transform.position - transform.position;
            if (distanceDelta.sqrMagnitude < range*range)
            {
                return true;
            }
        }
        return false;
    }

    protected virtual IEnumerator Attack()
    {
        Debug.Log("Enemy attacking.");
        agent.isStopped = true;
        
        yield return new WaitForSeconds(1);
        
        float radius = 1.5f;
        Vector3 center = transform.position + transform.forward * 1.5f;
        Collider[] hits = Physics.OverlapSphere(center, radius);
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Hit the player!");
            }
        }
        
        agent.isStopped = false;
    }

    public void TakeDamage()
    {
        Debug.Log("I took damage!");
        //Health checks for stronger enemies here.
        player.GetComponent<PlayerController>().IncreaseExp(expValue);
        gameObject.SetActive(false);
    }


}
