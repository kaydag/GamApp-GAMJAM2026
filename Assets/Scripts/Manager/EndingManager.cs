using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class GameEndingManager : MonoBehaviour
{
    public static GameEndingManager Instance;

    [SerializeField] private Image black;
    [SerializeField] private float transitionDuration = 0.8f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartEnding()
    {
        StartCoroutine(TransitionToEnding());
    }

    private IEnumerator TransitionToEnding()
    {
        RectTransform rt = black.rectTransform;

        black.gameObject.SetActive(true);
        rt.anchoredPosition = new Vector2(-Screen.width, 0f);
        yield return rt
            .DOAnchorPosX(
                0f,
                transitionDuration
            )
            .SetEase(Ease.Linear)
            .WaitForCompletion();
        SceneManager.LoadScene("EndScene");
    }
}