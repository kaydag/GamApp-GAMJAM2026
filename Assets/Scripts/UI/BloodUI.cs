using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BloodUI : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bloodBar;
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
    void UpdateBloodUI(GameObject target, float currentHealth, float maxHealth)
    {
        if (target != transform.root.gameObject) return;
        float percent = currentHealth / maxHealth;
        bloodBar.transform.localScale = new Vector3(percent,1f,1f);
    }
}
