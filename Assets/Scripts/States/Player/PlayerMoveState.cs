using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : IState
{
    private PlayerController player;
    public PlayerMoveState(PlayerController player) => this.player = player;
    public void Enter()
    {
        player.Animator.SetBool("isMoving", true);
    }
    public void LogicUpdate()
    {
        if (player.Joystick.Horizontal == 0 
            && player.Joystick.Vertical == 0 
            && Input.GetAxisRaw("Horizontal") == 0 
            && Input.GetAxisRaw("Vertical") == 0)
        {
            player.StateMachine.ChangeState<PlayerIdleState>();
        }
    }
    public void PhysicsUpdate()
    {
        float moveX = player.Joystick.Horizontal != 0 ? player.Joystick.Horizontal : Input.GetAxisRaw("Horizontal");
        float moveY = player.Joystick.Vertical != 0 ? player.Joystick.Vertical : Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(moveX, moveY).normalized;
        player.Rb.velocity = direction * player.MoveSpeed;
        // Nếu có di chuyển, cập nhật lại LastDirection
        if (direction.sqrMagnitude > 0.01f)
        {
            // Lọc để lấy hướng ưu tiên tuyệt đối (tránh bị lệch chéo nếu dùng 4 hướng đơn thuần)
            // Hoặc bạn có thể giữ nguyên direction nếu dùng Blend Tree 2D
            if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
            {
                player.LastDirection = new Vector2(moveX > 0 ? 1 : -1, 0);
            }
            else
            {
                player.LastDirection = new Vector2(0, moveY > 0 ? 1 : -1);
            }
            if (moveX < 0) player.transform.localScale = new Vector3(-Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
            else if (moveX > 0) player.transform.localScale = new Vector3(Mathf.Abs(player.transform.localScale.x), player.transform.localScale.y, player.transform.localScale.z);
            // Truyền giá trị vào Animator
            player.Animator.SetFloat("MoveX", player.LastDirection.x);
            player.Animator.SetFloat("MoveY", player.LastDirection.y);
        }
    }
    public void Exit() { }
}