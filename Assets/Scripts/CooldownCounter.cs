using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Dùng TextMeshPro (nếu xài Text thường thì đổi sang UnityEngine.UI.Text)

public class CooldownCounter : MonoBehaviour
{
    [Header("Cấu hình Cooldown")]
    [SerializeField] private float cooldownTime = 5f; // Swap truyền thời gian hồi chiêu
    [Header("References UI")]
    [SerializeField] private Button skillButton;          // Kéo cái Button vào đây
    [SerializeField] private Image cooldownImage;       // Kéo Image làm hiệu ứng tối/fill (đặt Type là Filled)
    [SerializeField] private TextMeshProUGUI cooldownText; // (Tùy chọn) Text hiển thị số giây
    private float currentCooldown = 0f;
    private bool isCooldown = false;
    private void Start()
    {
        // Nếu không kéo thủ công, tự động lấy Button nằm cùng GameObject
        if (skillButton == null)
            skillButton = GetComponent<Button>();
        // Lắng nghe sự kiện click nút
        if (skillButton != null)
        {
            skillButton.onClick.AddListener(OnSkillButtonClicked);
        }
        // Khởi tạo ban đầu
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;
        if (cooldownText != null)
            cooldownText.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (isCooldown)
        {
            currentCooldown -= Time.deltaTime;
            // Cập nhật hiệu ứng UI vòng tròn/tối dần (Fill Amount chạy từ 1 về 0)
            if (cooldownImage != null)
            {
                cooldownImage.fillAmount = currentCooldown / cooldownTime;
            }
            // Cập nhật Text số giây đếm ngược (làm tròn lên cho đẹp)
            if (cooldownText != null)
            {
                cooldownText.text = Mathf.Ceil(currentCooldown).ToString();
            }
            if (currentCooldown <= 0f)
            {
                CooldownEnd();
            }
        }
    }

    private void OnSkillButtonClicked()
    {
        if (isCooldown) return;
        StartCooldown();
    }
    public void StartCooldown()
    {
        isCooldown = true;
        currentCooldown = cooldownTime;
        if (skillButton != null) skillButton.interactable = false;
        if (cooldownText != null) cooldownText.gameObject.SetActive(true);
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
