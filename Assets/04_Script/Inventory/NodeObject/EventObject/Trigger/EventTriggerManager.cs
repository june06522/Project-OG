using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerManager : MonoBehaviour
{
    Rigidbody _playerRb;

    private void Awake()
    {
        _playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayerController.EventController.OnCoolExecute();

        if (_playerRb.velocity == Vector3.zero)
            PlayerController.EventController.OnIdleExecute();

    }
}
