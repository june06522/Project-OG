using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour, IInteractable
{

    [SerializeField] private InvenBrick brick;
    public bool one = true;

    private WeaponInventory inventory;
    private Transform parent;

    private void Awake()
    {
        
        inventory = GameManager.Instance.Inventory;
        parent = FindObjectOfType<WeaponInventoryViewer>().parent;

    }

    public void OnInteract()
    {
        var point = inventory.CheckItemAuto(brick.InvenObject);
        if (point != null)
        {

            var obj = Instantiate(brick, Vector3.zero, Quaternion.identity, parent);
            obj.GetComponent<Image>().enabled = false;
            inventory.AddItem(obj.InvenObject, Vector2Int.FloorToInt(point.Value));
            obj.Setting();
            obj.transform.localPosition = (point.Value * 100) - (new Vector2(inventory.Width, inventory.Height) * 50) + new Vector2(50, 50);
            obj.transform.localPosition += new Vector3((obj.GetComponent<RectTransform>().rect.width - 100) / 2, (obj.GetComponent<RectTransform>().rect.height - 100) / 2);
            obj.origin = gameObject;

            if (one == true)
                gameObject.SetActive(false);
        }
    }
}
