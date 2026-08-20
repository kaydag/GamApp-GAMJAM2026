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

    [SerializeField] protected Animator animator;

    protected GameObject attackTarget;
    protected bool playerInRange = false;
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

        animator = GetComponent<Animator>();
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
    }

    protected virtual void FixedUpdate()
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
        Vector2 direction = targetPoint.position - transform.position;
        UpdateAnimationDirection(direction);
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
    public void ReturnFromIdle()
    {
        stateMachine.ChangeState(moveState);
    }
    public void PlayIdleAnimation()
    {
        animator.Play("BaseEnemyIdle");
    }
    public void PlayMoveAnimation()
    {
        animator.Play("BaseEnemyMove");
    }
    private void UpdateAnimationDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetFloat("Horizontal", direction.x > 0 ? 1 : -1);
            animator.SetFloat("Vertical", 0);
        }
        else
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical",direction.y > 0 ? 1 : -1);
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public float GetMaxHealth()
    {
        return enemyData.maxHealth;
    }
}

