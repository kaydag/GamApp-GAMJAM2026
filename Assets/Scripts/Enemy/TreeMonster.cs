using UnityEngine;

public class TreeMonster : BaseEnemy
{
    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private float cooldown = 10f;

    private float timer;

    private TreeMonsterAttackState treeAttackState;

    protected override void Awake()
    {
        base.Awake();

        timer = cooldown;

        treeAttackState = new TreeMonsterAttackState(this);
    }

    protected override void Update()
    {
        CheckDie();

        if (isDead)
        {
            stateMachine.LogicUpdate();
            return;
        }

        // Đếm cooldown spawn
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            stateMachine.ChangeState(treeAttackState);
        }

        // FSM xử lý state hiện tại
        stateMachine.LogicUpdate();
    }

    public void SpawnWood()
    {
        if (woodPrefab == null || spawnPoint == null) return;
        AudioManager.Instance.PlaySFX(AudioManager.Instance.woodThrowSound);
        Instantiate(woodPrefab, spawnPoint.position, Quaternion.Euler(0f, 0f, -45f));
        timer = 0f;
    }

    public void PlayAttackAnimation()
    {
        Animation.PlayState(EnemyAnimationState.Attack);
    }

    public void ReturnFromAttack()
    {
        if (isDead) return;
        stateMachine.ChangeState(idleState);
    }
}