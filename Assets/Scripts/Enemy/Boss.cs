using UnityEngine;
public class Boss : BaseEnemy
{
    [SerializeField] private GameObject rockPrefab;

    [Header("Skill")]
    [SerializeField] private float cooldownSkill = 10f;
    [SerializeField] private float skillDuration = 4f;
    [SerializeField] private float rockCooldown = 0.5f;
    [SerializeField] private int rockCount = 8;

    private float cooldownTimer;
    private float skillTimer;
    private float rockTimer;

    private bool isUsingSkill = false;

    [Header("Roll")]
    [SerializeField] private float rollSpeed = 8f;
    [SerializeField] private float rollStopDistance = 0.5f;

    private bool isRolling = false;
    private Vector3 rollTarget;

    protected override void Start()
    {
        base.Start();

        cooldownTimer = 0f;
        skillTimer = 0f;
        rockTimer = 0f;
    }

    protected override void Update()
    {
        if (isUsingSkill)
        {
            skillTimer += Time.deltaTime;
            rockTimer += Time.deltaTime;
            // Bắn đá
            if (rockTimer >= rockCooldown)
            {
                SpawnRocks();
                rockTimer = 0f;
            }
            // HẾT SKILL
            if (skillTimer >= skillDuration)
            {
                EndSkill();
            }
            return;
        }
        if (isRolling)
        {
            RollToPlayer();
            return;
        }
        base.Update();
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= cooldownSkill)
        {
            StartSkill();
        }
    }

    protected override void FixedUpdate()
    {
        if (!isUsingSkill)
        {
            base.FixedUpdate();
        }
    }
    private void StartSkill()
    {
        isUsingSkill = true;

        skillTimer = 0f;
        rockTimer = 0f;
        cooldownTimer = 0f;
    }

    private void EndSkill()
    {
        isUsingSkill = false;
        rockTimer = 0f;
        StartRoll();
    }

    private void SpawnRocks()
    {
        float angleStep = 360f / rockCount;

        for (int i = 0; i < rockCount; i++)
        {
            float angle = i * angleStep;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            Instantiate(rockPrefab, transform.position, rotation);
        }
    }
    private void StartRoll()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        isRolling = true;
        rollTarget = player.transform.position;
    }
    private void RollToPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, rollTarget, rollSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, rollTarget) <= rollStopDistance)
        {
            isRolling = false;
            //xử lí attack sau
            skillTimer = 0f;
        }
    }
}