using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class ShopInventory : MonoBehaviour
{
    [SerializeField] private Transform tile;
    [SerializeField] private Transform block;

    [Header("Prefab")]
    [SerializeField] private InvenSlot slotPrefab;
    [SerializeField] private Image temp;

    private WeaponInventory inventory;

    private void Start()
    {
        inventory = GameManager.Instance.Inventory;
        for (int i = 0; i < inventory.Width; i++)
        {
            for (int j = 0; j < inventory.Height; j++)
            {
                Vector2Int point = new Vector2Int(i, j);
                var pos = (tile.position + (Vector3)((Vector2)point * 100 * transform.localScale.x)) - (new Vector3(inventory.Width, inventory.Height) * 100 * transform.localScale.x / 2);

                var slot = Instantiate(slotPrefab, Vector2.zero, Quaternion.identity, tile);
                slot.invenPoint = point;
                slot.transform.position = pos + new Vector3(50 * transform.localScale.x, 50 * transform.localScale.x);
            }
        }

    }

    void Update()
    {
        for (int i = block.childCount - 1; i >= 0; i--)
        {
            Destroy(block.GetChild(i).gameObject);
        }

        RectTransform[] obj = GameManager.Instance.Inventory.viewer.parent.GetComponentsInChildren<RectTransform>();

        foreach (var v in obj)
        {
            if (v.GetComponent<InvenBrick>() == null) continue;
            Image blockTile = Instantiate(temp, block);
            blockTile.sprite = v.GetComponent<Image>().sprite;
            blockTile.GetComponent<RectTransform>().sizeDelta = v.rect.size;
            blockTile.transform.localPosition = v.transform.localPosition;
        }
    }
}