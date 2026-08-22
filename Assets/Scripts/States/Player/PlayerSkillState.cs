using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillState : IState
{
    private PlayerController player;
    private float attackTimer = 0f;
    private float attackDuration = 0.5f;
    public PlayerSkillState(PlayerController player) => this.player = player;
    public void Enter()
    {
        // Dừng di chuyển
        player.Rb.velocity = Vector2.zero;
        player.Animator.SetBool("isMoving", false);
        player.Animator.SetFloat("MoveX", player.LastDirection.x);
        player.Animator.SetFloat("MoveY", player.LastDirection.y);
        // Kích hoạt Trigger
        player.Animator.SetTrigger("Skill");
        // Reset bộ đếm
        attackTimer = 0f;
    }
    public void LogicUpdate()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDuration)
        {
            if (player.Joystick.Horizontal != 0 || player.Joystick.Vertical != 0)
            {
                player.StateMachine.ChangeState<PlayerMoveState>();
            }
            else
            {
                player.StateMachine.ChangeState<PlayerIdleState>();
            }
        }
    }
    public void PhysicsUpdate()
    {
        // Giữ đứng yên trong lúc tấn công
        player.Rb.velocity = Vector2.zero;
    }
    public void Exit()
    {

    }
}
