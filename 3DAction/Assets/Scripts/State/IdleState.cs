using UnityEditor.ShaderGraph;
using UnityEngine;

public class IdleState : IState
{
    PlayerController playerController;
    Animator animator;

    public IdleState(PlayerController player)
    {
        this.playerController = player;
        animator = player.gameObject.GetComponent<Animator>();
    }

    public override void OnAttack()
    {
        playerController.StateMachine.Transition(playerController.StateMachine.attackState);
    }

    public override void Enter()
    {
        animator.SetBool("Idle", true);
    }

    public override void Update()
    {
        if (playerController.moveInput.magnitude > 0.1f)
        {
            playerController.StateMachine.Transition(playerController.StateMachine.moveState);
        }
    }

    public override void Exit()
    {
        animator.SetBool("Idle", false);
    }
}
