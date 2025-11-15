using System.Collections;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    public GameObject enemyPrefab;
    
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
            StartCoroutine(spawnMaxEnemies());
        }
        
        if (nowTime - timeSinceIncrease > 60)
        {
            maxEnemies += 10;
            timeSinceIncrease = Time.time;
        }
    }

    IEnumerator spawnMaxEnemies()
    {
        while(true)
        {
            if (enemyCount < maxEnemies)
            {
                Vector3 direction = Random.insideUnitCircle.normalized;
                float dist = Random.Range(20, 30);
                Vector3 pos = player.transform.position + new Vector3(direction.x, 0, direction.y) * dist;
                
                Instantiate(enemyPrefab, pos, Quaternion.identity);
                enemyCount++;
            }
            yield return new WaitForSeconds(3);
        }
    }
    
    // todo: update this to reduce the enemy count when a skeleton is killed.
}
