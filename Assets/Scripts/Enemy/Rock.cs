using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Rock : MonoBehaviour
{
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float damage = 10f;

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

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject target = collision.gameObject;
            GameEvent.Attack?.Invoke(gameObject, target, damage);
            Break();
        }

        else if (collision.CompareTag("Wall"))
        {
            Break();
        }
    }

    private void Move()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
    private void Break()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.rockBreakSound);
        Destroy(gameObject);
    }
}
