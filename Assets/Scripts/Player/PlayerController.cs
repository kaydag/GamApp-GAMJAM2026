using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICollidable
{
    // Singleton
    public static PlayerController instance;
    public Rigidbody2D Rb;
    public VariableJoystick Joystick;
    public Animator Animator;
    public float MoveSpeed = 5f;
    // Lưu lại hướng nhìn cuối cùng
    public Vector2 LastDirection { get; set; } = new Vector2(0, -1);
    public PlayerStateMachine StateMachine { get; private set; }
    private Vector3 lastPosition;
    // Xử lý swap hình ảnh
    public RuntimeAnimatorController FirstFormController;
    public RuntimeAnimatorController SecondFormController;
    public bool isInSecondForm { get; private set; }
    private PlayerAttack playerAttack;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        StateMachine = new PlayerStateMachine();
        StateMachine.AddState(new PlayerIdleState(this));
        StateMachine.AddState(new PlayerMoveState(this));
        StateMachine.AddState(new PlayerAttackState(this));
        StateMachine.AddState(new PlayerSkillState(this));
        playerAttack = GetComponent<PlayerAttack>();
    }
    private void Start()
    {
        StateMachine.Initialize(typeof(PlayerIdleState));
        lastPosition = transform.position;
    }
    private void Update() => StateMachine.LogicUpdate();
    private void LateUpdate()
    {
        // Lưu lại vị trí an toàn của frame trước
        lastPosition = transform.position;
        Vector3 camPos = Camera.main.transform.position;
        camPos.x = lastPosition.x;
        camPos.y = lastPosition.y;
        Camera.main.transform.position = camPos;
    }

    public void OnCollide()
    {
        // Trả Player về lại vị trí trước khi đâm vào block để tạo cảm giác bị chặn đứng
        transform.position = lastPosition;
    }
    private void FixedUpdate() => StateMachine.PhysicsUpdate();
    public void SwapForm()
    {
        isInSecondForm = !isInSecondForm;
        Animator.runtimeAnimatorController = isInSecondForm ? SecondFormController : FirstFormController;
        // (Tùy chọn) Kích hoạt trigger chạy animation biến hình ở đây nếu có
        // Animator.SetTrigger("Swap");
        playerAttack.SwapFormAttack();
    }
}