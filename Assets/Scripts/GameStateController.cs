using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    /* Variable declaration */
    public GameObject skeletonMinion;
    public GameObject skeletonMage;
    public GameObject skeletonWarrior;
    public GameObject skeletonArcher;
    
    private GameObject player;
    private int maxEnemies;
    private float timeSinceIncrease;
    private int enemyCount;
    private float timeBetweenSpawn = 2f;
    
    /* Initialize default values */
    void Start()
    {
        player =  GameObject.Find("Player");
        timeSinceIncrease = Time.time;
        maxEnemies = 10;
        enemyCount = 0;
        
        /* Begin an infinite loop to keep spawning enemies */
        StartCoroutine(SpawnMaxEnemies());
    }

    /* Every 60 seconds, increase the amount of max enemies, and decrease the time between the spawning */
    void Update()
    {
        float nowTime = Time.time;
        
        if (nowTime - timeSinceIncrease > 60)
        {
            maxEnemies += 10;
            timeBetweenSpawn -= 0.2f;
            timeSinceIncrease = Time.time;
        }
    }

    /* Randomly pick an enemy type, and call the spawn function for it, then wait before spawning the next one */
    IEnumerator SpawnMaxEnemies()
    {
        while(true)
        {
            int enemyType =  Random.Range(0, 20);
            if (enemyCount < maxEnemies)
            {
                switch (enemyType)
                {
                    case 0:
                        SpawnEnemy(skeletonMage);
                        break;
                    case 1:
                        SpawnEnemy(skeletonWarrior);
                        break;
                    case 2:
                        SpawnEnemy(skeletonArcher);
                        break;
                    case 3:
                        SpawnEnemy(skeletonArcher);
                        break;
                    default:
                        SpawnEnemy(skeletonMinion);
                        break;
                }
            }
            yield return new WaitForSeconds(timeBetweenSpawn);
        }
    }

    /* Spawn an enemy in a random direction from the player at a distance of 20-30 units away, then increase the enemy count */
    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 direction = Random.insideUnitCircle.normalized;
        float dist = Random.Range(20, 30);
        Vector3 pos = player.transform.position + new Vector3(direction.x, 0, direction.y) * dist;
                
        Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemyCount++;
    }

    /* Decrement enemyCount to keep it accurate */
    public void EnemyDied()
    {
        enemyCount--;
    }
}
