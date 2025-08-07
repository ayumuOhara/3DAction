using UnityEngine;

public abstract class IState
{
    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }

    public virtual void OnAttack() { }
    public virtual void OnGuard() { }
    public virtual void OnDodge() { }
}
