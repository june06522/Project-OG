using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class SixthElite : MonoBehaviour, IHitAble
{
    [field:SerializeField]
    public FeedbackPlayer feedbackPlayer { get; set; }

    private Action _dieAction;

    private Rigidbody2D _rigid;

    [SerializeField] 
    private Transform _up;
    [SerializeField] 
    private Transform _down;
    [SerializeField] 
    private Transform _left;
    [SerializeField] 
    private Transform _right;
    [SerializeField]
    private Transform _visual;

    [SerializeField]
    private SixthEliteDataSO _so;

    [SerializeField]
    private AudioClip _hitSound;

    [SerializeField]
    private float _animationPlayTime;
    private float _currentHp;

    private bool _isDead;
    private bool _once;
    private bool _y;

    void Start()
    {
        _currentHp = _so.MaxHP;
        _dieAction += DieEvt;
        _isDead = false;
        _rigid = GetComponent<Rigidbody2D>();
        _rigid.velocity = (GameManager.Instance.player.position - transform.position).normalized * _so.Speed;
        _once = false;
        _y = false;
    }

    public bool Hit(float damage)
    {
        if (_isDead) return false;

        SoundManager.Instance.SFXPlay("HitEnemy", _hitSound, 0.55f);
        feedbackPlayer?.Play(damage + UnityEngine.Random.Range(0.25f, 1.75f));
        _currentHp -= (int)damage;

        if (_currentHp <= 0)
        {
            Die();
            return false;
        }

        return true;
    }

    private void Die()
    {
        _isDead = true;
        _dieAction?.Invoke();
    }

    private void DieEvt()
    {
        Destroy(gameObject);
    }

    private void StopImmediately()
    {
        _rigid.velocity = Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHP player;

        if (collision.gameObject.TryGetComponent<PlayerHP>(out player) && !_once)
        {
            _once = true;
            player.Hit(_so.AttackPower);

            Vector3 normalVec = -collision.contacts[0].normal;
            Vector3 reflectVec = Vector3.Reflect(_rigid.velocity, normalVec);
            _rigid.velocity = reflectVec.normalized * _so.Speed;
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Debug.Log("ddd");
            _once = false;

            StopImmediately();

            RaycastHit2D up = Physics2D.Raycast(transform.position, Vector2.up, 2, LayerMask.GetMask("Wall"));
            RaycastHit2D down = Physics2D.Raycast(transform.position, Vector2.down, 2, LayerMask.GetMask("Wall"));
            RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 2, LayerMask.GetMask("Wall"));
            RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 2, LayerMask.GetMask("Wall"));

            Vector3 originSize = Vector3.zero;
            Transform trans = null;

            if(up.collider != null)
            {
                Debug.Log("dd");
                _visual.parent = _up;
                originSize = _up.localScale;
                _y = true;
                trans = _up;
            }
            else if(down.collider != null)
            {
                _visual.parent = _down;
                originSize = _down.localScale;
                _y = true;
                trans = _down;
            }
            else if(left.collider != null)
            {
                _visual.parent = _left;
                originSize = _left.localScale;
                _y = false;
                trans = _left;
            }
            else if(right.collider != null)
            {
                _visual.parent = _right;
                originSize = _right.localScale;
                _y = false;
                trans = _right;
            }

            if(!_y)
            {
                trans.DOScaleX(originSize.x / 2, _animationPlayTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    trans.DOScaleX(originSize.x, Time.deltaTime).SetEase(Ease.InOutSine);
                    Vector2 dir = (GameManager.Instance.player.position - transform.position).normalized;
                    _rigid.velocity = dir * _so.Speed;
                });
            }
            else
            {
                trans.DOScaleY(originSize.y / 2, _animationPlayTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    trans.DOScaleY(originSize.y, Time.deltaTime).SetEase(Ease.InOutSine);
                    Vector2 dir = (GameManager.Instance.player.position - transform.position).normalized;
                    _rigid.velocity = dir * _so.Speed;
                });
            }
        }
    }
}
