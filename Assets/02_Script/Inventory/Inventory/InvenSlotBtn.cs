using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlotBtn : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int pos;

    public void Add()
    {
        
        if (GameManager.Instance.Inventory.isNewWidth(pos.y))
            GameManager.Instance.Inventory.AddHeight();
        if (GameManager.Instance.Inventory.isNewHeight(pos.x))
            GameManager.Instance.Inventory.AddWidth();

        GameManager.Instance.Inventory.ExcuteSlotEvent(pos);
        GameManager.Instance.Inventory.AddSlot(pos);
        ExpansionManager.Instance.UseSlot();
        Destroy(gameObject);
    }

}
