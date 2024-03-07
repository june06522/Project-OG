using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    private readonly int IdleHash = Animator.StringToHash("Idle");
    private readonly int MoveHash = Animator.StringToHash("Move");
    private readonly int AttackHash = Animator.StringToHash("Attack");

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetMove(bool value)
    {
        if(MoveHash != 0)
            animator.SetBool(MoveHash, value);
        animator.SetBool(IdleHash, !value);
    }
    public void SetAttack()
    {
        if(AttackHash != 0)
            animator.SetTrigger(AttackHash);
    }

    public void Flip(Vector2 dir)
    {
        spriteRenderer.flipX = dir.x > 0;
    }

    public void StopAnimator() => SetAnimatorSpeed(0);
    public void SetAnimatorSpeed(float speed) => animator.speed = speed;

}
