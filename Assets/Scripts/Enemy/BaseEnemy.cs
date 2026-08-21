using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] protected EnemyData enemyData;
    [SerializeField] protected EnemyPath path;

    //chỉ số
    [SerializeField] protected float currentHealth;
    protected List<Transform> waypoints;
    protected int currentPoint;
    protected int moveDirection = 1;
    protected float damage;

    //trạng thái
    protected bool isDead = false;
    protected EnemyStateMachine stateMachine;

    protected EnemyMoveState moveState;
    protected EnemyAttackState attackState;
    protected EnemyDieState dieState;
    protected EnemyHurtState hurtState;
    protected EnemyIdleState idleState;
    protected EnemyStunState stunState;

    public EnemyAnimation Animation { get; private set; }

    protected GameObject attackTarget;
    protected bool playerInRange = false;

    StatusEffect statusEffect;
    public bool IsStunned { get; private set; }
    // Start is called before the first frame update
    protected virtual void Awake()
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
        idleState = new EnemyIdleState(this, 0.5f);
        stunState = new EnemyStunState(this);
        attackState = new EnemyAttackState(this, null);

        Animation = GetComponent<EnemyAnimation>();
        statusEffect = GetComponent<StatusEffect>();
    }

    protected virtual void Start()
    {
        stateMachine.Initialize(idleState);
    }

    protected virtual void OnEnable()
    {
        GameEvent.Attack += OnAttack;
    }

    protected virtual void OnDisable()
    {
        GameEvent.Attack -= OnAttack;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.LogicUpdate();
        CheckDie();
    }

    protected virtual void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
    public virtual void Move()
    {
        if (waypoints.Count == 0 || IsStunned) return;
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
        Vector2 direction = targetPoint.position - transform.position;
        Animation.SetDirection(direction);
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, enemyData.moveSpeed * Time.fixedDeltaTime);
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
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        GameEvent.HealthChanged?.Invoke(gameObject, currentHealth, enemyData.maxHealth);
        if (currentHealth > 0)
        {
            stateMachine.ChangeState(hurtState);
        }
        else CheckDie();
    }
    public void Die()
    {
        GameEvent.EnemyDie?.Invoke(this);
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
    public void ReturnFromIdle()
    {
        stateMachine.ChangeState(moveState);
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return enemyData.maxHealth;
    }
    protected void CheckDie()
    {
        if (isDead) return;
        if (currentHealth <= 0)
        {
            isDead = true;
            stateMachine.ChangeState(dieState);
            Die();
        }
    }
    public void ApplyBurn(float duration, float damage)
    {
        statusEffect.ApplyBurn(duration, damage);
    }
    public void SetStunned(bool value)
    {
        IsStunned = value;
        if (isDead) return;
        if (value)
        {
            stateMachine.ChangeState(stunState);
        }
        else
        {
            ReturnFromStun();
        }
    }
    public void ReturnFromStun()
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
    public void TakeDamage(float damage, bool triggerHurt)
    {
        if (isDead) return;
        currentHealth -= damage;
        GameEvent.HealthChanged?.Invoke(gameObject,currentHealth,enemyData.maxHealth);
        if (currentHealth <= 0)
        {
            CheckDie();
            return;
        }
        if (triggerHurt)
        {
            stateMachine.ChangeState(hurtState);
        }
    }
}

