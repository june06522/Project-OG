using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class EMPBomb : MonoBehaviour
{
    public List<SpriteRenderer> renderers;
    [SerializeField] private float jumpPower = 3f;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float bombRadius = 2f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private AudioClip _bombClip;
    [SerializeField] GameObject bombEffect;

    float damage;

    Animator animator;

    private readonly int _BombHash = Animator.StringToHash("Bomb");

    private void Awake()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();

    }

    private void Start()
    {
        Destroy(gameObject,3f);
    }

    public void Throw(Vector3 targetPos, float damage, float interval = 0f, float offset = 1f)
    {

        this.damage = damage;

        transform.DOJump(targetPos, jumpPower, 0, duration)
            .AppendInterval(interval)
            .OnComplete(() => Boom(targetPos, offset));
        //transform.DOScale(transform.localScale * 1.5f, 0.5f).SetLoops(1, LoopType.Yoyo);

    }

    private void Boom(Vector3 targetPos, float offset)
    {
        if (_bombClip != null)
        {

            SoundManager.Instance.SFXPlay("Bomb", _bombClip, 0.5f);

        }

        bombRadius *= offset;

        if (bombEffect != null)
        {
            var obj = Instantiate(bombEffect, transform.position, Quaternion.identity);
            obj.transform.localScale *= offset;
            Destroy(obj, 1f);
        }
        EnemyHitCheck(targetPos);
        animator.SetTrigger(_BombHash);

    }

    private void EnemyHitCheck(Vector3 targetPos)
    {

        Collider2D[] col = new Collider2D[20];
        int count = Physics2D.OverlapCircleNonAlloc(targetPos, bombRadius, col, layerMask);

        for (int i = 0; i < count; i++)
        {

            IHitAble hitAble;

            if (col[i].TryGetComponent<IHitAble>(out hitAble))
            {

                hitAble.Hit(damage);
            }

        }

    }
}
