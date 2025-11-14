using UnityEngine;
using UnityEngine.AI;

public class SkeletonMinionController : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    public int expValue = 10;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (player)
        {
            transform.LookAt(player.transform);
            agent.SetDestination(player.transform.position);
        }
    }
}
