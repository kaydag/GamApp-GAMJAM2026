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
    bool isDead = false;
    private EnemyStateMachine stateMachine;

    private EnemyMoveState moveState;
    private EnemyAttackState attackState;
    private EnemyDieState dieState;
    private EnemyHurtState hurtState;

    private GameObject attackTarget;
    bool playerInRange = false;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHealth = enemyData.maxHealth;
        currentPoint = 0; 
        waypoints = path.waypoints;
        damage = enemyData.damage;
        isDead = false;

        stateMachine = new EnemyStateMachine();
        moveState = new EnemyMoveState(this);
        dieState = new EnemyDieState(this);
        hurtState = new EnemyHurtState(this, 0.2f);
    }
    
    void Start()
    {
        stateMachine.Initialize(moveState);
    }

    private void OnEnable()
    {
        GameEvent.Attack += OnAttack;
    }

    private void OnDisable()
    {
        GameEvent.Attack -= OnAttack;
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.LogicUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
    public virtual void Move()
    {
        if (waypoints.Count == 0) return;
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
        transform.position = Vector2.MoveTowards(transform.position,  targetPoint.position, enemyData.moveSpeed * Time.fixedDeltaTime);
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
            attackTarget = other.gameObject;
            playerInRange = true;
            Debug.Log("Enemy detected Player");
            attackState = new EnemyAttackState(this, attackTarget);
            stateMachine.ChangeState(attackState);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            attackTarget = null;
            Debug.Log("Player left Enemy");
            if (stateMachine.CurrentState is EnemyAttackState)
            {
                stateMachine.ChangeState(moveState);
            }
        }
    }
    public virtual void Attack(GameObject target)
    {
        GameEvent.Attack?.Invoke(gameObject, target, damage);
    }
    private void OnAttack(GameObject attacker, GameObject target, float damage)
    {
        if (target != gameObject) return;
        TakeDamage(damage);
    }
    private void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            isDead = true;
            stateMachine.ChangeState(dieState);
        }
        else stateMachine.ChangeState(hurtState);
    }
    public void Die()
    {
        Destroy(gameObject, 0.5f);
    }
    public void ReturnFromHurt()
    {
        if (playerInRange && attackTarget != null)
        {
            attackState = new EnemyAttackState(this, attackTarget);
            stateMachine.ChangeState(attackState);
        }
        else
        {
            stateMachine.ChangeState(moveState);
        }
    }
}
