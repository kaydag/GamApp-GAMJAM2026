using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtState : IState
{
    private PlayerController player;
    private float hurtDuration;
    private float timer;
    public PlayerHurtState(PlayerController player, float hurtDuration = 0.2f)
    {
        this.player = player;
        this.hurtDuration = hurtDuration;
    }
    public void Enter()
    {
        timer = 0f;
        player.Animator.SetTrigger("Hurt");
    }
    public void LogicUpdate()
    {
        timer += Time.deltaTime;
        // Khi hết thời gian animation
        if (timer >= hurtDuration)
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
        player.Rb.velocity = Vector2.zero;
    }
    public void Exit()
    {
        player.Animator.ResetTrigger("Hurt");
        timer = 0f;
    }
}
