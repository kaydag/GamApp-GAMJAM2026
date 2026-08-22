using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject questPopup;
    [SerializeField] private GameObject dialoguePopup;
    [SerializeField] private Dialogue dialogueScript;
    [Header("Hội thoại khi mới vào game")]
    [SerializeField] private List<string> welcomeDialogues = new List<string>();
    [Header("Hội thoại trước khi vào nhiệm vụ")]
    [SerializeField] private List<string> dialoguesBeforeQuest = new List<string>();
    [Header("Hội thoại sau khi hoàn thành nhiệm vụ")]
    [SerializeField] private List<string> dialoguesAfterQuest = new List<string>();
    [Header("Hội thoại khi end game")]
    [SerializeField] private List<string> finalEndingDialogues = new List<string>();
    private int currentDialogueIndex = 0;
    private enum DialogueState { Welcome, AfterQuest, BeforeQuest, FinalEnding }
    private DialogueState currentState = DialogueState.Welcome;
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        if (questPopup != null) questPopup.SetActive(false);
        if (dialoguePopup != null) dialoguePopup.SetActive(true);
        currentState = DialogueState.Welcome;
        currentDialogueIndex = 0;
        dialogueScript.StartDialogue(welcomeDialogues[currentDialogueIndex]);
    }

    public void OnClickNextDialogue()
    {
        // Riêng Welcome và FinalEnding là list nhiều câu
        if (currentState == DialogueState.Welcome || currentState == DialogueState.FinalEnding)
        {
            currentDialogueIndex++;
            List<string> activeList = (currentState == DialogueState.Welcome) ? welcomeDialogues : finalEndingDialogues;
            if (currentDialogueIndex < activeList.Count)
            {
                dialogueScript.StartDialogue(activeList[currentDialogueIndex]);
            }
            else
            {
                HandleDialoguePhaseCompletion();
            }
        }
        else
        {
            // Các trạng thái khác bấm Next là kết thúc phase hội thoại hiện tại
            HandleDialoguePhaseCompletion();
        }
    }

    private void HandleDialoguePhaseCompletion()
    {
        if (currentState == DialogueState.Welcome)
        {
            // Hết Welcome -> Bật hội thoại BeforeQuest đầu tiên
            currentState = DialogueState.BeforeQuest;
            if (dialoguePopup != null) dialoguePopup.SetActive(true);
            dialogueScript.StartDialogue(dialoguesBeforeQuest[QuestManager.Instance.currentQuestIndex]);
        }
        else if (currentState == DialogueState.AfterQuest)
        {
            // Hết hội thoại After -> Gọi TimeManager chuyển ngày/đêm, sau đó mở tiếp BeforeQuest của ngày mới
            if (TimeManager.instance != null)
            {
                TimeManager.instance.GameFill();
            }
            if (PlayerController.instance != null)
            {
                PlayerController.instance.gameObject.transform.position = Vector3.zero;
            }

            // Giữ dialoguePopup bật để chạy tiếp BeforeQuest
            currentState = DialogueState.BeforeQuest;
            if (dialoguePopup != null) dialoguePopup.SetActive(true);
            dialogueScript.StartDialogue(dialoguesBeforeQuest[QuestManager.Instance.currentQuestIndex]);
        }
        else if (currentState == DialogueState.BeforeQuest)
        {
            // ĐÃ ĐỌC XONG HỘI THOẠI BEFORE -> LÚC NÀY MỚI CHÍNH THỨC TẮT DIALOGUE VÀ GỌI QUESTMANAGER LOAD QUEST
            if (dialoguePopup != null) dialoguePopup.SetActive(false);
            if (questPopup != null) questPopup.SetActive(true);

            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.ProceedToNextGroup();
            }
        }
        else if (currentState == DialogueState.FinalEnding)
        {
            if (dialoguePopup != null) dialoguePopup.SetActive(false);
        }
    }

    // Được gọi từ QuestManager khi hoàn thành một nhóm nhiệm vụ
    public void TriggerIntermissionDialogues(bool hasMoreQuests, int finishedGroupIndex)
    {
        if (questPopup != null) questPopup.SetActive(false);
        if (dialoguePopup != null) dialoguePopup.SetActive(true);

        if (hasMoreQuests)
        {
            // Chạy đúng 1 câu AfterQuest tương ứng với nhóm vừa hoàn thành
            currentState = DialogueState.AfterQuest;
            dialogueScript.StartDialogue(dialoguesAfterQuest[finishedGroupIndex]);
        }
        else
        {
            // Hoàn thành nhiệm vụ cuối cùng -> Chạy Final Ending
            currentState = DialogueState.FinalEnding;
            currentDialogueIndex = 0;
            if (finalEndingDialogues.Count > 0 && dialogueScript != null)
            {
                dialogueScript.StartDialogue(finalEndingDialogues[currentDialogueIndex]);
            }
            else
            {
                if (dialoguePopup != null) dialoguePopup.SetActive(false);
            }
        }
    }
}