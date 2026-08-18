using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void LogicUpdate(); // Xử lý input, chuyển state
    void PhysicsUpdate(); // Xử lý vật lý (Rigidbody)
    void Exit();
}
