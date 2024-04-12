using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class InventorySize : MonoBehaviour
{
    RectTransform rect;
    RectTransform inven;
    PixelPerfectCamera pixelSize;
    WeaponInventory weaponInventory;
    ConnectVisible connectVisible;

    [HideInInspector]
    public float ratio;
    [HideInInspector]
    public float positionRatio;
    
    public Transform slotPrt;

    private void Awake()
    {
        connectVisible = GetComponent<ConnectVisible>();
        rect = GetComponent<RectTransform>();
        //pixelSize = FindObjectOfType<PixelPerfectCamera>();
        inven = transform.parent.parent.parent.GetComponent<RectTransform>();
    }

    private void Start()
    {
        weaponInventory = GameManager.Instance.Inventory;
    }

    private void Update()
    {

        SetInvenScale();
        SettingLineRender();
    }

    private void SetInvenScale()
    {
        int x = Mathf.Max(weaponInventory.Width, weaponInventory.Height);

        if (ExpansionManager.Instance.leftCnt > 0)
            x += 2;

        if (7 - x < 0)
            rect.localScale = new Vector3(1 + (7 - x) * 0.08f, 1 + (7 - x) * 0.08f, 1);
        else
            rect.localScale = new Vector3(1 + (7 - x) * 0.12f, 1 + (7 - x) * 0.12f, 1);

        //if (pixelSize.assetsPPU == 100)
        //{
        //    inven.localScale = new Vector3(1 / 2f, 1 / 2f);
        //}

        //ratio = 50f / (pixelSize.assetsPPU);
        //inven.localScale = new Vector3(ratio, ratio);

        //positionRatio = (pixelSize.assetsPPU);
    }

    private void SettingLineRender()
    {
        if (GameManager.Instance.Inventory.GetInvenSize() > 7)
        {
            connectVisible.mulX = (2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.16f);// * ratio;
            connectVisible.mulY = (2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.16f);// * ratio;
        }
        else
        {
            connectVisible.mulX = (2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.25f);// * ratio;
            connectVisible.mulY = (2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.25f);// * ratio;
        }
    }
}
