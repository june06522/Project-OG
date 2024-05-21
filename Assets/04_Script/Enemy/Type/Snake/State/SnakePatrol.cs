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
    private float _distance;
    private float _speed = 25f;

    private void Awake()
    {
        _target = GameManager.Instance.player;
        _root = transform;

        ResetLerpData();
    }

    private void Update()
    {
        // Move
        WindingMove();
    }

    // 베지어 곡선을 활용한 이동
    private void WindingMove()
    {
        if(Vector2.Distance(_root.position, _movePos) < 0.1f)
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
        _lerpValue += 0.01f * _speed;

        _movePos = _points[0];
        for(int i = 1; i < _points.Count; ++i)
        {
            _movePos = Vector2.Lerp(_movePos, _points[i], _lerpValue);
        }

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

        // Winding Position
        int windingCount = 2;
        Vector2 dir = (_target.position - _root.position).normalized;
        for (int i = 1; i <= windingCount; ++i)
        {
            float posPartValue = (1f / (windingCount+1)) * i;
            Vector2 partPos = Vector2.Lerp(_root.position, _target.position, posPartValue);




        }

        


        _points.Add(_target.position);

        _distance = Vector2.Distance(_root.position, _target.position) + 30f;
        _lerpValue = 0;
    }
}
