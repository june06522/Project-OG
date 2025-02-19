using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventoryViewer : MonoBehaviour
{

    [Header("Prefab")]
    [SerializeField] private InvenSlot slotPrefab;

    [field:Space]
    [field:Header("Setting")]
    [field:SerializeField] public Transform point { get; protected set; }
    [field:SerializeField] public Transform parent { get; protected set; }

    Vector3 thispos = Vector3.zero;
    private WeaponInventory inventory;

    public List<InvenSlot> slots { get; set; } = new List<InvenSlot>();

    private void Awake()
    {
        
        thispos = point.position;
        inventory = GameManager.Instance.Inventory; 
        inventory.OnSlotAddEvent += HandleSlotAdded;

    }

    private void HandleSlotAdded(Vector2Int point)
    {

        var pos = (thispos + (Vector3)((Vector2)(point * 100))) - (new Vector3(inventory.StartWidth, inventory.StartHeight) * 100 / 2);

        //var pos = (Vector3)(new Vector2(point.x - 51.5f,point.y - 53));

        var slot = Instantiate(slotPrefab, Vector2.zero, Quaternion.identity ,parent);
        slot.invenPoint = point;
        Vector3 localpos = pos + new Vector3((int)50, (int)50);
        slot.transform.localPosition = localpos;
        slot.localPoint = localpos / 100;

        slots.Add(slot);

    }

    public Vector3 GetPosition(Vector2Int point)
    {
        var pos = (thispos + (Vector3)((Vector2)(point * 100))) - (new Vector3(inventory.StartWidth, inventory.StartHeight) * 100 / 2);

        return pos + new Vector3((int)50, (int)50);
    }

}