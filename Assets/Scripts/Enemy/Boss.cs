using UnityEngine;

public class Boss : BaseEnemy
{
    [Header("Throw Skill")]
    [SerializeField] private GameObject rockPrefab;

    // Khoảng thời gian giữa 2 đợt ném
    [SerializeField] private float throwInterval = 0.8f;

    // Khoảng thời gian giữa các lần dùng skill
    [SerializeField] private float skillCooldown = 10f;

    // Có 2 đợt, mỗi đợt 8 hướng
    [SerializeField] private int throwRounds = 2;

    [Header("Roll")]
    [SerializeField] private float rollSpeed = 8f;
    [SerializeField] private float rollStopDistance = 0.5f;
    [SerializeField] private float rollDamage = 20f;

    [Header("Colliders")]
    [SerializeField] private Collider2D normalCollider;
    [SerializeField] private Collider2D rollCollider;

    private EnemyStateThrow throwState;
    private EnemyStateRoll rollState;

    private float skillTimer;

    public GameObject RockPrefab => rockPrefab;
    public float ThrowInterval => throwInterval;
    public int ThrowRounds => throwRounds;

    public float RollSpeed => rollSpeed;
    public float RollStopDistance => rollStopDistance;
    public float RollDamage => rollDamage;

    protected override void Awake()
    {
        base.Awake();

        throwState = new EnemyStateThrow(this);
        rollState = new EnemyStateRoll(this);

        skillTimer = 0f;
    }

    protected override void Update()
    {
        base.Update();
        if (stateMachine.CurrentState == throwState || stateMachine.CurrentState == rollState) return;
        skillTimer += Time.deltaTime;
        if (skillTimer >= skillCooldown) StartThrowSkill();
    }

    public void StartThrowSkill()
    {
        skillTimer = 0f;
        stateMachine.ChangeState(throwState);
    }
    public void StartRoll()
    {
        stateMachine.ChangeState(rollState);
    }

    public void ReturnFromRoll()
    {
        stateMachine.ChangeState(idleState);
    }

    public void DealRollDamage(GameObject player)
    {
        GameEvent.Attack?.Invoke(gameObject, player, rollDamage);
    }
    public void SetRollMode(bool value)
    {
        if (normalCollider != null) normalCollider.enabled = !value;
        if (rollCollider != null) rollCollider.enabled = value;
    }
}