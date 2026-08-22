using UnityEngine;

public class TreeMonsterAttackState : IState
{
    private TreeMonster enemy;

    private float timer;
    private bool hasSpawned;

    private const float attackDuration = 1f;
    private const float spawnTime = 0.4f;

    public TreeMonsterAttackState(TreeMonster enemy)
    {
        this.enemy = enemy;
    }

    public void Enter()
    {
        timer = 0f;
        hasSpawned = false;

        enemy.PlayAttackAnimation();
    }

    public void LogicUpdate()
    {
        timer += Time.deltaTime;

        // Đúng 0.4 giây thì spawn Wood
        if (!hasSpawned && timer >= spawnTime)
        {
            hasSpawned = true;
            enemy.SpawnWood();
        }

        // Attack animation kết thúc sau 1 giây
        if (timer >= attackDuration)
        {
            enemy.ReturnFromAttack();
        }
    }

    public void PhysicsUpdate()
    {
    }

    public void Exit()
    {
    }
}