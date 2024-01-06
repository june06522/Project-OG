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
    [field:SerializeField] public Transform point { get; protected set; } //인벤토리의 중앙지점
    [field:SerializeField] public Transform parent { get; protected set; } //인벤토리 슬롯들의 부모 오브젝트

    private WeaponInventory inventory;

    public List<InvenSlot> slots { get; set; } = new List<InvenSlot>();

    private void Awake()
    {
        
        inventory = FindObjectOfType<WeaponInventory>();
        inventory.OnSlotAddEvent += HandleSlotAdded;

    }

    private void HandleSlotAdded(Vector2Int point)
    {

        var pos = (this.point.position + (Vector3)((Vector2)point * 100)) - (new Vector3(inventory.Width, inventory.Height) * 100 / 2);

        var slot = Instantiate(slotPrefab, Vector2.zero, Quaternion.identity ,parent);
        slot.invenPoint = point;
        slot.transform.position = pos + new Vector3(50, 50);
        slot.localPoint = slot.transform.localPosition / 100;

        slots.Add(slot);

    }

}
