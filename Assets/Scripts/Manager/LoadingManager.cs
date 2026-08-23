using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("Start Screen")]
    [SerializeField] private GameObject playButton;

    [Header("Scenes")]
    [SerializeField] private string introSceneName = "IntroScene";
    [SerializeField] private string gameSceneName = "GameScene";
    private void Start()
    {
        playButton.SetActive(true);
    }

    public void OnClickPlay()
    {
        Debug.Log("Play button clicked");
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        bool hasPlayedBefore = PlayerPrefs.GetInt("HasPlayedBefore", 0) == 1;
        string nextScene;
        if (hasPlayedBefore) nextScene = gameSceneName;
        else nextScene = introSceneName;
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
    }
}