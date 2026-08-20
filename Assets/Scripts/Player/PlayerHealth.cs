using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void OnEnable()
    {
        GameEvent.Attack += OnAttack;
        GameEvent.PlayerHealthChanged += Heal;
    }
    private void OnDisable()
    {
        GameEvent.Attack -= OnAttack;
        GameEvent.PlayerHealthChanged -= Heal;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Current Health: {currentHealth}");
    }

    private void OnAttack(GameObject attacker, GameObject target, float damage)
    {
        if (target != gameObject)
            return;

        TakeDamage(damage);
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Debug.Log("Player Died");
        Destroy(gameObject);
        //quay về điểm checkpoint
    }

    void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
}
