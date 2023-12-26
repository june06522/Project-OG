using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenWeapon : Weapon
{

    [SerializeField] private InventoryObjectData objectData;

    protected override void Awake()
    {

        base.Awake();

        objectData = Instantiate(objectData);
        objectData.Init(transform);

    }

    protected override void Attack(Transform target)
    {



    }

}
