using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EventTriggerManager : MonoBehaviour
{
    public static EventTriggerManager Instance;

    static ulong index = 0;

    Rigidbody2D _playerRb;

    private void Awake()
    {
        #region 싱긅톤
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : EventTriggerManager is multiply running!");
            Destroy(gameObject);

        }
        #endregion

        _playerRb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CoolExecute();
        IdleExecute();
        RunExecute();
    }

    public static ulong GetIndex()
    {
        if (index++ == ulong.MaxValue)
            index = ulong.MinValue;
        return index;
    }

    #region 익스큐트 관리
    public void CoolExecute()
    {
        PlayerController.EventController.OnCoolExecute();

    }

    public void IdleExecute()
    {
        if (_playerRb.velocity == Vector2.zero)
            PlayerController.EventController.OnIdleExecute();
    }

    public void BasicAttackExecute()
    {
        PlayerController.EventController.OnBasicAttackExecute();
    }

    public void RunExecute()
    {
        if (_playerRb.velocity != Vector2.zero)
            PlayerController.EventController.OnMoveExecute();
    }

    public void DashExecute()
    {
        PlayerController.EventController.OnDashExecute();
    }
    #endregion
}
