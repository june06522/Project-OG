using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SlotData
{

    public SlotData(Vector2Int point)
    {

        this.point = point;
        isFilled = false;

    }

    public Vector2Int point;
    public bool isFilled;

}

public delegate void SlotAdded(Vector2Int point);
public delegate void SlotChanged(Vector2Int point, bool fill);

public class WeaponInventory : MonoBehaviour
{

    [field:SerializeField] public int Width { get; private set; }
    [field:SerializeField] public int Height { get; private set; }

    private WeaponInventoryViewer viewer;
    private List<SlotData> invenslots = new();
    private List<InventoryObjectData> container = new();

    public event SlotAdded OnSlotAddEvent;
    public event SlotChanged OnSlotChangeEvent;

    private void Awake()
    {
        
        viewer = FindObjectOfType<WeaponInventoryViewer>();

    }

    private void Start()
    {
        
        for(int x = 0; x < Width; x++)
        {

            for(int y = 0; y < Height; y++)
            {

                Vector2Int point = new Vector2Int(x, y);
                invenslots.Add(new SlotData(point));
                OnSlotAddEvent?.Invoke(point);

            }

        }

    }

    public void AddSlot(Vector2Int point)
    {

        invenslots.Add(new SlotData(point));
        OnSlotAddEvent?.Invoke(point);

    }

    public void FillSlot(Vector2Int point, bool value) 
    {

        var slot = invenslots.Find(x => x.point == point);

        if (slot == null) return;

        slot.isFilled = value;

    }

    public void FillSlots(List<Vector2Int> points, Vector2Int origin, bool value)
    {

        foreach(var point in points)
        {

            FillSlot(point + origin, value);

        }

    }

    public bool CheckFill(Vector2Int point)
    {

        var slot = invenslots.Find(x => x.point == point);

        return slot != null && !slot.isFilled;

    }

    public bool CheckFills(List<Vector2Int> points, Vector2Int origin)
    {

        foreach (var point in points)
        {

            if(!CheckFill(point + origin)) return false;

        }

        return true;

    }

    public bool AddItem(InventoryObjectData item, Vector2Int origin)
    {

        if(CheckFills(item.bricks, origin))
        {

            item.originPos = origin;
            container.Add(item);
            FillSlots(item.bricks, origin, true);
            return true;

        }

        return false;

    }

    public void RemoveItem(InventoryObjectData item, Vector2Int origin) 
    {
        
        container.Remove(item);
        FillSlots(item.bricks, origin, false);
    
    }

    public Vector2? CheckItemAuto(InventoryObjectData item)
    {

        foreach(var slot in invenslots)
        {

            if (slot.isFilled) continue;

            if(CheckFills(item.bricks, slot.point))
            {

                return slot.point;

            }

        }

        return null;

    }

    public InventoryObjectData GetObjectData(Vector2Int point, Vector2Int dir, Vector2Int origin)
    {

        var c = container.Find(
            x => x.signalPoints.Count != 0 ? 
            x.signalPoints.FindIndex(y => y.point + x.originPos == origin + (point + dir)) != -1 && dir == -dir
            : x.bricks.FindIndex(y => y + x.originPos == origin + (point + dir)) != -1);

        if(c == null) return null;

        return c;

    }

    public Vector2Int? FindInvenPoint(Vector2Int localPoint)
    {
        ///
        var c = viewer.slots.Find(x =>
        {

            return Vector2Int.FloorToInt(x.transform.position  / 100) == Vector2Int.FloorToInt(localPoint);

        });

        if (c == null) return null;

        return Vector2Int.FloorToInt(c.invenPoint);

    }

}
