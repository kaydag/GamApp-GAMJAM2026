using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Singleton
    public static PlayerAttack instance;
    private IFormAttack currentFormAttack;
    private List<IFormAttack> FormAttacks = new List<IFormAttack>();
    private PlayerController playerController;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // Tự động tìm tất cả các script implement IFormAttack đang nằm trên object này
        GetComponents<IFormAttack>(FormAttacks);
        if (FormAttacks.Count > 0)
        {
            currentFormAttack = FormAttacks[0];
        }
        playerController = GetComponent<PlayerController>();
    }
    public void DoNormalAttack() => currentFormAttack?.NormalAttack(playerController.LastDirection);
    public void DoFirstSkill() => currentFormAttack?.FirstSkill(playerController.LastDirection);
    public void SwapFormAttack()
    {
        int index = PlayerController.instance.isInSecondForm ? 1 : 0;
        if (index < FormAttacks.Count)
        {
            currentFormAttack = FormAttacks[index];
        }
    }
}
