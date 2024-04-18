using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeMove : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField]
    private Transform _mainHPObjectTransform;
    private IHitAble _mainHPObject;

    [Header("BodyInfo")]
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private Transform _bodyRootTrm;
    [SerializeField]
    private List<Transform> _bodyList = new List<Transform>();

    [SerializeField]
    private List<Transform> _tailList = new List<Transform>();

    public List<Transform> BodyList => _bodyList;
    public List<Transform> TailList => _tailList;

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
    [SerializeField]
    private string _bodyObject_SpriteObjectName;
    private readonly int HASH_BLINK = Shader.PropertyToID("_StrongTintFade");

    private void Awake()
    {
        _startPos = _head.position;

        if(_mainHPObjectTransform != null)
        {

            _mainHPObject = _mainHPObjectTransform.GetComponent<IHitAble>();

        }

        if(_mainHPObject != null && _head.TryGetComponent<HPLinkObject>(out HPLinkObject hpLink))
        {
            hpLink.Link(_mainHPObject);
        }
        
        
    }

    private void Update()
    {
        if (_isPlayedDestroy == false)
            MoveParts();
        else
            DestroyMove();
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
            if (distance > _bodyMaxInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMaxInterval);
            }
            else if (BackToFrontDir == Vector3.zero)
                break;

            backPart.up = BackToFrontDir;
            if (distance < _bodyMinInterval)
            {
                backPart.position = frontPart.position - (BackToFrontDir * _bodyMinInterval);
            }

            frontPart = backPart;
        }
    }

    public void AddBody(Vector3 dir, int cnt = 1)
    {
        for(int i = 0; i < cnt; i++) 
        {
            GameObject backPart = Instantiate(_bodyObject, _bodyRootTrm);

            if (_bodyList.Count == 0)
                backPart.transform.position = _head.position;
            else
                backPart.transform.position = _bodyList[_bodyList.Count - 1].position;
            backPart.transform.position += dir * _bodyMaxInterval;

            if (_mainHPObject != null && backPart.TryGetComponent<HPLinkObject>(out HPLinkObject hpLink))
            {
                hpLink.Link(_mainHPObject);
            }

            _bodyList.Add(backPart.transform);
        }
        
    }
    public void AddBody(bool spawnLastBodyDir = false)
    {
        Vector3 dir = Vector3.zero;
        if (spawnLastBodyDir && _bodyList.Count >= 2)
            dir = (_bodyList[_bodyList.Count - 1].position - _bodyList[_bodyList.Count - 2].position).normalized;
        else if (spawnLastBodyDir && _bodyList.Count >= 1)
            dir = (_bodyList[0].position - _head.position).normalized;

        GameObject backPart = Instantiate(_bodyObject, _bodyRootTrm);
        BlinkBody(backPart, 0.8f);

        if (_mainHPObject != null && backPart.TryGetComponent<HPLinkObject>(out HPLinkObject hpLink))
        {
            hpLink.Link(_mainHPObject);
        }

        if (_bodyList.Count == 0)
            backPart.transform.position = _head.position;
        else
            backPart.transform.position = _bodyList[_bodyList.Count - 1].position;
        backPart.transform.position += dir * _bodyMaxInterval;

        _bodyList.Add(backPart.transform);
    }

    public void BlinkBody(GameObject backPart, float second)
    {
        if (string.IsNullOrEmpty(_bodyObject_SpriteObjectName) == false)
        {

            Transform findObject = backPart.transform.Find(_bodyObject_SpriteObjectName);
            SpriteRenderer backPartSprite = findObject?.GetComponent<SpriteRenderer>();

            if (backPartSprite != null)
            {
                Color saveColor = backPartSprite.color;
                backPartSprite.color = Color.white;
                backPartSprite.material.SetFloat(HASH_BLINK, 1f);

                FAED.InvokeDelay(() =>
                {
                    backPartSprite.color = saveColor;
                    backPartSprite.material.SetFloat(HASH_BLINK, 0f);
                }, second);
            }
        }
    }

    public void DestroyBody(float destroySpeed = 0f)
    {
        if (destroySpeed != 0f)
            _destroySpeed = destroySpeed;

        if (_isPlayedDestroy) return;
        _isPlayedDestroy = true;
    }
    public void ForceDestroyBody()
    {
        foreach(Transform body in _bodyList)
        {
            Destroy(body.gameObject);
        }

        foreach(Transform tail in _tailList)
        {
            Destroy(tail.gameObject);
        }

        _bodyList.Clear();
        _tailList.Clear();

        _bodyList = new List<Transform>();
        _tailList = new List<Transform>();
    }
}
