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
        enemy.Attack(target);
        enemy.Animation.PlayState(EnemyAnimationState.Attack);
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

    }
}