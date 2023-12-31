using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{

    [SerializeField] private InvenBrick brick;

    private WeaponInventory inventory;
    private Transform parent;

    private void Awake()
    {
        
        inventory = FindObjectOfType<WeaponInventory>();
        parent = FindObjectOfType<WeaponInventoryViewer>().parent;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            var point =  inventory.CheckItemAuto(brick.InvenObject);

            if(point != null)
            {

                var obj = Instantiate(brick, (point.Value * 100) - new Vector2(50, 50), Quaternion.identity, parent);
                inventory.AddItem(obj.InvenObject, Vector2Int.FloorToInt(point.Value));

            }

        }

    }

}
