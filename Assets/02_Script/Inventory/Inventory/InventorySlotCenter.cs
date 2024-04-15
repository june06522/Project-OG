using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotCenter : MonoBehaviour
{
    public static InventorySlotCenter Instance;

    RectTransform rect;

    private int width = 0;
    private int height = 0;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : InventorySlotCenter is Multiply running!");
            Destroy(gameObject);
        }

        rect = GetComponent<RectTransform>();
    }

    public void ChangeWidth(int val)
    {
        width += val;
        SetPos();
    }

    public void ChangeHeight(int val)
    {
        height += val;
        SetPos();
    }

    private void SetPos()
    {
        rect.localPosition = new Vector3(width * (50 * rect.localScale.x), height * (50 * rect.localScale.y));
    }
}
