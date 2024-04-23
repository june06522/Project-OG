using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkillLine : MonoBehaviour
{
    [SerializeField] GameObject _impact;

    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] EdgeCollider2D _edgeCollider;

    public LineRenderer LineRenderer => _lineRenderer;
    public EdgeCollider2D EdgeCollider => _edgeCollider;

    private float damage;

    float _delayTime = 0.1f;
    float _curdelay = 0f;

    private void Awake()
    {
        _curdelay = _delayTime;
    }

    private void Update()
    {
        _curdelay -= Time.deltaTime; 
    }

    public void SetLine(Vector2 startPos, Vector2 endPos, float damage, float width = 0.5f)
    {
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.startWidth = width;
        _lineRenderer.SetPosition(1, endPos);
        _lineRenderer.endWidth = width;
        this.damage = damage;

        Instantiate(_impact, endPos, Quaternion.identity);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && collision.TryGetComponent<IHitAble>(out var h) && _curdelay <= 0f)
        {
            _curdelay = _delayTime;
            h.Hit(damage);
        }
    }
}
