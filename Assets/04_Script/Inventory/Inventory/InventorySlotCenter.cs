using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotCenter : MonoBehaviour
{
    public static InventorySlotCenter Instance;

    RectTransform rect;

    [HideInInspector]
    public int width = 0;
    [HideInInspector]
    public int height = 0;

    [HideInInspector]
    public int minuswidth = 0;
    [HideInInspector]
    public int minusheight = 0;

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

    private void Update()
    {
        SetPos();
    }

    public void ChangeWidth(int val)
    {
        minuswidth += (val > 0) ? val : 0;
        width += val;
        SetPos();
    }

    public void ChangeHeight(int val)
    {
        minusheight += (val > 0) ? val : 0;
        height += val;
        SetPos();
    }

    public void SetPos()
    {
        rect.localPosition = new Vector3(width * (50 * rect.localScale.x), height * (50 * rect.localScale.y));
    }
}
