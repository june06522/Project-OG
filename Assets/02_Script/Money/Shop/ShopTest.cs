using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField]
    Shop _shop;

    Transform playerTrm;

    private void Start()
    {
        playerTrm = GameManager.Instance.player.transform;

        _shop = GameObject.FindAnyObjectByType<Shop>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            _shop.PlayerMoney.GetGold(10);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _shop.CloseShop();
        }

        if (PlayerCheck() == false)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            _shop.OpenShop();
        }

    }

    private bool PlayerCheck()
    {
        return (Vector2.Distance(transform.position, playerTrm.position) < 2f);
    }
}
