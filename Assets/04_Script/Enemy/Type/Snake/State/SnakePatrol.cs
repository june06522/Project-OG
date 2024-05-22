using System.Collections.Generic;
using UnityEngine;

public class SnakePatrol : MonoBehaviour
{
    // Cashing
    private Transform _root;
    private Transform _target;

    // Move Value
    private List<Vector2> _points = new List<Vector2>();

    private Vector2 _movePos;
    private float _lerpValue;
    private float _speed = 8f;
    private bool _isLeftSnakeMove = false;
    private bool _isMoveToPlayer = false;

    private void Awake()
    {
        _target = GameManager.Instance.player;
        _root = transform;

        ResetLerpData();
    }

    private void Update()
    {
        // Move
        float distance = Vector2.Distance(_target.position, _root.position);
        if (distance < 2f)
        {
            if (distance < 0.1f)
                return;

            _isMoveToPlayer = true;
            _movePos = _target.position;
            SetRootAngle();
            RootMove();
            return;
        }

        WindingMove();
    }

    // 베지어 곡선을 활용한 이동
    private void WindingMove()
    {
        if(Vector2.Distance(_root.position, _movePos) < 0.1f || _isMoveToPlayer)
            CalculateMovePos();

        SetRootAngle();
        RootMove();

    }

    private void CalculateMovePos()
    {
        // Recalculate
        if(_lerpValue >= 1)
        {
            ResetLerpData();
        }

        // lerp calculate
        _lerpValue += 0.1f;

        _movePos = _points[0];
        for(int i = 1; i < _points.Count; ++i)
        {
            _movePos = Vector2.Lerp(_movePos, _points[i], _lerpValue);
        }

        _isMoveToPlayer = false;
    }
    private void SetRootAngle()
    {
        Vector2 dir = (_movePos - (Vector2)_root.position).normalized;
        _root.up = dir;
    }
    private void RootMove()
    {
        Vector3 dir = (_movePos - (Vector2)_root.position).normalized;
        _root.position += Time.deltaTime * dir * _speed;
    }

    private void ResetLerpData()
    {
        _points.Clear();
        _points.Add(_root.position);

        Vector3 dir = (_target.position - _root.position).normalized;
        Vector3 rotateDir = RotateDir(dir);

        _points.Add(_root.position + (dir * 2f) + (rotateDir * 10f));

        _points.Add(_root.position + dir * 4f);

        _lerpValue = 0;
    }
    private Vector2 RotateDir(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x);
        angle += ((_isLeftSnakeMove ? -90f : 90f) * Mathf.Deg2Rad);
        _isLeftSnakeMove = !_isLeftSnakeMove;

        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

}
