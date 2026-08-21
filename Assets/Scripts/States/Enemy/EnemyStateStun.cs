using UnityEngine;

public class EnemyStunState : IState
{
    private BaseEnemy enemy;
    public EnemyStunState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.PlayStunAnimation();
    }
    public void LogicUpdate()
    {
        // Trong thời gian stun không xử lý 
    }

    public void PhysicsUpdate()
    {
        // Không di chuyển
    }
    public void Exit()
    {
        enemy.PlayMoveAnimation();
    }
}