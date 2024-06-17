using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

// inspector에서 보이는 변수들
public partial class SixthElite
{
    [Header("Transform")]
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

    [Header("SO")]
    [SerializeField]
    private SixthEliteDataSO _so;

    [Header("SixthEliteOnly")]
    [SerializeField]
    private float _animationPlayTime;

    [Header("SpriteRenderer")]
    [SerializeField]
    private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
}

public partial class SixthElite : MonoBehaviour
{
    private List<Material> _materials = new List<Material>();

    private Rigidbody2D _rigid;

    private LineRenderer _warningLine;

    private bool _once;
    private bool _y;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();

        _warningLine = GetComponent<LineRenderer>();
    }

    void Start()
    {
        _once = false;
        _y = false;

        foreach(SpriteRenderer matSprite in _spriteRenderers)
        {
            _materials.Add(matSprite.material);
        }

        StartCoroutine(StartAttack(1));
    }

    private void Update()
    {
        Vector2 dir = GameManager.Instance.player.position - transform.position;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _visual.transform.rotation = Quaternion.Euler(0, 0, z - 90);
    }

    private IEnumerator StartAttack(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        _rigid.velocity = (GameManager.Instance.player.position - transform.position).normalized * _so.Speed;
        StartCoroutine(Charging(waitTime, transform.position, _rigid.velocity));
    }

    private void StopImmediately()
    {
        _rigid.velocity = Vector2.zero;
    }

    private IEnumerator Charging(float chargingTime, Vector2 originPos, Vector2 dir)
    {
        float currentTime = 0;

        if(WallCheck(dir) != Vector2.zero)
        {
            ShowLine(_warningLine, originPos, dir, Color.red, 1);
        }

        while(currentTime < chargingTime)
        {
            currentTime += Time.deltaTime;

            foreach(Material mat in _materials)
            {
                mat.SetFloat("_VibrateFade", 1);
            }

            yield return null;
        }

        foreach (Material mat in _materials)
        {
            mat.SetFloat("_VibrateFade", 0);
        }
        _warningLine.enabled = false;
    }

    private void ShowLine(LineRenderer line, Vector2 originPos, Vector2 dir, Color color,  float scale)
    {
        line.enabled = true;
        line.startWidth = scale;
        line.startColor = color * new Color(1, 1, 1, 0.2f);
        line.SetPosition(0, originPos);
        line.SetPosition(1, WallCheck(dir));
        line.endWidth = scale;
        line.endColor = color * new Color(1, 1, 1, 0.2f);
    }

    private Vector2 WallCheck(Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Mathf.Infinity, LayerMask.GetMask("Wall"));

        if(hit.point != null)
        {
            return hit.point;
        }

        return Vector2.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Vector3.up * 3);
        Gizmos.DrawRay(transform.position, Vector3.down * 3);
        Gizmos.DrawRay(transform.position, Vector3.left * 3);
        Gizmos.DrawRay(transform.position, Vector3.right * 3);
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
            _once = false;

            StopImmediately();

            RaycastHit2D up = Physics2D.Raycast(transform.position, Vector2.up, 3, LayerMask.GetMask("Wall"));
            RaycastHit2D down = Physics2D.Raycast(transform.position, Vector2.down, 3, LayerMask.GetMask("Wall"));
            RaycastHit2D left = Physics2D.Raycast(transform.position, Vector2.left, 3, LayerMask.GetMask("Wall"));
            RaycastHit2D right = Physics2D.Raycast(transform.position, Vector2.right, 3, LayerMask.GetMask("Wall"));

            Vector3 originSize = Vector3.zero;
            Transform trans = null;

            if(up.collider != null)
            {
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
                Vector2 dir = (GameManager.Instance.player.position - transform.position).normalized;
                Vector3 correction = Vector2.zero;
                if(trans == _right)
                {
                    correction = Vector2.left * 1f;
                }
                else
                {
                    correction = Vector2.right * 1f;
                }
                StartCoroutine(Charging(_animationPlayTime, _visual.parent.position + correction, dir));
                Debug.Log("X Charging");
                trans.DOScaleX(originSize.x / 2, _animationPlayTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    Debug.Log("X Charging Ended");
                    trans.DOScaleX(originSize.x, Time.deltaTime).SetEase(Ease.InOutSine);
                    _rigid.velocity = dir * _so.Speed;
                    
                });
            }
            else
            {
                Vector2 dir = (GameManager.Instance.player.position - transform.position).normalized;
                Vector3 correction = Vector2.zero;
                if (trans == _up)
                {
                    correction = Vector2.down * 1f;
                }
                else
                {
                    correction = Vector2.up * 1f;
                }
                StartCoroutine(Charging(_animationPlayTime, _visual.parent.position + correction, dir));
                Debug.Log("Y Charging");
                trans.DOScaleY(originSize.y / 2, _animationPlayTime)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    Debug.Log("Y Charging Ended");
                    trans.DOScaleY(originSize.y, Time.deltaTime).SetEase(Ease.InOutSine);
                    _rigid.velocity = dir * _so.Speed;
                    
                });
            }
        }
    }
}
