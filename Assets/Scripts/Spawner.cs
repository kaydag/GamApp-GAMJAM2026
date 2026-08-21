using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyName
{
    Normal,
    TreeMonster,
    Boss
}

public class Spawner : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnableObjects;
    [SerializeField] List<EnemyName> waves;
    [SerializeField] float durationSpawn = 2f;

    private Dictionary<EnemyName, GameObject> enemyPrefabs;

    void Awake()
    {
        enemyPrefabs = new Dictionary<EnemyName, GameObject>();
        enemyPrefabs[EnemyName.Normal] = spawnableObjects[0];
        enemyPrefabs[EnemyName.TreeMonster] = spawnableObjects[1];
        enemyPrefabs[EnemyName.Boss] = spawnableObjects[2];
        StartCoroutine(SpawnWave());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator SpawnWave()
    {
        foreach (EnemyName enemyName in waves)
        {
            SpawnEnemy(enemyName);
            yield return new WaitForSeconds(durationSpawn);
        }
    }

    private void SpawnEnemy(EnemyName enemyName)
    {
        if (!enemyPrefabs.TryGetValue(enemyName, out GameObject prefab)) return;
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
