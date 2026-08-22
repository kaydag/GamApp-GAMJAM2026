using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestGroupData
{
    public List<QuestData> questDatas = new List<QuestData>();
    public GameObject questLoadout;
}

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("UI Prefab & Container")]
    [SerializeField] private GameObject questPrefab;
    [SerializeField] private Transform questParent;
    [Header("Quest Groups Configuration")]
    [SerializeField] private List<QuestGroupData> questGroups = new List<QuestGroupData>();
    private int currentQuestIndex = 0;
    // Lưu danh sách các quest đang hiện trên UI của nhóm hiện tại
    private List<Quest> spawnedQuests = new List<Quest>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartFirstQuest()
    {
        SpawnActiveQuestGroup();
    }
    private void SpawnActiveQuestGroup()
    {
        // Xóa các quest cũ nếu có
        foreach (var q in spawnedQuests)
        {
            if (q != null) Destroy(q.gameObject);
        }
        spawnedQuests.Clear();
        if (currentQuestIndex >= questGroups.Count) return;
        var currentGroup = questGroups[currentQuestIndex];
        foreach (var data in currentGroup.questDatas)
        {
            if (questPrefab != null && questParent != null)
            {
                GameObject questObj = Instantiate(questPrefab, questParent);
                Quest questScript = questObj.GetComponent<Quest>();
                if (questScript != null)
                {
                    questScript.SetQuest(data);
                    spawnedQuests.Add(questScript);
                    if (currentGroup.questLoadout != null)
                    {
                        Instantiate(currentGroup.questLoadout, 
                            PlayerDirection.instance.GetEnemyLocation(data.LocationIndex).transform.position, 
                            Quaternion.identity);
                    }
                }
            }
        }
    }
    public void CheckAllQuests()
    {
        if (spawnedQuests.Count == 0) return;
        foreach (var quest in spawnedQuests)
        {
            if (quest == null || !quest.IsCompleted) return;
        }
        OnCurrentGroupCompleted();
    }
    private void OnCurrentGroupCompleted()
    {
        currentQuestIndex++;
        if (GameManager.instance != null)
        {
            GameManager.instance.TriggerIntermissionDialogues(currentQuestIndex < questGroups.Count);
        }
    }
    public void ProceedToNextGroup()
    {
        if (currentQuestIndex < questGroups.Count)
        {
            SpawnActiveQuestGroup();
        }
    }
}