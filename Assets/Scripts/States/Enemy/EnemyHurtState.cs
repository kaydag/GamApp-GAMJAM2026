using UnityEngine;

public class EnemyHurtState : IState
{
    private BaseEnemy enemy;
    private float hurtDuration;
    private float timer;
    public EnemyHurtState(BaseEnemy enemy, float hurtDuration = 0.2f)
    {
        this.enemy = enemy;
        this.hurtDuration = hurtDuration;
    }
    public void Enter()
    {
        timer = hurtDuration;
        // Play Hurt Animation sau này
    }
    public void LogicUpdate()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemy.ReturnFromHurt();
        }
    }
    public void PhysicsUpdate()
    {
    }
    public void Exit()
    {
    }
}