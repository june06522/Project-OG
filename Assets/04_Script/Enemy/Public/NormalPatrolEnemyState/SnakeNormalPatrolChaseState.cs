using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeNormalPatrolChaseState : NormalPatrolRootState
{
    // Cashing
    private Transform _root;
    private Transform _target;

    // Move Value
    private List<Vector2> _points = new List<Vector2>();

    private Vector2 _movePos;
    private float _lerpValue;
    private float _distance;
    private float _lerpSpeed = 5f;

    public SnakeNormalPatrolChaseState(BaseFSM_Controller<ENormalPatrolEnemyState> controller) : base(controller)
    {
    }

    protected override void EnterState()
    {
        base.EnterState();

        _target = controller.Target;
        _root = controller.Enemy.transform;

        if (_target == null || _root == null)
            ExitState();
    }

    protected override void UpdateState()
    {
        base.UpdateState();

        // Move
        WindingMove();
    }

    protected override void ExitState()
    {
        base.ExitState();
    }

    // 베지어 곡선을 활용한 이동
    private void WindingMove()
    {
        if (Vector3.Distance(_root.position, _movePos) < 0.2f)
            CalculateMovePos();

        _root.position = _movePos;
    }

    private void CalculateMovePos()
    {
        // Recalculate
        if(_lerpValue >= 1)
        {
            _points.Clear();
            _points.Add(_root.position);
            Vector2 secondPoint = (Vector2)_target.position + Random.insideUnitCircle * 5f;
            _points.Add(secondPoint);
            _points.Add(_target.position);

            _distance = Vector2.Distance(_root.position, _target.position) + 5f;
            _lerpValue = 0;
        }

        // lerp calculate
        _lerpValue += (_lerpSpeed * Time.deltaTime) / _distance;
        _movePos = Vector2.Lerp(_points[0], _points[1], _lerpValue);    // lerpP0P1 = Lerp(P0, P1)
        _movePos = Vector2.Lerp(_movePos, _points[2], _lerpValue);      // MovePos = Lerp(lerpP0P1, P2)

    }




}
