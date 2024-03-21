using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTest : MonoBehaviour, IInteractable
{
    [SerializeField]
    Shop _shop;

    private void Start()
    {
        _shop = GameObject.FindAnyObjectByType<Shop>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            _shop.PlayerMoney.EarnGold(10);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _shop.CloseShop();
        }

    }

    public void OnInteract()
    {
        Debug.Log("상호작용");
        _shop.OpenShop();
    }
}
