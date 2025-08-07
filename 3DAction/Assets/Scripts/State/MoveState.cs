using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MoveState : IState
{
    PlayerController playerController;
    Animator animator;

    float moveSpeed = 10.0f;
    float applySpeed = 0.3f; // ��]���x
    Vector3 moveDirection;
    Rigidbody rb;

    public MoveState(PlayerController player)
    {
        this.playerController = player;
        animator = player.gameObject.GetComponent<Animator>();
        rb = player.gameObject.GetComponent<Rigidbody>();
    }

    public override void OnAttack()
    {
        playerController.StateMachine.Transition(playerController.StateMachine.attackState);
    }

    public override void Enter()
    {
        animator.SetBool("Running", true);
    }

    public override void Update()
    {
        if (playerController.moveInput.magnitude < 0.1f)
        {
            playerController.StateMachine.Transition(playerController.StateMachine.idleState);
            return;
        }

        // �J�����̌�������Ɉړ��������v�Z
        Vector3 camForward = playerController.refCamera.transform.forward;
        Vector3 camRight = playerController.refCamera.transform.right;

        // �㉺�����������iXZ���ʂɓ��e�j
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // ���͂ɉ������ړ��x�N�g��
        moveDirection = (camRight * playerController.moveInput.x + camForward * playerController.moveInput.y).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        playerController.gameObject.transform.rotation = Quaternion.Slerp(playerController.gameObject.transform.rotation, targetRotation, applySpeed);

        // ���ۂɈړ�
        rb.transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    public override void Exit()
    {
        animator.SetBool("Running", false);
    }
}
