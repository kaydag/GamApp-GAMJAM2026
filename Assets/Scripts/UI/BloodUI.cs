using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class BloodUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bloodBar;
    [SerializeField] private Image bloodBarImage;
    [SerializeField] private GameObject target;
    private void OnEnable()
    {
        GameEvent.HealthChanged += UpdateBloodUI;
    }
    void OnDisable()
    {
        GameEvent.HealthChanged -= UpdateBloodUI;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void UpdateBloodUI(GameObject targetObject, float currentHealth, float maxHealth)
    {
        if (targetObject != target) return;
        float percent = (currentHealth > 0 ? currentHealth : 0) / maxHealth;
        if (bloodBarImage != null)
        {
            bloodBarImage.fillAmount = percent;
            return;
        }
        else if (bloodBar != null)
        {
            bloodBar.transform.localScale = new Vector3(percent, 1f, 1f);
        }
    }
}
