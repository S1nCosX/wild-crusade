using Godot;

public abstract class AStateMachine<TStateMachine, TState>
    where TStateMachine : AStateMachine<TStateMachine, TState>
    where TState : AState<TStateMachine, TState> {
    TState CurrentState_;
    public TState CurrentState => CurrentState_;

    public void ChangeState(TState state) {
        CurrentState_?.Exit();
        CurrentState_ = state;
        CurrentState_.Enter((TStateMachine) this);
    }

    public void Process(double delta) {
        CurrentState_?.Process(delta);
    }
}