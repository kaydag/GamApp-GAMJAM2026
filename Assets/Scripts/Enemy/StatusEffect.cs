using System.Collections;
using UnityEngine;
public class StatusEffect : MonoBehaviour
{
    private BaseEnemy enemy;
    private Coroutine burnCoroutine;
    private Coroutine stunCoroutine;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem burnEffect;
    [SerializeField] private ParticleSystem stunEffect;
    private void Awake()
    {
        enemy = GetComponent<BaseEnemy>();
        if (burnEffect != null)
            burnEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (stunEffect != null)
            stunEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    public void ApplyBurn(float duration, float damagePerSecond)
    {
        if (burnCoroutine != null)
        {
            StopCoroutine(burnCoroutine);
        }
        PlayBurnEffect();
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
        StopBurnEffect();
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
        PlayStunEffect();
        yield return new WaitForSeconds(duration);
        enemy.SetStunned(false);
        StopStunEffect();
        stunCoroutine = null;
    }
    private void PlayBurnEffect()
    {
        if (burnEffect != null)
        {
            burnEffect.Play();
        }
    }

    private void StopBurnEffect()
    {
        if (burnEffect != null)
        {
            burnEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void PlayStunEffect()
    {
        if (stunEffect != null)
        {
            stunEffect.Play();
        }
    }

    private void StopStunEffect()
    {
        if (stunEffect != null)
        {
            stunEffect.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}