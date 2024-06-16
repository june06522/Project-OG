using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotCenter : MonoBehaviour
{
    public static InventorySlotCenter Instance;

    private ConnectVisible _cv;

    RectTransform rect;

    [HideInInspector]
    public int width = 0;
    [HideInInspector]
    public int height = 0;

    [HideInInspector]
    public int minuswidth = 0;
    [HideInInspector]
    public int minusheight = 0;


    private int _lastWidth = 0;
    private int _lastheight = 0;

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
        _cv = FindObjectOfType<ConnectVisible>();
    }

    //private void Update()
    //{
    //    SetPos();
    //}

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
        if (_lastheight != height || _lastWidth != width)
        {
            _lastWidth = width;
            _lastheight = height;

            rect.localPosition = new Vector3(width * (50 * rect.localScale.x), height * (50 * rect.localScale.y));

            _cv.VisibleLineAllChange(true);
        }
    }
}
