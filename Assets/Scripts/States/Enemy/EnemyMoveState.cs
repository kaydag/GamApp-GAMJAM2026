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
        Debug.Log("Enemy Enter Move State");
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
        Debug.Log("Enemy Exit Move State");
    }
}