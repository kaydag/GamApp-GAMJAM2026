using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float attackRange = 5f;

    [Header("Damage")]
    [SerializeField] private float damage = 10f;

    [Header("Colliders")]
    [SerializeField] private Collider2D projectileCollider;
    [SerializeField] private Collider2D blockCollider;

    private Vector2 targetPosition;
    private Vector2 startPosition;

    [SerializeField] private bool isMoving = true;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        startPosition = transform.position;

        isMoving = true;

        projectileCollider.enabled = true;
        blockCollider.enabled = false;
        GetComponent<Block>().SetActive(false);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector2 playerPosition = player.transform.position;
        Vector2 direction = (playerPosition - startPosition).normalized;

        targetPosition = startPosition + direction * attackRange;
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.enabled = true;
        }
    }
    private void Update()
    {
        if (!isMoving)
            return;

        Move();

        if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        {
            BecomeBlock();
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    private void BecomeBlock()
    {
        isMoving = false;
        transform.position = targetPosition;
        gameObject.tag = "Wall";
        projectileCollider.enabled = false;
        blockCollider.enabled = true;
        GetComponent<Block>().SetActive(true);
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMoving)
            return;

        if (collision.CompareTag("Player"))
        {
            GameEvent.Attack?.Invoke(
                gameObject,
                collision.gameObject,
                damage
            );

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void Break()
    {
        if (!isMoving)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.woodBreakSound);
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GameEvent.Attack += OnAttack;
    }

    private void OnDisable()
    {
        GameEvent.Attack -= OnAttack;
    }

    private void OnAttack(GameObject attacker, GameObject target, float damage)
    {
        if (target != gameObject)
        {
            return;
        }
        if (!attacker.CompareTag("Player"))
            return;

        if (!isMoving)
        {
            Break();
        }
    }
}