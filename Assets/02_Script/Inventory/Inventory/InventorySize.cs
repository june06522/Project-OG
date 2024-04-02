using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class InventorySize : MonoBehaviour
{
    RectTransform rect;
    PixelPerfectCamera pixelSize;
    WeaponInventory weaponInventory;
    ConnectVisible connectVisible;

    private void Awake()
    {
        connectVisible = GetComponent<ConnectVisible>();
        rect = GetComponent<RectTransform>();
        pixelSize = Camera.main.GetComponent<PixelPerfectCamera>();
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

        if(7 - x < 0)
            rect.localScale = new Vector3(1 + (7 - x) * 0.08f, 1 + (7 - x) * 0.08f, 1);
        else
            rect.localScale = new Vector3(1 + (7 - x) * 0.12f, 1 + (7 - x) * 0.12f, 1);
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
