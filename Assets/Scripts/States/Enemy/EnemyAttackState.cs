using UnityEngine;

public class EnemyAttackState : IState
{
    private BaseEnemy enemy;
    private GameObject target;
    public EnemyAttackState(BaseEnemy enemy, GameObject target)
    {
        this.enemy = enemy;
        this.target = target;
    }
    public void Enter()
    {
        Debug.Log("Enemy Enter Attack State");
        enemy.Attack(target);
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
        Debug.Log("Enemy Exit Attack State");
    }
}