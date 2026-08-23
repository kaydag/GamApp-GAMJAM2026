using UnityEngine;
using UnityEngine.UI;
using TMPro; // Sử dụng nếu dùng TextMeshPro cho chữ

public class Quest : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image finishedBox;
    [SerializeField] private Sprite finishedIcon;
    [SerializeField] private Sprite UnFinishedIcon;
    [SerializeField] private TMP_Text contentText;
    [Header("Quest Settings")]
    public int targetAmount = 4;
    public int LocationIndex = 0;
    public string targetName = "BaseEnemy";
    public string displayTargetName = "base enemies";
    private int currentAmount = 0;
    private bool isCompleted = false;
    public bool IsCompleted => isCompleted;
    public void SetQuest(QuestData data)
    {
        targetAmount = data.targetAmount;
        LocationIndex = data.LocationIndex;
        targetName = data.targetName;
        displayTargetName = data.displayTargetName;
        UpdateQuestUI();
    }
    private void Start()
    {
        UpdateQuestUI();
    }
    private void OnEnable()
    {
        GameEvent.EnemyDie += HandleEnemyDied;
    }

    private void OnDisable()
    {
        GameEvent.EnemyDie -= HandleEnemyDied;
    }
    public void HandleEnemyDied(BaseEnemy enemy)
    {
        Debug.Log(enemy.name);
        if (isCompleted) return;
        if (enemy.name.Contains(targetName))
        {
            currentAmount++;
            if (currentAmount >= targetAmount)
            {
                currentAmount = targetAmount;
                CompleteQuest();
            }
            UpdateQuestUI();
        }
    }
    private void CompleteQuest()
    {
        isCompleted = true;
        QuestManager.Instance.CheckAllQuests();
    }
    private void UpdateQuestUI()
    {
        contentText.text = $"Defeat {targetAmount} {displayTargetName}\n({currentAmount}/{targetAmount})";
        if (finishedIcon != null)
        {
            if (IsCompleted)
            {
                finishedBox.sprite = finishedIcon;
                if (PlayerDirection.instance != null)
                {
                    PlayerDirection.instance.HideDirection();
                }
            }
            else
            {
                finishedBox.sprite = UnFinishedIcon;
                if (PlayerDirection.instance != null)
                {
                    PlayerDirection.instance.ShowDirection(LocationIndex);
                }
            }
        }
    }
}