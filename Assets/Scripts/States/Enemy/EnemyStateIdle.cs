using UnityEngine;

public class EnemyIdleState : IState
{
    private BaseEnemy enemy;
    private float idleDuration;
    private float timer;
    public EnemyIdleState(BaseEnemy enemy, float idleDuration = 0.5f)
    {
        this.enemy = enemy;
        this.idleDuration = idleDuration;
    }
    public void Enter()
    {
        timer = idleDuration;
        enemy.Animation.PlayState(EnemyAnimationState.Idle);
    }

    public void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemy.ReturnFromIdle();
        }
    }
    public void PhysicsUpdate()
    {
        // Idle không di chuyển
    }

    public void Exit()
    {

    }
}