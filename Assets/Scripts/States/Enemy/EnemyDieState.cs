using UnityEngine;

public class EnemyDieState : IState
{
    private BaseEnemy enemy;
    private float dieDuration = 1f;
    private float timer;
    private bool destroyed;
    public EnemyDieState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        timer = 0f;
        destroyed = false;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDieSound);
        enemy.Animation.PlayState(EnemyAnimationState.Die);
    }
    public void LogicUpdate()
    {
        if (destroyed)
            return;
        timer += Time.deltaTime;

        if (timer >= dieDuration)
        {
            destroyed = true;
            Object.Destroy(enemy.gameObject);
        }
    }
    public void PhysicsUpdate()
    {
        //update sau
    }
    public void Exit()
    {
        //update sau
    }
}