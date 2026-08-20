using System.Collections;
using UnityEngine;
public class StatusEffect : MonoBehaviour
{
    private BaseEnemy enemy;
    private Coroutine burnCoroutine;
    private Coroutine stunCoroutine;
    private void Awake()
    {
        enemy = GetComponent<BaseEnemy>();
    }
    public void ApplyBurn(float duration, float damagePerSecond)
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        burnCoroutine = StartCoroutine(BurnCoroutine(duration, damagePerSecond));
    }

    private IEnumerator BurnCoroutine(float duration,float damagePerSecond){
        float timer = 0f;
        while (timer < duration)
        {
            enemy.TakeDamage(damagePerSecond, false);
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }
        burnCoroutine = null;
    }

    public void ApplyStun(float duration)
    {
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        stunCoroutine = StartCoroutine(StunCoroutine(duration));
    }

    private IEnumerator StunCoroutine(float duration)
    {
        enemy.SetStunned(true);
        yield return new WaitForSeconds(duration);
        enemy.SetStunned(false);
        stunCoroutine = null;
    }
}