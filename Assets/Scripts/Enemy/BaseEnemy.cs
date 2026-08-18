using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] private EnemyPath path;

    //chỉ số
    [SerializeField] private int currentHealth;
    private List<Transform> waypoints;
    int currentPoint;
    int moveDirection = 1;

    //trạng thái
    bool isDead;
    bool isAttacking;
    bool isMoving;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = enemyData.maxHealth;
        currentPoint = 0;
        isDead = false;
        isMoving = true;
        waypoints = path.waypoints;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    protected virtual void Move()
    {
        if (isDead || waypoints.Count == 0) return;
        if (waypoints[currentPoint] == null)
        {
            currentPoint += moveDirection;
            if (currentPoint >= waypoints.Count)
            {
                currentPoint = waypoints.Count - 2;
                moveDirection = -1;
            }
            else if (currentPoint < 0)
            {
                currentPoint = 1;
                moveDirection = 1;
            }
            return;
        }
        Transform targetPoint = waypoints[currentPoint];
        transform.position = Vector2.MoveTowards(transform.position,  targetPoint.position, enemyData.moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.01f)
        {
            currentPoint += moveDirection;
            if (currentPoint >= waypoints.Count)
            {
                currentPoint = waypoints.Count - 2;
                moveDirection = -1;
            }
            else if (currentPoint < 0)
            {
                currentPoint = 1;
                moveDirection = 1;
            }
        }
    }
}
