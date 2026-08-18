using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    // Dictionary lưu trữ tất cả các state theo kiểu dữ liệu (Type) của chúng
    private Dictionary<Type, IState> states = new Dictionary<Type, IState>();
    public IState CurrentState { get; private set; }
    // Thêm một state vào danh sách quản lý
    public void AddState(IState state)
    {
        Type stateType = state.GetType();
        if (!states.ContainsKey(stateType))
        {
            states.Add(stateType, state);
        }
    }
    // Khởi tạo trạng thái bắt đầu (gọi khi vào game)
    public void Initialize(Type startingStateKey)
    {
        if (states.ContainsKey(startingStateKey))
        {
            CurrentState = states[startingStateKey];
            CurrentState.Enter();
        }
    }
    // Chuyển sang trạng thái mới bằng cách truyền kiểu của State (ví dụ: ChangeState<PlayerMoveState>())
    public void ChangeState<T>() where T : IState
    {
        Type targetType = typeof(T);
        // Nếu đang ở trạng thái đó rồi thì không làm gì cả
        if (CurrentState != null && CurrentState.GetType() == targetType)
            return;
        if (states.ContainsKey(targetType))
        {
            CurrentState?.Exit();
            CurrentState = states[targetType];
            CurrentState.Enter();
        }
    }
    // Gọi hàm Update của state hiện tại
    public void LogicUpdate()
    {
        CurrentState?.LogicUpdate();
    }
    // Gọi hàm FixedUpdate của state hiện tại
    public void PhysicsUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }
}
