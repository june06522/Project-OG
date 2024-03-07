using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour
{

    [SerializeField] private InvenBrick brick;
    public bool one = true;

    private WeaponInventory inventory;
    private Transform parent;

    private void Awake()
    {
        
        inventory = GameManager.Instance.Inventory;
        parent = FindObjectOfType<WeaponInventoryViewer>().parent;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {

            var point =  inventory.CheckItemAuto(brick.InvenObject);
            if(point != null)
            {

                var obj = Instantiate(brick, Vector3.zero, Quaternion.identity, parent);
                inventory.AddItem(obj.InvenObject, Vector2Int.FloorToInt(point.Value));
                obj.Setting();
                obj.transform.localPosition = (point.Value * 100) - (new Vector2(inventory.Width, inventory.Height) * 50) + new Vector2(50, 50);

                if (one == true)
                    Destroy(gameObject);
            }

        }

        

    }

}
