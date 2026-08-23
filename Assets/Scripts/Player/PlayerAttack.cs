using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack instance;
    private IFormAttack currentFormAttack;
    private List<IFormAttack> FormAttacks = new List<IFormAttack>();
    private PlayerController playerController;
    [Header("UI References")]
    [SerializeField] private Image SwapButton;
    [SerializeField] private Image NormalButton;
    [SerializeField] private Image SkillButton;
    [SerializeField] private CooldownCounter skillCooldownUI;
    private Dictionary<IFormAttack, float> formNormalCooldowns = new Dictionary<IFormAttack, float>();
    private Dictionary<IFormAttack, float> formSkillCooldowns = new Dictionary<IFormAttack, float>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else { Destroy(gameObject); return; }

        GetComponents<IFormAttack>(FormAttacks);
        foreach (var form in FormAttacks)
        {
            formNormalCooldowns[form] = 0f;
            formSkillCooldowns[form] = 0f;
        }

        if (FormAttacks.Count > 0) currentFormAttack = FormAttacks[0];
        playerController = GetComponent<PlayerController>();
    }
    public void DoNormalAttack()
    {
        if (currentFormAttack == null) return;
        if (Time.time < formNormalCooldowns[currentFormAttack]) return;

        currentFormAttack.NormalAttack(playerController.LastDirection);
    }
    public void DoFirstSkill()
    {
        if (currentFormAttack == null) return;
        if (Time.time < formSkillCooldowns[currentFormAttack]) return;

        currentFormAttack.FirstSkill(playerController.LastDirection);
    }
    public void SetNormalCooldown(IFormAttack form, float normalCD)
    {
        if (formNormalCooldowns.ContainsKey(form))
            formNormalCooldowns[form] = Time.time + normalCD;
    }
    public void SetSkillCooldown(IFormAttack form, float normalCD, float skillCD)
    {
        if (formNormalCooldowns.ContainsKey(form))
            formNormalCooldowns[form] = Mathf.Max(formNormalCooldowns[form], Time.time + normalCD);
        if (formSkillCooldowns.ContainsKey(form))
            formSkillCooldowns[form] = Time.time + skillCD;
        if (form == currentFormAttack && skillCooldownUI != null)
        {
            skillCooldownUI.TriggerCooldown(skillCD);
        }
    }
    public void ChangeAttackState() => playerController.StateMachine.ChangeState<PlayerAttackState>();
    public void ChangeSkillState() => playerController.StateMachine.ChangeState<PlayerSkillState>();
    public void SwapFormAttack()
    {
        int index = PlayerController.instance.isInSecondForm ? 1 : 0;
        if (index < FormAttacks.Count)
        {
            currentFormAttack = FormAttacks[index];
            SwapButton.sprite = currentFormAttack.GetSwapIcon();
            NormalButton.sprite = currentFormAttack.GetNormalAttackIcon();
            SkillButton.sprite = currentFormAttack.GetFirstSkillIcon();
            if (skillCooldownUI != null)
            {
                float nextTime = formSkillCooldowns[currentFormAttack];
                float remaining = Mathf.Max(0f, nextTime - Time.time);
                float total = currentFormAttack.GetFirstSkillCooldown();

                skillCooldownUI.SyncCooldown(remaining, total);
            }
        }
    }
}