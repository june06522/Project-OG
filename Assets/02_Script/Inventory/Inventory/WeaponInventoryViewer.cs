using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryViewer : MonoBehaviour
{

    private WeaponInventory inventory;

    private void Awake()
    {
        
        inventory = FindObjectOfType<WeaponInventory>();
        inventory.OnSlotAddEvent += HandleSlotAdded;

    }

    private void HandleSlotAdded(Vector2 point)
    {



    }

}
