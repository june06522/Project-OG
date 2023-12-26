using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotData
{

    public SlotData(Vector2 point)
    {

        this.point = point;
        isFilled = false;

    }

    public Vector2 point;
    public bool isFilled;

}

public delegate void SlotAdded(Vector2 point);
public delegate void SlotChanged(Vector2 point, bool fill);

public class WeaponInventory : MonoBehaviour
{

    [field:SerializeField] public int Width { get; private set; }
    [field:SerializeField] public int Height { get; private set; }

    private List<SlotData> invenslots = new();

    public event SlotAdded OnSlotAddEvent;
    public event SlotChanged OnSlotChangeEvent;

    private void Start()
    {
        
        for(int x = 0; x < Width; x++)
        {

            for(int y = 0; y < Height; y++)
            {

                Vector2 point = new Vector2(x, y);
                invenslots.Add(new SlotData(point));
                OnSlotAddEvent?.Invoke(point);

            }

        }

    }

    public void AddSlot(Vector2 point)
    {

        invenslots.Add(new SlotData(point));
        OnSlotAddEvent?.Invoke(point);

    }

    public void FillSlot(Vector2 point, bool value) 
    {

        var slot = invenslots.Find(x => x.point == point);

        if (slot == null) return;

        slot.isFilled = value;

    }

    public void FillSlots(List<Vector2> points, bool value)
    {

        foreach(var point in points)
        {

            FillSlot(point, value);

        }

    }

    public bool CheckFill(Vector2 point)
    {

        var slot = invenslots.Find(x => x.point == point);

        return slot != null && !slot.isFilled;

    }

    public bool CheckFill(List<Vector2> points)
    {

        foreach (var point in points)
        {

            if(!CheckFill(point)) return false;

        }

        return true;

    }

}
