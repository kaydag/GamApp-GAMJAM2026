using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    [Header("List Quests")]
    [SerializeField] private List<Quest> questList = new List<Quest>();
    private void Awake()
    {
        // Tạo Singleton đơn giản để dễ gọi từ các script khác
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Hàm kiểm tra trạng thái tất cả các quest
    public void CheckAllQuests()
    {
        foreach (var quest in questList)
        {
            // Nếu có ít nhất 1 quest chưa hoàn thành thì dừng lại
            if (!quest.IsCompleted)
            {
                return;
            }
        }

        // Nếu chạy hết vòng lặp mà không return nghĩa là tất cả đã hoàn thành!
        OnAllQuestsCompleted();
    }
    private void OnAllQuestsCompleted()
    {
        Debug.Log("Tất cả nhiệm vụ đã hoàn thành! Tiến hành phần thưởng hoặc mở khóa màn chơi tiếp theo.");
        // Viết logic tiếp theo ở đây (ví dụ: hiện popup chiến thắng, cộng quà, chuyển scene...)
    }
}