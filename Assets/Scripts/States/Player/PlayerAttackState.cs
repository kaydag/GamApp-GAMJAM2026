using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : IState
{
    private PlayerController player;
    private float attackDuration;
    private float timer;
    public PlayerAttackState(PlayerController player) => this.player = player;
    public void Enter()
    {
        player.Rb.velocity = Vector2.zero;
        player.Animator.SetBool("isMoving", false);
        player.Animator.SetFloat("MoveX", player.LastDirection.x);
        player.Animator.SetFloat("MoveY", player.LastDirection.y);
        // Kích hoạt Trigger Attack
        player.Animator.SetTrigger("Attack");
        attackDuration = GetCurrentAnimationClipLength("Attack");
        timer = 0f;
    }
    public void LogicUpdate()
    {
        timer += Time.deltaTime;

        // Khi hết thời gian animation
        if (timer >= attackDuration)
        {
            if (player.Joystick.Horizontal != 0
                || player.Joystick.Vertical != 0
                || Input.GetAxisRaw("Horizontal") != 0
                || Input.GetAxisRaw("Vertical") != 0)
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
        player.Animator.ResetTrigger("Attack");
        timer = 0f;
    }
    private float GetCurrentAnimationClipLength(string clipName)
    {
        if (player.Animator == null) return 0.5f;
        RuntimeAnimatorController ac = player.Animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        return 0.5f;
    }
}