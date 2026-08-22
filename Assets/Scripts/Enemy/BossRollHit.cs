using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRollHit : MonoBehaviour
{
    private Boss boss;
    private void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        boss.DealRollDamage(collision.gameObject);
        boss.ReturnFromRoll();
    }
}
