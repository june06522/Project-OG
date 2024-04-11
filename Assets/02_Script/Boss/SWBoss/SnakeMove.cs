using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    [Header("Info")]
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private Transform _bodyRootTrm;
    [SerializeField]
    private List<Transform> _bodyList = new List<Transform>();

    [SerializeField]
    private List<Transform> _tailList = new List<Transform>();

    [Header("Value")]
    [SerializeField]
    private float _bodyMinInterval = 1f;
    [SerializeField]
    private float _bodyMaxInterval = 1.5f;
    private Vector3 _startPos;

    [Header("Destroy")]
    [SerializeField]
    private float _destroySpeed;
    [SerializeField]
    private float _destroyDistance;
    private bool _isPlayedDestroy = false;

    [Header("Object")]
    [SerializeField]
    private GameObject _bodyObject;

    private void Awake()
    {
        _startPos = _head.position;
    }

    private void Update()
    {
        if (_isPlayedDestroy == false)
            MoveParts();
        else
            DestroyMove();

        if (Input.GetKeyDown(KeyCode.L))
            AddBody();
        else if (Input.GetKeyDown(KeyCode.K))
            DestroyBody();
    }

    private void DestroyMove()
    {
        if(_bodyList.Count == 0 && _tailList.Count == 0)
        {
            _isPlayedDestroy = false;
        }

        Transform frontPart = _head;
        // body
        for (int i = 0; i < _bodyList.Count; i++)
        {
            Transform backPart = _bodyList[i];

            float distance = Vector2.Distance(frontPart.position, backPart.position);
            Vector3 p1 = frontPart.position;
            Vector3 p2 = backPart.position;
            p1.z = 0;
            p2.z = 0;
            Vector3 BackToFrontDir = (p1 - p2).normalized;
            if (BackToFrontDir == Vector3.zero)
                continue;

            backPart.up = BackToFrontDir;
            if (distance > _bodyMaxInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMaxInterval);
            }

            backPart.position += BackToFrontDir * _destroySpeed * Time.deltaTime;
            if(Vector2.Distance(_head.position, backPart.position) <= _destroyDistance)
            {
                _bodyList.RemoveAt(i);
                i--;

                Destroy(backPart.gameObject);
            }    
            else
            {
                frontPart = backPart;
            }
        }

        // tail
        for (int i = 0; i < _tailList.Count; i++)
        {
            Transform backPart = _tailList[i];

            float distance = Vector2.Distance(frontPart.position, backPart.position);
            Vector3 p1 = frontPart.position;
            Vector3 p2 = backPart.position;
            p1.z = 0;
            p2.z = 0;
            Vector3 BackToFrontDir = (p1 - p2).normalized;
            if (BackToFrontDir == Vector3.zero)
                continue;

            backPart.up = BackToFrontDir;
            if (distance > _bodyMaxInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMaxInterval);
            }

            backPart.position += BackToFrontDir * _destroySpeed * Time.deltaTime;
            if (Vector2.Distance(_head.position, backPart.position) <= _destroyDistance)
            {
                _tailList.RemoveAt(i);
                i--;

                Destroy(backPart.gameObject);
            }
            else
            {
                frontPart = backPart;
            }
        }
    }

    public void MoveParts()
    {
        Transform frontPart = _head;
        // body
        for(int i = 0; i < _bodyList.Count; i++)
        {
            Transform backPart = _bodyList[i];
            

            float distance = Vector2.Distance(frontPart.position, backPart.position);
            Vector3 p1 = frontPart.position;
            Vector3 p2 = backPart.position;
            p1.z = 0;
            p2.z = 0;
            Vector3 BackToFrontDir = (p1 - p2).normalized;

            backPart.up = BackToFrontDir;
            if (distance > _bodyMaxInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMaxInterval);
            }
            else if(backPart.position != _startPos && distance < _bodyMinInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMinInterval);
            }

            frontPart = backPart;
        }

        // tail
        for (int i = 0; i < _tailList.Count; i++)
        {
            Transform backPart = _tailList[i];


            float distance = Vector2.Distance(frontPart.position, backPart.position);
            Vector3 p1 = frontPart.position;
            Vector3 p2 = backPart.position;
            p1.z = 0;
            p2.z = 0;
            Vector3 BackToFrontDir = (p1 - p2).normalized;
            if (BackToFrontDir == Vector3.zero)
                continue;

            backPart.up = BackToFrontDir;
            if (distance < _bodyMinInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMinInterval);
            }
            else if (distance > _bodyMaxInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMaxInterval);
            }

            frontPart = backPart;
        }
    }

    public void AddBody(int cnt = 1)
    {
        for(int i = 0; i < cnt; i++) 
        {
            GameObject backPart = Instantiate(_bodyObject, _bodyRootTrm);
            if (_bodyList.Count == 0)
                backPart.transform.position = _head.position;
            else
                backPart.transform.position = _bodyList[_bodyList.Count - 1].position;
            _bodyList.Add(backPart.transform);
        }
        
    }

    public void DestroyBody()
    {
        if (_isPlayedDestroy) return;
        _isPlayedDestroy = true;
    }
}
