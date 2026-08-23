using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFormAttack : MonoBehaviour, IFormAttack
{
    [Header("References")]
    [SerializeField] private Sprite SwapIcon;
    [SerializeField] private GameObject StartNormalAttackEffect;
    [SerializeField] private GameObject NormalAttackEffect;
    [SerializeField] private Sprite NormalAttackIcon;
    [SerializeField] private GameObject FirstSkillEffect;
    [SerializeField] private Sprite FirstSkillIcon;
    [Header("Cooldowns")]
    [SerializeField] private float normalAttackCooldown = 0.5f;
    [SerializeField] private float firstSkillCooldown = 5f;
    private IEnumerator DelayedSpawnEffect(Vector2 direction)
    {
        yield return new WaitForSeconds(0.25f);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90f);
        GameObject effect = Instantiate(NormalAttackEffect, StartNormalAttackEffect.transform.position, rotation);
        if (effect != null) Destroy(effect, 1f);
    }
    public void NormalAttack(Vector2 direction)
    {
        PlayerAttack.instance.SetNormalCooldown(this, normalAttackCooldown + 0.5f);
        PlayerAttack.instance.ChangeAttackState();
        StartCoroutine(DelayedSpawnEffect(direction));
    }
    public void FirstSkill(Vector2 direction)
    {
        PlayerAttack.instance.SetSkillCooldown(this, 1.25f, firstSkillCooldown);

        GameObject effect = Instantiate(FirstSkillEffect, transform.position, Quaternion.identity);
        if (effect != null) Destroy(effect, 1f);

        PlayerAttack.instance.ChangeSkillState();
    }
    public Sprite GetNormalAttackIcon() => NormalAttackIcon;
    public Sprite GetFirstSkillIcon() => FirstSkillIcon;
    public Sprite GetSwapIcon() => SwapIcon;
    public float GetFirstSkillCooldown() => firstSkillCooldown;
    public float GetRemainingFirstSkillCooldown() => 0f;
    public float GetRemainingNormalAttackCooldown() => 0f;
}