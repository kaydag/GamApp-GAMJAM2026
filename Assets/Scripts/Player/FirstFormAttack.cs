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
    private float nextNormalAttackTime = 0f;
    private float nextFirstSkillTime = 0f;
    public void NormalAttack(Vector2 direction)
    {
        if (Time.time < nextNormalAttackTime) return;
        nextNormalAttackTime = Time.time + normalAttackCooldown + 0.5f;
        PlayerAttack.instance.ChangeAttackState();
        StartCoroutine(DelayedSpawnEffect(direction));
    }
    private IEnumerator DelayedSpawnEffect(Vector2 direction)
    {
        yield return new WaitForSeconds(0.25f);
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
        nextNormalAttackTime = Time.time + 1.25f;
        GameObject effect = Instantiate(FirstSkillEffect, transform.position, Quaternion.identity);
        if (effect != null)
        {
            Destroy(effect, 1f);
        }
        PlayerAttack.instance.ChangeSkillState();
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

    //xử lí phá khối gỗ của enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Wood wood = collision.GetComponent<Wood>();
        if (wood != null)
        {
            wood.Break();
        }
    }
}
