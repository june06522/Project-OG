using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void SetMove(bool value)
    {
    }
    public void SetAttack()
    {
    }

    public void Flip(Vector2 dir)
    {
        transform.rotation = dir.x < 0 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);
        //spriteRenderer.flipX = dir.x > 0;
    }

    public void StopAnimator() => SetAnimatorSpeed(0);
    public void SetAnimatorSpeed(float speed) => animator.speed = speed;

}
