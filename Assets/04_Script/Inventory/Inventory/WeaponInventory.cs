using System.Collections.Generic;
using UnityEngine;

public class SlotData
{

    public SlotData(Vector2Int point)
    {

        this.point = point;
        isFilled = false;

    }

    public Vector2Int point;
    public bool isFilled;
    public bool isOn;
}

public delegate void SlotAdded(Vector2Int point);
public delegate void CameraSetting();
public delegate void AddItem();

public class WeaponInventory : MonoBehaviour
{

    [field: SerializeField] public int Width { get; private set; }
    [field: SerializeField] public int Height { get; private set; }

    [HideInInspector] public int StartWidth;
    [HideInInspector] public int StartHeight;

    public WeaponInventoryViewer viewer { get; private set; }
    private List<SlotData> invenslots = new();
    private List<InventoryObjectData> container = new();
    private ConnectVisible line;

    public event SlotAdded OnSlotAddEvent;
    public event CameraSetting camerasetting;
    public event AddItem OnAddItem;
    //public event SlotChanged OnSlotChangeEvent;

    [HideInInspector]
    public float tileRength = 1000;

    private void Awake()
    {

        line = FindObjectOfType<ConnectVisible>();
        viewer = FindObjectOfType<WeaponInventoryViewer>();

    }

    private void Start()
    {
        StartWidth = Width;
        StartHeight = Height;
        for (int x = 0; x < Width; x++)
        {

            for (int y = 0; y < Height; y++)
            {

                Vector2Int point = new Vector2Int(x, y);
                invenslots.Add(new SlotData(point));
                OnSlotAddEvent?.Invoke(point);

            }

        }
        FindObjectOfType<InvenSelectBrickShow>().Init();
        camerasetting?.Invoke();
    }

    public void CheckTileLen()
    {
        if (FindObjectOfType<InventorySize>().slotPrt.childCount >= 2)
        {
            Vector3 child1 = FindObjectOfType<InventorySize>().slotPrt.GetChild(0).transform.position;
            Vector3 child2 = FindObjectOfType<InventorySize>().slotPrt.GetChild(1).transform.position;
            float x = Mathf.Abs(child1.x - child2.x);
            float y = Mathf.Abs(child1.y - child2.y);
            tileRength = x + y;
        }
        else
            tileRength = 1000;
    }

    private void Update()
    {
        CheckTileLen();
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

    public void FillSlots(List<BrickPoint> points, Vector2Int origin, bool value)
    {

        foreach (var point in points)
        {

            FillSlot(point.point + origin, value);

        }

    }

    public bool CheckFill(Vector2Int point)
    {

        var slot = invenslots.Find(x => x.point == point);
        return slot != null && !slot.isFilled;

    }

    public bool CheckFill2(Vector2Int point)
    {

        var slot = invenslots.Find(x => x.point == point);
        return slot != null && slot.isFilled;

    }

    public bool CheckFills(List<BrickPoint> points, Vector2Int origin)
    {

        foreach (var point in points)
        {

            if (!CheckFill(point.point + origin)) return false;

        }

        return true;

    }

    public bool AddItem(InventoryObjectData item, Vector2Int origin, InvenBrick brick)
    {

        if (CheckFills(item.bricks, origin))
        {

            item.originPos = origin;
            container.Add(item);
            FillSlots(item.bricks, origin, true);
            line.AddBrick(brick);
            OnAddItem?.Invoke();
            return true;

        }

        return false;

    }

    public void RemoveItem(InventoryObjectData item, Vector2Int origin)
    {

        container.Remove(item);
        OnAddItem?.Invoke();
        FillSlots(item.bricks, origin, false);

    }

    public Vector2? CheckItemAuto(InventoryObjectData item)
    {

        foreach (var slot in invenslots)
        {

            //if (slot.isFilled) continue;

            if (CheckFills(item.bricks, slot.point))
            {
                OnAddItem?.Invoke();
                return slot.point;

            }

        }

        return null;

    }

    public bool IsExist(Vector2Int pos)
    {
        foreach (var v in invenslots)
        {
            if (v.point == pos)
            {
                return true;
            }
        }
        return false;
    }

    public InventoryObjectData GetObjectData(Vector2Int point, Vector2Int dir, Vector2Int origin)
    {
        var c = container.Find(x => x.inputPoints.Count != 0 ?
        x.sendPoints.FindIndex(y => y.point + x.originPos == origin + (point + dir) && y.dir == -dir) != -1 :
        x.bricks.FindIndex(y => y.point + x.originPos == origin + (point + dir)) != -1);
        if (c == null) return null;

        return c;

    }

    public InventoryObjectData GetObjectData2(Vector2Int pos, Vector2Int dir)
    {
        foreach (var item in container)
        {
            foreach (BrickPoint p in item.bricks)
            {
                if (p.point + item.originPos == pos)
                {
                    return item;
                }
            }
        }
        return null;
    }

    public bool IsNewWidth(int y, bool settingSize = true)
    {
        foreach (var v in invenslots)
        {
            if (v.point.y == y)
            {
                return false;
            }
        }
        if(settingSize)
            SetWidth(y);
        return true;
    }

    public bool IsNewHeight(int x, bool settingSize = true)
    {
        foreach (var v in invenslots)
        {
            if (v.point.x == x)
            {
                return false;
            }
        }
        if(settingSize)
            SetHeight(x);
        return true;
    }

    public Vector2Int? FindInvenPoint(Vector2Int localPoint)
    {
        if(StartWidth % 2 == 0)
            localPoint += new Vector2Int(1, 0);

        var c = viewer.slots.Find(x =>
        {
            return Vector2Int.RoundToInt(x.GetComponent<RectTransform>().localPosition / 100) == Vector2Int.RoundToInt(localPoint);

        });

        if (c == null) return null;

        return Vector2Int.RoundToInt(c.invenPoint);

    }

    public Vector3? FindInvenPointPos(Vector2Int localPoint)
    {

        var c = viewer.slots.Find(x =>
        {
            return Vector2Int.RoundToInt(x.GetComponent<RectTransform>().localPosition / 100) == Vector2Int.RoundToInt(localPoint);

        });

        if (c == null) return null;
        return c.GetComponent<RectTransform>().position;

    }

    public List<SlotData> GetSlot()
    {
        return invenslots;
    }

    public void AddSlotData(Vector2Int point)
    {
        invenslots.Add(new SlotData(point));
    }

    //public void ExcuteSlotEvent(Vector2Int pos) => OnSlotAddEvent?.Invoke(pos);

    public int GetInvenSize() => Mathf.Max(Width, Height);

    public int AddHeight() => Height++;

    public int AddWidth() => Width++;

    private void SetWidth(int val)
    {
        val++;
        foreach (var v in invenslots)
        {
            if (v.point.y == val)
            {
                InventorySlotCenter.Instance?.ChangeHeight(1);
                return;
            }
        }
        InventorySlotCenter.Instance?.ChangeHeight(-1);

    }

    private void SetHeight(int val)
    {
        val++;
        foreach (var v in invenslots)
        {
            if (v.point.x == val)
            {
                InventorySlotCenter.Instance?.ChangeWidth(1);
                return;
            }
        }
        InventorySlotCenter.Instance?.ChangeWidth(-1);

    }

    public void SettingLineRender() => OnAddItem?.Invoke();
}
