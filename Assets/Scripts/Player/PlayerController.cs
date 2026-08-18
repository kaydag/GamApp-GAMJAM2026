using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rb;
    public VariableJoystick Joystick;
    public Animator Animator;
    public float MoveSpeed = 5f;
    // Lưu lại hướng nhìn cuối cùng (mặc định cho hướng xuống hoặc lên tùy bạn)
    public Vector2 LastDirection { get; set; } = new Vector2(0, -1);
    public PlayerStateMachine StateMachine { get; private set; }
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();
        StateMachine.AddState(new PlayerIdleState(this));
        StateMachine.AddState(new PlayerMoveState(this));
    }
    private void Start() => StateMachine.Initialize(typeof(PlayerIdleState));
    private void Update() => StateMachine.LogicUpdate();
    private void FixedUpdate() => StateMachine.PhysicsUpdate();
}