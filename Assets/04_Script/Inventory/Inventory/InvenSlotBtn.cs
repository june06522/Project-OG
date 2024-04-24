using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenSlotBtn : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clickSound;

    [HideInInspector]
    public Vector2Int pos;

    public void Add()
    {
        
        if (GameManager.Instance.Inventory.IsNewWidth(pos.y))
            GameManager.Instance.Inventory.AddHeight();
        if (GameManager.Instance.Inventory.IsNewHeight(pos.x))
            GameManager.Instance.Inventory.AddWidth();

        GameManager.Instance.Inventory.ExcuteSlotEvent(pos);
        GameManager.Instance.Inventory.AddSlot(pos);
        ExpansionManager.Instance.UseSlot();
        if (_clickSound != null)
            SoundManager.Instance.SFXPlay("Click", _clickSound);
        
        Destroy(gameObject);
    }

}
