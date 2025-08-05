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
    float applySpeed = 1.0f; // 回転速度
    Vector3 moveDirection;
    Rigidbody rb;
    Animator animator;

    bool isMove = true;

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
            // カメラの向きを基準に移動方向を計算
            Vector3 camForward = refCamera.transform.forward;
            Vector3 camRight = refCamera.transform.right;

            // 上下方向を除く（XZ平面に投影）
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // 入力に応じた移動ベクトル
            moveDirection = (camRight * moveInput.x + camForward * moveInput.y).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, applySpeed);

            // 実際に移動
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
        isMove = false;
        animator.SetBool("Running", false);
    }

    void OnAttackEndEvent()
    {
        isMove = true;
    }

    void OnAttackLightPerfomed(InputAction.CallbackContext ctx)
    {
        Debug.Log("軽攻撃");
        animator.SetTrigger("Attack");
    }

    void OnAttackHeavyPerfomed(InputAction.CallbackContext ctx)
    {
        Debug.Log("重攻撃");
        animator.SetTrigger("Attack");
    }

    void OnDodgePerformed(InputAction.CallbackContext ctx)
    {
        Debug.Log("回避");

        isMove = false;
        rb.transform.position += moveDirection * 8;
        isMove = true;
    }
}
