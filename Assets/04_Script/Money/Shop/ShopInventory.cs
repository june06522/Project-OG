using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventory : MonoBehaviour
{
    [SerializeField] private Transform tile;
    [SerializeField] private Transform block;
    [SerializeField] private Transform lineRender;

    [Header("Prefab")]
    [SerializeField] private InvenSlot slotPrefab;
    [SerializeField] private Image temp;

    private WeaponInventory inventory;

    List<InvenSlot> slots = new();

    private void OnEnable()
    {
        ReSetScale();
        CreateSlot();
    }

    void Update()
    {
        CreateImage();
        SetScale();
        SetPos();
        //DrawLineRender();
    }

    private void OnDisable()
    {
        DestroySlot();
    }

    void CreateSlot()
    {
        inventory = GameManager.Instance.Inventory;

        foreach (var v in inventory.GetSlot())
        {
            Vector2Int point = v.point;
            var pos = (tile.position + (Vector3)((Vector2)point * 100 * transform.localScale.x)) - (new Vector3(inventory.Width, inventory.Height) * 100 * transform.localScale.x / 2);

            var slot = Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, tile);
            slot.invenPoint = point;
            slot.transform.position = pos + new Vector3(50 * transform.localScale.x, 50 * transform.localScale.y);
            slots.Add(slot);
        }
    }

    void DestroySlot()
    {
        foreach (var v in slots)
        {
            Destroy(v.gameObject);
        }
        slots.Clear();
    }

    void CreateImage()
    {
        for (int i = block.childCount - 1; i >= 0; i--)
        {
            Destroy(block.GetChild(i).gameObject);
        }

        RectTransform[] obj = GameManager.Instance.Inventory.viewer.parent.parent.GetComponentsInChildren<RectTransform>();

        foreach (var v in obj)
        {
            if (v.GetComponent<InvenBrick>() == null) continue;
            Image blockTile = Instantiate(temp, block);
            blockTile.sprite = v.GetComponent<Image>().sprite;
            blockTile.GetComponent<RectTransform>().sizeDelta = v.rect.size;
            blockTile.transform.localPosition = v.transform.localPosition;
        }
    }

    void SetScale()
    {
        float x = GameManager.Instance.Inventory.GetInvenSize();
        x = Mathf.Min(x, 12);

        float size;

        if (x == 7)
            size = 1f;
        else if (x < 7)
            size = 1 + ((7 - x) / x);
        else
            size = 1 - ((x - 7) / x);


        tile.localScale = new Vector3(size, size, 1);
        block.localScale = new Vector3(size, size, 1);
    }

    void ReSetScale()
    {
        tile.localScale = new Vector3(1, 1, 1);
    }

    void SetPos()
    {
        block.localPosition = new Vector3(InventorySlotCenter.Instance.width * (50 * tile.localScale.x), InventorySlotCenter.Instance.height * (50 * tile.localScale.y));
        tile.localPosition = new Vector3(0, InventorySlotCenter.Instance.height * (100 * tile.localScale.y));
    }

    void DrawLineRender()
    {
        if(lineRender != null)
        {
            for (int i = lineRender.childCount - 1; i >= 0; i--)
            {
                Destroy(lineRender.GetChild(i).gameObject);
            }

            LineRenderer[] obj = GameManager.Instance.Inventory.viewer.parent.parent.GetComponentsInChildren<LineRenderer>();

            foreach (var v in obj)
            {
                if (v.positionCount <= 1) continue;
                LineRenderer line = Instantiate(v,lineRender);
                line = v;
            }
        }
    }
}