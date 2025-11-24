using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameObject skeletonMinion;
    public GameObject skeletonMage;
    public GameObject skeletonWarrior;
    public GameObject skeletonArcher;
    
    private GameObject player;
    private int maxEnemies;
    private float timeSinceIncrease;
    private int enemyCount;
    private float timeBetweenSpawn = 2f;
    
    void Start()
    {
        player =  GameObject.Find("Player");
        timeSinceIncrease = Time.time;
        maxEnemies = 10;
        enemyCount = 0;

    }

    void Update()
    {
        float nowTime = Time.time;

        if (enemyCount == 0)
        {
            StartCoroutine(SpawnMaxEnemies());
        }
        
        if (nowTime - timeSinceIncrease > 60)
        {
            maxEnemies += 10;
            timeBetweenSpawn -= 0.2f;
            timeSinceIncrease = Time.time;
        }
    }

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

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 direction = Random.insideUnitCircle.normalized;
        float dist = Random.Range(20, 30);
        Vector3 pos = player.transform.position + new Vector3(direction.x, 0, direction.y) * dist;
                
        Instantiate(enemyPrefab, pos, Quaternion.identity);
        enemyCount++;
    }

    public void EnemyDied()
    {
        enemyCount--;
    }
}
