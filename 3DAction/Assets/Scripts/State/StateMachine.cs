using UnityEngine;

public class StateMachine
{
    public IState CurrentState { get; private set; }

    public IdleState idleState;
    public MoveState moveState;
    public AttackState attackState;
    public GuardState guardState;
    public DodgeRollState dodgeRollState;

    public StateMachine(PlayerController player)
    {
        this.idleState = new IdleState(player);
        this.moveState = new MoveState(player);
        this.attackState = new AttackState(player);
        this.guardState = new GuardState(player);
        this.dodgeRollState = new DodgeRollState(player);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();
    }

    public void Transition(IState nextState)
    {
        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}
