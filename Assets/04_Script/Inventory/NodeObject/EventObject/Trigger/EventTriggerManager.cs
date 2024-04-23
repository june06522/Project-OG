using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventTriggerManager : MonoBehaviour
{
    static ulong index = 0;

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

    public static ulong GetIndex()
    {
        if (index++ == ulong.MaxValue)
            index = ulong.MinValue;
        return index;
    }

    public void CoolExecute()
    {

    }   

    public void IdleExecute()
    {

    }

    public void RunExecute()
    {

    }

    public void DashExecute()
    {

    }
}
