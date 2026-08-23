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
    private void Start()
    {
        dialogue.onDialogueComplete.RemoveAllListeners();
        dialogue.onDialogueComplete.AddListener(NextIntro);
        currentIndex = 0;
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
        PlayerPrefs.SetInt("HasPlayedBefore", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene("GameScene");
    }
}