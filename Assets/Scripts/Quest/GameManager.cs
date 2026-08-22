using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject questPopup;
    [SerializeField] private GameObject dialoguePopup;
    [SerializeField] private Dialogue dialogueScript;
    [Header("Hội thoại mở đầu")]
    [SerializeField] private List<string> welcomeDialogues = new List<string>();
    [Header("Hội thoại giữa các nhiệm vụ")]
    [SerializeField] private List<string> intermissionDialogues = new List<string>();
    [Header("Hội thoại khi end game")]
    [SerializeField] private List<string> finalEndingDialogues = new List<string>();
    private int currentDialogueIndex = 0;
    private enum DialogueState { Welcome, Intermission, FinalEnding }
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
        if (welcomeDialogues.Count > 0 && dialogueScript != null)
        {
            dialogueScript.StartDialogue(welcomeDialogues[currentDialogueIndex]);
        }
    }
    public void OnClickNextDialogue()
    {
        currentDialogueIndex++;
        List<string> activeList = GetActiveDialogueList();
        if (currentDialogueIndex < activeList.Count)
        {
            dialogueScript.StartDialogue(activeList[currentDialogueIndex]);
        }
        else
        {
            HandleDialoguePhaseCompletion();
        }
    }
    private List<string> GetActiveDialogueList()
    {
        switch (currentState)
        {
            case DialogueState.Welcome: return welcomeDialogues;
            case DialogueState.Intermission: return intermissionDialogues;
            case DialogueState.FinalEnding: return finalEndingDialogues;
            default: return welcomeDialogues;
        }
    }
    private void HandleDialoguePhaseCompletion()
    {
        if (currentState == DialogueState.Welcome)
        {
            // Hết hội thoại chào mừng -> Mở quest group đầu tiên
            if (dialoguePopup != null) dialoguePopup.SetActive(false);
            if (questPopup != null) questPopup.SetActive(true);
            if (QuestManager.Instance != null) QuestManager.Instance.StartFirstQuest();
        }
        else if (currentState == DialogueState.Intermission)
        {
            // Hết hội thoại chuyển ngày/group -> Tắt dialogue, mở lại quest popup cho ngày mới
            if (dialoguePopup != null) dialoguePopup.SetActive(false);
            if (questPopup != null) questPopup.SetActive(true);
            if (TimeManager.instance != null)
            {
                TimeManager.instance.GameFill();
            }
            if (PlayerController.instance != null)
            {
                PlayerController.instance.gameObject.transform.position = Vector3.zero;
            }
            if (QuestManager.Instance != null) QuestManager.Instance.ProceedToNextGroup();
        }
        else if (currentState == DialogueState.FinalEnding)
        {
            // Hết hội thoại kết thúc game -> Tắt luôn khung thoại
            if (dialoguePopup != null) dialoguePopup.SetActive(false);
        }
    }
    public void TriggerIntermissionDialogues(bool hasMoreQuests)
    {
        if (questPopup != null) questPopup.SetActive(false);
        if (dialoguePopup != null) dialoguePopup.SetActive(true);
        currentDialogueIndex = 0;
        if (hasMoreQuests) // qua ngày
        {
            currentState = DialogueState.Intermission; 
            if (intermissionDialogues.Count > 0 && dialogueScript != null)
            {
                dialogueScript.StartDialogue(intermissionDialogues[currentDialogueIndex]);
            }
        }
        else // hết game
        {
            currentState = DialogueState.FinalEnding;
            if (finalEndingDialogues.Count > 0 && dialogueScript != null)
            {
                dialogueScript.StartDialogue(finalEndingDialogues[currentDialogueIndex]);
            }
        }
    }
}