using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Item : MonoBehaviour, IInteractable
{

    [SerializeField] private InvenBrick brick;
    public InvenBrick Brick => brick;
    public bool one = true;

    public BrickType brickType;

    private WeaponInventory inventory;
    private Transform parent;

    public event Action<Transform> OnInteractItem;

    private void Awake()
    {

        inventory = GameManager.Instance.Inventory;

        if (brick.InvenObject.colorMat != null)
            parent = GameManager.Instance.invenAddType.generator;
        else if(brick.InvenObject.sendPoints.Count == 0)
            parent = GameManager.Instance.invenAddType.weapon;
        else
            parent = GameManager.Instance.invenAddType.connector;
    }

    public void OnInteract()
    {
        var point = inventory.CheckItemAuto(brick.InvenObject);
        if (point != null)
        {
            PlaySceneEffectSound.Instance.PlayItemEat();

            var obj = Instantiate(brick, Vector3.zero, Quaternion.identity, parent);
            obj.GetComponent<Image>().enabled = false;
            obj.Setting();
            obj.transform.localPosition = (point.Value * 100) - (new Vector2(inventory.StartWidth, inventory.StartHeight) * 50) + new Vector2(50, 50);
            obj.transform.localPosition += new Vector3((obj.GetComponent<RectTransform>().rect.width - 100) / 2, (obj.GetComponent<RectTransform>().rect.height - 100) / 2);
            obj.GetComponent<Image>().enabled = true;
            inventory.AddItem(obj.InvenObject, Vector2Int.FloorToInt(point.Value), obj);

            if (one == true)
            {
                OnInteractItem?.Invoke(transform);
                Destroy(gameObject);
            }
        }
        else
        {
            GameManager.Instance.InventoryActive.WarningTextInvenFull();
        }
    }

    public InvenBrick GetBrick() => brick;
}
