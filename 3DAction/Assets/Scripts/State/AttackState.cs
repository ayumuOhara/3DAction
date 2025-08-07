using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : IState
{
    PlayerController playerController;
    Animator animator;

    bool isAttackL = false;
    bool isAttackH = false;
    bool canNextAttack = true;

    int comboStepL = 0;
    int comboStepH = 0;

    public AttackState(PlayerController player)
    {
        this.playerController = player;
        animator = player.gameObject.GetComponent<Animator>();
    }

    public override void Enter()
    {
        comboStepL = 0;
        comboStepH = 0;

        OnAttack();
    }

    public override void Exit()
    {
        comboStepL = 0;
        comboStepH = 0;

        isAttackL = false;
        isAttackH = false;
    }

    public void ReadyNextAttack()
    {
        canNextAttack = true;
    }

    public override void OnAttack()
    {
        if (playerController.attack == PlayerController.ATTACK_TYPE.LIGHT)
        {
            LightCombo();
        }
        else if (playerController.attack == PlayerController.ATTACK_TYPE.HEAVY)
        {
            HeavyCombo();
        }

        canNextAttack = false;
    }

    void LightCombo()
    {
        if (isAttackH || comboStepL >= 1 && !canNextAttack) return;

        Debug.Log("ŒyUŒ‚");

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
    }

    void HeavyCombo()
    {
        if (isAttackL || comboStepH >= 1 && !canNextAttack) return;

        Debug.Log("dUŒ‚");

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
    }
}
