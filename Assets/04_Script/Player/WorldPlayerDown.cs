using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayerDown : MonoBehaviour
{
    [SerializeField]
    Transform _worldObject;

    [SerializeField]
    private float _speed;

    private void FixedUpdate()
    {

        _worldObject.position += Vector3.up * _speed * Time.fixedDeltaTime;

    }

}
