using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerManager : MonoBehaviour
{
    Rigidbody2D _playerRb;

    private void Awake()
    {
        _playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        PlayerController.EventController.OnCoolExecute();

        if (_playerRb.velocity == Vector2.zero)
            PlayerController.EventController.OnIdleExecute();

    }
}
