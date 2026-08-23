using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
public class Dialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float textSpeed = 0.05f;
    private string currentFullText = "";
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public UnityEvent onDialogueComplete;
    public void StartDialogue(string textToPrint)
    {
        if (string.IsNullOrEmpty(textToPrint)) return;
        currentFullText = textToPrint;
        // Nếu đang chạy dở đoạn trước thì dừng lại
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine());
    }
    // Coroutine chạy từng chữ cái
    private IEnumerator TypeLine()
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char c in currentFullText.ToCharArray())
        {
            dialogueText.text += c;
            // Đợi một khoảng thời gian ngắn rồi hiện chữ tiếp theo
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }
    // Nếu người chơi bấm chuột/phím để muốn hiện toàn bộ chữ luôn
    public void SkipOrComplete()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentFullText; // Hiện toàn bộ ngay lập tức
            isTyping = false;
        }
        else
        {
            onDialogueComplete?.Invoke();
        }
    }
}
