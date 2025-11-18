using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameObject skeletonMinion;
    public GameObject skeletonMage;
    public GameObject skeletonWarrior;
    
    private GameObject player;
    private int maxEnemies;
    private float timeSinceIncrease;
    private int enemyCount;
    
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
            timeSinceIncrease = Time.time;
        }
    }

    IEnumerator SpawnMaxEnemies()
    {
        while(true)
        {
            int enemyType =  Random.Range(0, 10);
            if (enemyCount < maxEnemies)
            {
                switch (enemyType)
                {
                    case 0:
                        SpawnEnemy(skeletonMage);
                        break;
                    case 1:
                        SpawnEnemy(skeletonMage);
                        break;
                    case 2:
                        SpawnEnemy(skeletonWarrior);
                        break;
                    case 3:
                        SpawnEnemy(skeletonWarrior);
                        break;
                    default:
                        SpawnEnemy(skeletonMinion);
                        break;
                }
            }
            yield return new WaitForSeconds(1);
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
