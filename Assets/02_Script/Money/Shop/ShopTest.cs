using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTest : MonoBehaviour
{
    [SerializeField]
    Shop _shop;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _shop.OpenShop();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _shop.CloseShop();
        }
    }
}
