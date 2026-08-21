using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMonster : BaseEnemy
{
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float cooldown = 10f;
    float timer;
    protected override void Awake()
    {
        base.Awake();
        timer = 0f;
    }
    // Start is called before the first frame update

    // Update is called once per frame
    protected override void Update()
    {
        CheckDie();
        SpawnWood();
        Debug.Log("Enemy" + currentHealth);
    }
    void SpawnWood()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown)
        {
            Instantiate(woodPrefab, spawnPoint.position, Quaternion.identity);
            timer = 0f;
        }
    }
}
