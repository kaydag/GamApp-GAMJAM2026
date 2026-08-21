using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitEffectType
{
    None,
    Burn,
    Stun
}
public class HitEnemy : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [Header("Status Effect")]
    [SerializeField] private HitEffectType effectType;

    [SerializeField] private float duration = 5f;
    [SerializeField] private float damagePerSecond = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameEvent.Attack?.Invoke(transform.root.gameObject, collision.gameObject,damage);
            StatusEffect statusEffect = collision.GetComponent<StatusEffect>();
            if (statusEffect == null) return;
            switch (effectType)
            {
                case HitEffectType.Burn:
                    statusEffect.ApplyBurn(duration, damagePerSecond);
                    break;

                case HitEffectType.Stun:
                    statusEffect.ApplyStun(duration);
                    break;
            }
        }
    }
}
