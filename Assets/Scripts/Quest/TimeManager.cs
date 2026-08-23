using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps; // Thêm thư viện Tilemaps để đổi màu bản đồ
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private Image BlackScreen;
    [SerializeField] private TextMeshProUGUI TimeText;
    public bool IsNight { get; private set; } = false;
    public static TimeManager instance;
    private int currentCycleCount = 1; // Mặc định là Day 1

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (BlackScreen != null)
        {
            Color c = BlackScreen.color;
            c.a = 0;
            BlackScreen.color = c;
            BlackScreen.gameObject.SetActive(false);
        }
        if (TimeText != null)
        {
            TimeText.gameObject.SetActive(false);
        }
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayDayBGM();
        }
    }

    public void GameFill(bool doEnd)
    {
        bool targetNightState = !IsNight;
        IsNight = targetNightState;
        if (BlackScreen == null || TimeText == null)
        {
            ExecuteStateChange();
            return;
        }
        BlackScreen.gameObject.SetActive(true);
        TimeText.gameObject.SetActive(true);
        Color screenColor = BlackScreen.color;
        screenColor.a = 1f;
        BlackScreen.color = screenColor;
        // Cập nhật nội dung chữ
        string phaseName = IsNight ? "NIGHT" : "DAY";
        TimeText.text = $"{phaseName} {currentCycleCount}";
        Color textColor = TimeText.color;
        textColor.a = 0;
        TimeText.color = textColor;
        RectTransform rt = BlackScreen.rectTransform;
        rt.anchoredPosition = new Vector2(-Screen.width, 0);
        Sequence seq = DOTween.Sequence();
        seq.Append(rt.DOAnchorPosX(0, 0.6f).SetEase(Ease.OutQuad));
        seq.AppendCallback(() =>
        {
            if (doEnd)
            {
                SceneManager.LoadScene("EndgameScene");
                return;
            }
            ExecuteStateChange();
        });
        seq.Append(TimeText.DOFade(1f, 0.3f));
        seq.AppendInterval(1f);
        seq.Append(TimeText.DOFade(0f, 0.3f));
        seq.Append(rt.DOAnchorPosX(Screen.width, 0.6f).SetEase(Ease.InQuad));
        seq.OnComplete(() =>
        {
            BlackScreen.gameObject.SetActive(false);
            TimeText.gameObject.SetActive(false);
            if (IsNight)
            {
                currentCycleCount++;
            }
        });
    }
    private void ExecuteStateChange()
    {
        if (!IsNight)
        {
            SummonDayTime();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayDayBGM();
            }
        }
        else
        {
            SummonNightTime();
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayNightBGM();
            }
        }
    }
    public void SummonNightTime()
    {
        Color nightColor = new Color32(110, 101, 168, 255);
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();
        foreach (var sr in allSprites)
        {
            sr.DOColor(nightColor, 0.5f);
        }
        Tilemap[] allTilemaps = FindObjectsOfType<Tilemap>();
        foreach (var tm in allTilemaps)
        {
            DOTween.To(() => tm.color, x => tm.color = x, nightColor, 0.5f);
        }
    }
    public void ChangeColorByTime(Transform parent)
    {
        Color correctColor = new Color32(110, 101, 168, 255);
        if (!IsNight)
        {
            correctColor = new Color32(255, 255, 255, 255);
        }
        SpriteRenderer[] renderers = parent.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = correctColor;
        }
    }
    public void SummonDayTime()
    {
        Color dayColor = new Color32(255, 255, 255, 255);
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();
        foreach (var sr in allSprites)
        {
            sr.DOColor(dayColor, 0.5f);
        }
        Tilemap[] allTilemaps = FindObjectsOfType<Tilemap>();
        foreach (var tm in allTilemaps)
        {
            DOTween.To(() => tm.color, x => tm.color = x, dayColor, 0.5f);
        }
    }
}