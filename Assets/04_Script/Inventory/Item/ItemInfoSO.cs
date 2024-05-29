using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum ItemRate
{
    NORMAL,
    RARE,
    EPIC,
    LEGEND
}

[CreateAssetMenu(menuName = "SO/Inventory/ItemInfoSO")]
public class ItemInfoSO : ScriptableObject
{
    public Item ItemObject = null;
    public InvenBrick Brick = null;
    public ItemRate Rate = ItemRate.NORMAL;
    public string ItemName = string.Empty;

    public Sprite Sprite = null;
    private Transform _parent;

#if UNITY_EDITOR
    private void OnValidate()
    {
        
        //Brick = Brick.GetComponent<InvenBrick>();
        if (Brick == null)
            Debug.LogError("Don't find Item Class");
        else
        {
            Image image = Brick.GetComponent<Image>();
            if( image != null )
                Sprite = image.sprite;
        }
    }
#endif

    public bool GetItem()
    {
        _parent = FindObjectOfType<WeaponInventoryViewer>().parent;
        WeaponInventory inventory = GameManager.Instance.Inventory;

        var point = inventory.CheckItemAuto(Brick.InvenObject);
        if (point != null)
        {

            var obj = Instantiate(Brick, Vector3.zero, Quaternion.identity, _parent);
            obj.Setting();
            obj.transform.localPosition = (point.Value * 100) - (new Vector2(inventory.StartWidth, inventory.StartHeight) * 50) + new Vector2(50, 50);
            obj.transform.localPosition += new Vector3((obj.GetComponent<RectTransform>().rect.width - 100) / 2, (obj.GetComponent<RectTransform>().rect.height - 100) / 2);
            inventory.AddItem(obj.InvenObject, Vector2Int.FloorToInt(point.Value), obj);

            return true;
        }
        else
            return false;
    }
}
