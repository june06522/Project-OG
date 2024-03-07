using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField]
    Shop _shop;

    PlayerController playerController;
    Transform playerTrm;

    private void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        playerTrm = playerController.transform.root;
    }

    private void Update()
    {
        if (PlayerCheck() == false)
            return;

        if(Input.GetKeyDown(KeyCode.E))
        {
            _shop.OpenShop();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _shop.CloseShop();
        }
    }

    private bool PlayerCheck()
    {
        return (Vector2.Distance(transform.position, playerTrm.position) < 2f);
    }
}
