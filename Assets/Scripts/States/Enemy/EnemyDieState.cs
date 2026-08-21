using UnityEngine;

public class EnemyDieState : IState
{
    private BaseEnemy enemy;
    public EnemyDieState(BaseEnemy enemy)
    {
        this.enemy = enemy;
    }
    public void Enter()
    {
        enemy.Die();
        enemy.Animation.PlayState(EnemyAnimationState.Die);
    }
    public void LogicUpdate()
    {
        //update sau
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