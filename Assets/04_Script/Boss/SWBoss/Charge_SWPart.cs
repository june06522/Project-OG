using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge_SWPart : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Transform _head;

    private float _damage;
    private float _speed;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();

    }

    private void LateUpdate()
    {
        SetDangerLine();
        MoveObject();
    }

    public void SetPartInfo(Transform head, float damage, float speed)
    {
        _head = head;
        _damage = damage;
        _speed = speed;
    }

    private void MoveObject()
    {
        Vector3 dir = _head.position - transform.position;
        dir.z = 0;
        dir.Normalize();

        transform.position += dir * _speed * Time.deltaTime;
    }

    private void SetDangerLine()
    {
        if(Vector3.Distance(_head.position, transform.position) <= 50f)
        {
            _lineRenderer.SetPositions(new Vector3[] {
                transform.position, _head.position
            });
        }

        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(collision.TryGetComponent<IHitAble>(out IHitAble hitObject))
            {
                hitObject.Hit(_damage);
            }
        }
    }
}
