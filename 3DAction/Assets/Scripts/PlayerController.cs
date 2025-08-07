using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum ATTACK_TYPE
    {
        LIGHT,
        HEAVY,
    }

    public ATTACK_TYPE attack;

    public PlayerControls controls;
    public Vector2 moveInput;

    public Camera refCamera;

    StateMachine stateMachine;

    public StateMachine StateMachine => stateMachine;

    void OnEnable() => controls.Player.Enable();
    void OnDisable() => controls.Player.Disable();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        refCamera = Camera.main;

        stateMachine = new StateMachine(this);
        stateMachine.Initialize(stateMachine.idleState);

        controls = new PlayerControls();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Player.Attack_Light.performed += OnAttackLightInput;
        controls.Player.Attack_Heavy.performed += OnAttackHeavyInput;

        controls.Player.DodgeRoll.performed += OnDodgeInput;

        controls.Player.Guard.performed += OnGuardInput;
        controls.Player.Guard.canceled += OnGuardOutput;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stateMachine.Update();
    }

    void OnAttackLightInput(InputAction.CallbackContext ctx)
    {
        attack = ATTACK_TYPE.LIGHT;
        StateMachine.CurrentState?.OnAttack();
    }

    void OnAttackHeavyInput(InputAction.CallbackContext ctx)
    {
        attack = ATTACK_TYPE.HEAVY;
        StateMachine.CurrentState?.OnAttack();
    }

    void OnDodgeInput(InputAction.CallbackContext ctx)
    {
        StateMachine.CurrentState?.OnDodge();
    }

    void OnGuardInput(InputAction.CallbackContext ctx)
    {
        StateMachine.CurrentState?.OnGuard();
    }

    void OnGuardOutput(InputAction.CallbackContext ctx)
    {
        StateMachine.CurrentState?.OnGuard();
    }

    void OnAttackEndEvent()
    {
        Debug.Log("アニメーション終了");
        
        if (moveInput.magnitude > 0.1f)
        {
            stateMachine.Transition(stateMachine.moveState);
        }
        else
        {
            stateMachine.Transition(stateMachine.idleState);
        }
    }

    void OnNextAttackEvent()
    {
        Debug.Log("コンボ攻撃可能");

        if (stateMachine.CurrentState == stateMachine.attackState)
        {
            stateMachine.attackState.ReadyNextAttack();
        }
    }
}
