using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class IntroManager : MonoBehaviour
{
    [Header("Intro Data")]
    [SerializeField] private List<IntroData> introDatas;
    [Header("Background")]
    [SerializeField] private Image bg1;
    [SerializeField] private Image bg2;
    [Header("Character")]
    [SerializeField] private Image luna;
    [SerializeField] private Image sol;
    [SerializeField] private Image Name;
    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private TMP_Text nameText;
    [Header("UI")]
    [SerializeField] private GameObject introPanel;
    private int currentIndex = 0;
    [SerializeField] bool isIntro= false;
    [Header("Ending")]
    [SerializeField] private Image black;
    [SerializeField] private TMP_Text toBeContinued;
    [SerializeField] private float transitionDuration = 1.5f;

    private void Start()
    {
        dialogue.onDialogueComplete.RemoveAllListeners();
        dialogue.onDialogueComplete.AddListener(NextIntro);
        currentIndex = 0;
        if (toBeContinued != null)
        {
            toBeContinued.gameObject.SetActive(false);
        }
        if (black != null)
        {
            black.gameObject.SetActive(false);
        }
        ShowIntro(currentIndex);
    }

    private void ShowIntro(int index)
    {
        if (index >= introDatas.Count)
        {
            FinishIntro();
            return;
        }
        IntroData data = introDatas[index];
        ApplyIntroData(data);
    }
    private void ApplyIntroData(IntroData data)
    {
        bg1.gameObject.SetActive(data.background == BackgroundType.Normal);
        bg2.gameObject.SetActive(data.background == BackgroundType.Destroyed);
        luna.gameObject.SetActive(data.character == CharacterType.Luna);
        sol.gameObject.SetActive(data.character == CharacterType.Sol);
        if (data.character == CharacterType.None)
        {
            Name.gameObject.SetActive(false);
        }
        else
        {
            Name.gameObject.SetActive(true);
            nameText.text = data.characterName.ToString();
        }
        dialogue.StartDialogue(data.dialogue);
    }
    public void NextIntro()
    {
        currentIndex++;
        if (currentIndex >= introDatas.Count)
        {
            FinishIntro();
            return;
        }
        ShowIntro(currentIndex);
    }
    private void FinishIntro()
    {
        if (introPanel != null)
        {
            introPanel.SetActive(false);
        }
        if (!isIntro)
        {
            StartCoroutine(FinishEnding());
            return;
        }
        PlayerPrefs.SetInt("HasPlayedBefore", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
    private IEnumerator FinishEnding()
    {
        isIntro = false;

        if (black != null)
        {
            black.gameObject.SetActive(true);
            RectTransform rt = black.rectTransform;
            rt.anchoredPosition =new Vector2(-Screen.width,0f);
            yield return rt.DOAnchorPosX(0f, transitionDuration).SetEase(Ease.OutQuad).WaitForCompletion();
        }
        yield return new WaitForSeconds(0.5f);
        if (toBeContinued != null)
        {
            toBeContinued.gameObject.SetActive(true);
            Color textColor = toBeContinued.color;
            textColor.a = 0f;
            toBeContinued.color = textColor;
            yield return toBeContinued.DOFade(1f, 0.5f).WaitForCompletion();
        }
    }
}