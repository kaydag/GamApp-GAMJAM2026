using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    [SerializeField] private GameObject HP_RedBar;
    [SerializeField] private GameObject HP_WhiteBar;
    [Header("Animation Settings")]
    [SerializeField] private float whiteBarDelay = 0.2f;
    [SerializeField] private float whiteBarDuration = 0.4f;
    private BaseEnemy enemy;
    // Lưu lại scaleX gốc của từng thanh bar khi khởi tạo
    private float redBarInitialScaleX;
    private float whiteBarInitialScaleX;
    private void Start()
    {
        enemy = GetComponentInParent<BaseEnemy>();

        if (HP_RedBar != null) redBarInitialScaleX = HP_RedBar.transform.localScale.x;
        if (HP_WhiteBar != null) whiteBarInitialScaleX = HP_WhiteBar.transform.localScale.x;

        GameEvent.Attack += OnAttackEvent;
    }
    private void OnDestroy()
    {
        GameEvent.Attack -= OnAttackEvent;
    }
    private void OnAttackEvent(GameObject attacker, GameObject target, float damage)
    {
        GameObject enemyObj = transform.parent != null ? transform.parent.gameObject : gameObject;
        if (target != enemyObj) return;
        float maxHp = enemy.GetMaxHealth();
        if (maxHp <= 0) return;
        float targetPercent = Mathf.Clamp01(enemy.GetCurrentHealth() / maxHp);
        HP_RedBar.transform.DOKill();
        HP_RedBar.transform.DOScaleX(targetPercent * redBarInitialScaleX, 0.1f);
        HP_WhiteBar.transform.DOKill();
        HP_WhiteBar.transform.DOScaleX(targetPercent * whiteBarInitialScaleX, whiteBarDuration)
            .SetDelay(whiteBarDelay)
            .SetEase(Ease.OutQuad);
    }
}