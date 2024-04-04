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

    private void Awake()
    {
        connectVisible = GetComponent<ConnectVisible>();
        rect = GetComponent<RectTransform>();
        pixelSize = FindObjectOfType<PixelPerfectCamera>();
        inven = transform.parent.parent.parent.GetComponent<RectTransform>();
        Debug.Log(pixelSize);
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

        if (7 - x < 0)
            rect.localScale = new Vector3(1 + (7 - x) * 0.08f, 1 + (7 - x) * 0.08f, 1);
        else
            rect.localScale = new Vector3(1 + (7 - x) * 0.12f, 1 + (7 - x) * 0.12f, 1);

        if (pixelSize.assetsPPU == 100)
        {
            inven.localScale = new Vector3(1 / 2f, 1 / 2f);
        }
        
            float size = 50f / (pixelSize.assetsPPU);
            inven.localScale = new Vector3(size, size);
        
        // 0 == 2
        // 50 = 1
        // 75 == 0.75
        // 100 = 0.5\
        // 150
        // assetsPPU > 50
        // 1 * (assetsPPU - 50) / 50
        // assetsPPU < 50
        //
    }

    private void SettingLineRender()
    {
        if (GameManager.Instance.Inventory.GetInvenSize() > 7)
        {
            connectVisible.mulX = 2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.16f;
            connectVisible.mulY = 2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.16f;
        }
        else
        {
            connectVisible.mulX = 2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.25f;
            connectVisible.mulY = 2.0f + (7 - GameManager.Instance.Inventory.GetInvenSize()) * 0.25f;
        }
    }
}
