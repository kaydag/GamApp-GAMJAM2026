using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFormAttack : MonoBehaviour, IFormAttack
{
    [Header("References")]
    [SerializeField] private Sprite SwapIcon;
    [SerializeField] private GameObject StartNormalAttackEffect;
    [SerializeField] private GameObject NormalAttackEffect;
    [SerializeField] private Sprite NormalAttackIcon;
    [SerializeField] private GameObject FirstSkillEffect;
    [SerializeField] private Sprite FirstSkillIcon;
    [Header("Cooldowns")]
    [SerializeField] private float normalAttackCooldown = 0.2f;
    [SerializeField] private float firstSkillCooldown = 5f;
    private float nextNormalAttackTime = 0f;
    private float nextFirstSkillTime = 0f;
    public void NormalAttack(Vector2 direction)
    {
        // Kiểm tra xem đã hết thời gian hồi chiêu chưa
        if (Time.time < nextNormalAttackTime) return;
        // Cập nhật mốc thời gian được phép đánh tiếp theo
        nextNormalAttackTime = Time.time + normalAttackCooldown;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90f);
        GameObject effect = Instantiate(NormalAttackEffect, StartNormalAttackEffect.transform.position, rotation);
        if (effect != null)
        {
            Destroy(effect, 1f);
        }
    }
    public void FirstSkill(Vector2 direction)
    {
        // Kiểm tra cooldown riêng cho FirstSkill
        if (Time.time < nextFirstSkillTime) return;
        nextFirstSkillTime = Time.time + firstSkillCooldown;
        GameObject effect = Instantiate(FirstSkillEffect, transform.position, Quaternion.identity);
        if (effect != null)
        {
            Destroy(effect, 1f);
        }
    }
    public Sprite GetNormalAttackIcon()
    {
        return NormalAttackIcon;
    }
    public Sprite GetFirstSkillIcon()
    {
        return FirstSkillIcon;
    }
    public Sprite GetSwapIcon()
    {
        return SwapIcon;
    }
}
