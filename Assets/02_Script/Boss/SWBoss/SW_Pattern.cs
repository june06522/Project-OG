using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum ESpikeWormState
{
    None,
    Cool,
    SnakeMove,

}

public class SW_Pattern : MonoBehaviour
{
    [Header("Boss")]
    private Transform _player;
    [SerializeField]
    ESpikeWormState state = ESpikeWormState.None;

    [Header("Info")]
    [SerializeField]
    private SnakeMove _snakeMove;
    [SerializeField]
    private LayerMask _obstacleLayer;

    [Header("SnakeMove Pattern")]
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private float _radius;
    [SerializeField]
    private Transform _startPos;

    private Vector3 _dir = Vector3.up;
    private float _speed = 10.0f;
    private float _cool = 0f;

    private void Awake()
    {
        _player = GameManager.Instance.player;
    }

    private void Update()
    {
        BossStatePlay();

        if(Input.GetKeyDown(KeyCode.C))
        {
            SetBossState(ESpikeWormState.SnakeMove);
        }
    }

    private void BossStatePlay()
    {
        switch (state)
        {
            case ESpikeWormState.None:
                //
                break;
            case ESpikeWormState.Cool:
                CoolDown();
                break;
            case ESpikeWormState.SnakeMove:
                SnakeMovePattern();
                break;
        }
    }

    private void SnakeMovePattern()
    {
        _head.position += _speed * _dir * Time.deltaTime;

        if(_dir == Vector3.up || _dir == Vector3.down)
        {
            // bossY == playerY
            if (Mathf.Abs(_head.position.y - _player.position.y) < 0.2f)
            {

                if (_head.position.x < _player.position.x)
                {

                    _dir = Vector3.right;

                }
                else
                {

                    _dir = Vector3.left;

                }

            }
        }
        else
        {
            if (Mathf.Abs(_head.position.x - _player.position.x) < 0.2f)
            {

                if (_head.position.y < _player.position.y)
                {

                    _dir = Vector3.up;

                }
                else
                {

                    _dir = Vector3.down;

                }

            }

        }
        
        if(Physics2D.OverlapCircle(_head.position, _radius, _obstacleLayer))
        {
            _snakeMove.DestroyBody();
            SetCoolTime(10f);
        }


    }

    private void SetCoolTime(float time)
    {
        _cool = time;
        SetBossState(ESpikeWormState.Cool);
    }
    private void CoolDown()
    {
        _cool -= Time.deltaTime;
        if (_cool <= 0)
            state = ESpikeWormState.None;
    }
    public void SetBossState(ESpikeWormState state)
    {
        if (this.state == ESpikeWormState.Cool)
            return;

        switch (state)
        {
            case ESpikeWormState.SnakeMove:
                _snakeMove.AddBody(30);
                _head.position = _startPos.position;
                break;
        }
        this.state = state;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_head.position, _radius);
        
    }
#endif
}
