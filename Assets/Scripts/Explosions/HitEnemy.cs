using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemy : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseEnemy enemy = collision.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            GameEvent.Attack?.Invoke(transform.root.gameObject, collision.gameObject,damage);
        }
    }
}
