using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string introSceneName = "IntroScene";
    [SerializeField] private string gameSceneName = "GameScene";
    private void Start()
    {
        
    }

    public void OnClickPlay()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.click);
        bool hasPlayedBefore = PlayerPrefs.GetInt("HasPlayedBefore", 0) == 1;
        string nextScene;
        if (hasPlayedBefore) nextScene = gameSceneName;
        else nextScene = introSceneName;
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
    }
}