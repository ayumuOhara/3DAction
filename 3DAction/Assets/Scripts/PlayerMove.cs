using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.XR;

public class PlayerMove : MonoBehaviour
{
    Vector2 moveInput;

    PlayerControls controls;
    Camera refCamera;

    float moveSpeed = 10.0f;
    float applySpeed = 0.1f; // ��]���x
    Vector3 moveDirection;
    Rigidbody rb;
    Animator animator;

    bool isMove = true;

    bool isAttackL = false;
    bool isAttackH = false;
    bool canNextAttack = true;

    int comboStepL = 0;
    int comboStepH = 0;

    void Awake()
    {
        controls = new PlayerControls();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Attack_Light.performed += OnAttackLightPerfomed;
        controls.Player.Attack_Heavy.performed += OnAttackHeavyPerfomed;

        controls.Player.DodgeRoll.performed += OnDodgePerformed;

        controls.Player.Guard.performed += ctx => animator.SetBool("Guard", true);
        controls.Player.Guard.canceled += ctx => animator.SetBool("Guard", false);

        refCamera = Camera.main;
    }

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    void Update()
    {
        if(isMove)
        {
            Move();
        }
    }

    void Move()
    {
        if (moveInput.magnitude > 0.1f)
        {
            // �J�����̌�������Ɉړ��������v�Z
            Vector3 camForward = refCamera.transform.forward;
            Vector3 camRight = refCamera.transform.right;

            // �㉺�����������iXZ���ʂɓ��e�j
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // ���͂ɉ������ړ��x�N�g��
            moveDirection = (camRight * moveInput.x + camForward * moveInput.y).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, applySpeed);

            // ���ۂɈړ�
            rb.transform.position += moveDirection * moveSpeed * Time.deltaTime;

            animator.SetBool("Running", true);

            //Debug.DrawRay(transform.position, camRight, Color.red);
            //Debug.DrawRay(transform.position, camForward, Color.blue);
            //Debug.DrawRay(transform.position, moveDirection, Color.green);
        }
        else
        {
            animator.SetBool("Running", false);
        }        
    }

    void OnAttackStartEvent()
    {
        Debug.Log("�A�j���[�V�����J�n");

        isMove = false;

        animator.SetBool("Running", false);
    }

    void OnAttackEndEvent()
    {
        Debug.Log("�A�j���[�V�����I��");

        isMove = true;

        isAttackL = false;
        isAttackH = false;

        comboStepL = 0;
        comboStepH = 0;
    }

    void OnNextAttackEvent()
    {
        canNextAttack = true;
    }

    void OnAttackLightPerfomed(InputAction.CallbackContext ctx)
    {
        if (isAttackH) return;

        if (comboStepL >= 1 && !canNextAttack) return;

        Debug.Log("�y�U��");

        isAttackL = true;

        if (comboStepL >= 3)
        {
            comboStepL = 1;
        }
        else
        {
            comboStepL++;
        }

        switch (comboStepL)
        {
            case 1: animator.SetTrigger("Attack_L1"); break;
            case 2: animator.SetTrigger("Attack_L2"); break;
            case 3: animator.SetTrigger("Attack_L3"); break;
        }

        canNextAttack = false;
    }

    void OnAttackHeavyPerfomed(InputAction.CallbackContext ctx)
    {
        if (isAttackL) return;

        if (comboStepH >= 1 && !canNextAttack) return;

        Debug.Log("�d�U��");

        isAttackH = true;

        if (comboStepH >= 3)
        {
            comboStepH = 1;
        }
        else
        {
            comboStepH++;
        }

        switch (comboStepH)
        {
            case 1: animator.SetTrigger("Attack_H1"); break;
            case 2: animator.SetTrigger("Attack_H2"); break;
            case 3: animator.SetTrigger("Attack_H3"); break;
        }

        canNextAttack = false;
    }

    void OnDodgePerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("���");

        isMove = false;
        rb.transform.position += moveDirection * 8;
        isMove = true;
    }
}
