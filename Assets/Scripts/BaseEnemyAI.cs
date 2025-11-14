using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemyAI : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected GameObject player;
    protected int range = 5;
    public int expValue = 10;

    void Start()
    {
        player = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        GetComponent<BoxCollider>().enabled = false;
    }

    protected void MoveToPlayer()
    {
        if (player)
        {
            transform.LookAt(player.transform);
            agent.SetDestination(player.transform.position);
        }
    }

    protected bool CheckDistance()
    {
        if (player)
        {
            Vector3 distanceDelta = player.transform.position - transform.position;
            if (distanceDelta.sqrMagnitude < range)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    protected IEnumerator Attack()
    {
        Debug.Log("Enemy attacking.");
        BoxCollider attackBox = GetComponent<BoxCollider>();
        //attackBox.enabled = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        yield return new WaitForSeconds(1);
        attackBox.enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

    }
}
