using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damage = 10f;
    [SerializeField] private float attackRange = 5f;
    private Vector2 targetPosition;
    private Vector2 startPosition;
    private bool isMoving = true;
    private void Awake()
    {
        startPosition = transform.position;
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
        if (!isMoving) return;
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
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMoving)
        {
            ICollidable collidable = collision.GetComponent<ICollidable>();
            if (collidable != null)
            {
                collidable.OnCollide();
            }
            return;
        }
        if (collision.CompareTag("Player"))
        {
            GameEvent.Attack?.Invoke(gameObject, collision.gameObject, damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isMoving)
        {
            ICollidable collidable = collision.GetComponent<ICollidable>();
            if (collidable != null)
            {
                collidable.OnCollide();
            }
        }
    }
    public void Break()
    {
        if (!isMoving) Destroy(gameObject);
    }
}