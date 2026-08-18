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
    [SerializeField] private float currentHealth;
    private List<Transform> waypoints;
    int currentPoint;
    int moveDirection = 1;
    float damage;

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
        damage = enemyData.damage;
    }
    private void OnEnable()
    {
        GameEvent.Attack += OnAttack;
    }

    private void OnDisable()
    {
        GameEvent.Attack -= OnAttack;
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
        else if (isAttacking)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Attack(player);
            }
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy is attacking the player!");
            isAttacking = true;
            isMoving = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Enemy stopped attacking the player!");
            isAttacking = false;
            isMoving = true;
        }
    }
    protected virtual void Attack(GameObject target)
    {
        GameEvent.Attack?.Invoke(gameObject, target, damage);
        Debug.Log("Attack Enemy");
    }
    private void OnAttack(GameObject attacker, GameObject target, float damage)
    {
        if (target != gameObject || isDead) return;
        TakeDamage(damage);
    }
    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && !isDead) Die();
    }
    private void Die()
    {
        isDead = true;
        isMoving = false;
        isAttacking = false;
        Destroy(gameObject, 0.5f);
    }
}
