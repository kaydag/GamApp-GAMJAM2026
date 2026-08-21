using UnityEngine;

public class EnemyMoveState : IState
{
    private BaseEnemy enemy;
    public EnemyMoveState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.Animation.PlayState(EnemyAnimationState.Move);
    }
    public void LogicUpdate()
    {
        // update sau
    }
    public void PhysicsUpdate()
    {
        enemy.Move();
    }
    public void Exit()
    {

    }
}