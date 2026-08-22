using UnityEngine;

public class EnemyStateRoll : IState
{
    private Boss boss;
    private GameObject player;
    public EnemyStateRoll(Boss boss)
    {
        this.boss = boss;
    }
    public void Enter()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            boss.ReturnFromRoll();
            return;
        }
        boss.SetRollMode(true);
        boss.Animation.PlayState(EnemyAnimationState.Roll);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.rollSound);
    }
    public void LogicUpdate()
    {
        if (player == null)
        {
            boss.ReturnFromRoll();
            return;
        }
        Vector3 target = player.transform.position;
        boss.transform.position = Vector3.MoveTowards(boss.transform.position, target, boss.RollSpeed * Time.deltaTime);
        if (Vector3.Distance(boss.transform.position, target) <= boss.RollStopDistance)
            boss.ReturnFromRoll();
    }
    public void PhysicsUpdate()
    {
    }
    public void Exit()
    {
        boss.SetRollMode(false);
    }
}