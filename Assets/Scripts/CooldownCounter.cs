using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownCounter : MonoBehaviour
{
    [Header("References UI")]
    [SerializeField] private Button skillButton;
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI cooldownText;
    private float currentCooldown = 0f;
    private float totalCooldownTime = 5f;
    private bool isCooldown = false;
    private void Start()
    {
        if (skillButton == null) skillButton = GetComponent<Button>();
        if (cooldownImage != null) cooldownImage.fillAmount = 0;
        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            if (cooldownImage != null)
            {
                float maxTime = totalCooldownTime > 0 ? totalCooldownTime : 5f;
                cooldownImage.fillAmount = Mathf.Clamp01(currentCooldown / maxTime);
            }
            if (cooldownText != null)
            {
                if (!cooldownText.gameObject.activeSelf)
                    cooldownText.gameObject.SetActive(true);

                cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
            }
            if (currentCooldown <= 0f)
            {
                CooldownEnd();
            }
        }
    }
    public void SyncCooldown(float remainingTime, float totalCooldown)
    {
        totalCooldownTime = totalCooldown > 0 ? totalCooldown : 5f;

        if (remainingTime > 0f)
        {
            isCooldown = true;
            currentCooldown = remainingTime;

            if (skillButton != null) skillButton.interactable = false;
            if (cooldownText != null)
            {
                cooldownText.gameObject.SetActive(true);
                cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
            }
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = currentCooldown / totalCooldownTime;
            }
        }
        else
        {
            CooldownEnd();
        }
    }
    public void TriggerCooldown(float duration)
    {
        SyncCooldown(duration, duration);
    }
    private void CooldownEnd()
    {
        isCooldown = false;
        currentCooldown = 0f;

        if (skillButton != null) skillButton.interactable = true;
        if (cooldownImage != null) cooldownImage.fillAmount = 0;
        if (cooldownText != null) cooldownText.gameObject.SetActive(false);
    }
}