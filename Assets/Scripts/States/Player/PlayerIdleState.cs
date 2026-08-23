using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : IState
{
    private PlayerController player;
    public PlayerIdleState(PlayerController player) => this.player = player;
    public void Enter()
    {
        player.Animator.SetBool("isMoving", false);
        player.Animator.SetFloat("MoveX", player.LastDirection.x);
        player.Animator.SetFloat("MoveY", player.LastDirection.y);
    }
    public void LogicUpdate()
    {
        if (player.Joystick.Horizontal != 0 
            || player.Joystick.Vertical != 0
            || Input.GetAxisRaw("Horizontal") != 0
            || Input.GetAxisRaw("Vertical") != 0)
        {
            player.StateMachine.ChangeState<PlayerMoveState>();
        }
    }
    public void PhysicsUpdate() => player.Rb.velocity = Vector2.zero;
    public void Exit() { }
}