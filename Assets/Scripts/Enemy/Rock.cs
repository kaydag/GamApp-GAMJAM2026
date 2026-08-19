using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Rock : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10f;

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject target = collision.gameObject;
            GameEvent.Attack?.Invoke(gameObject, target, damage);
            Destroy(gameObject);
        }

        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void Move()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
