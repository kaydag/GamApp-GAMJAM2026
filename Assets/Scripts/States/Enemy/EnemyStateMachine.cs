using UnityEngine;

public class EnemyStateMachine
{
    public IState CurrentState { get; private set; }
    public void Initialize(IState startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(IState newState)
    {
        if (newState == CurrentState) return;
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public void LogicUpdate()
    {
        CurrentState?.LogicUpdate();
    }
    public void PhysicsUpdate()
    {
        CurrentState?.PhysicsUpdate();
    }
}