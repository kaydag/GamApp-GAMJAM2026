using UnityEngine;

public class EnemyStateThrow : IState
{
    private Boss boss;
    private float timer;
    private int currentRound;
    public EnemyStateThrow(Boss boss)
    {
        this.boss = boss;
    }
    public void Enter()
    {
        timer = 0f;
        currentRound = 0;
        boss.Animation.PlayState(EnemyAnimationState.Throw);
        ThrowRocks();
    }
    public void LogicUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= boss.ThrowInterval)
        {
            timer = 0f;
            currentRound++;
            if (currentRound < boss.ThrowRounds) ThrowRocks();
            else boss.StartRoll();
        }
    }

    private void ThrowRocks()
    {
        const int rockCount = 8;
        float angleStep = 360f / rockCount;
        for (int i = 0; i < rockCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Object.Instantiate(boss.RockPrefab, boss.transform.position, rotation);
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.rockThrowSound);
    }
    public void PhysicsUpdate()
    {
    }
    public void Exit()
    {
    }
}