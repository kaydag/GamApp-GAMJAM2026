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

    private bool isMoving = true;

    private void Awake()
    {
        startPosition = transform.position;

        // Trạng thái ban đầu: Wood đang bay
        projectileCollider.enabled = true;
        blockCollider.enabled = false;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 playerPosition = player.transform.position;
        Vector2 toPlayer = playerPosition - startPosition;

        if (toPlayer.magnitude <= attackRange)
        {
            targetPosition = playerPosition;
        }
        else
        {
            targetPosition = startPosition + toPlayer.normalized * attackRange;
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
            return;

        if (!isMoving)
        {
            Break();
        }
    }
}