public abstract class AState <TStateMachine, TState>
    where TStateMachine : AStateMachine<TStateMachine, TState>
    where TState : AState<TStateMachine, TState> {
    protected TStateMachine stateMachine;

    public virtual void Enter(TStateMachine stateMachine){
        this.stateMachine = stateMachine;
    }
    public virtual void Process(double delta){}
    public virtual void Exit(){}
}