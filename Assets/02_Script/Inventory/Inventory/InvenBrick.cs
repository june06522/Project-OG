using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ItemType
{
    Weapon,
    Generator,
    Connector
}

public class InvenBrick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [field: SerializeField] public InventoryObjectData InvenObject { get; private set; }
    public Vector2 InvenPoint { get; set; }

    public Vector3 prevPos;

    private WeaponInventory inventory;

    public ItemType Type = ItemType.Weapon;

    private bool isDrag;
    public bool IsDrag => isDrag;

    private RectTransform rectTransform;

    protected virtual void Awake()
    {

        InvenObject = Instantiate(InvenObject);
        InvenObject.Init(transform);
        inventory = FindObjectOfType<WeaponInventory>();
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void Settings()
    {

    }

    public void Setting()
    {
        //bool isControllerSetActive = GameManager.Instance.InventoryActive.inven.activeSelf == false;

        //if (isControllerSetActive)
        //{
        //    GameManager.Instance.InventoryActive.inven.SetActive(true);
        //}

        Settings();

        //if (isControllerSetActive)
        //    GameManager.Instance.InventoryActive.inven.SetActive(false);
    }

    private void Update()
    {

        if (isDrag)
        {

            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rectTransform.position = new Vector3(rectTransform.position.x,rectTransform.position.y,0);
        }

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

        isDrag = false;

        Vector3Int p = Vector3Int.RoundToInt(rectTransform.localPosition / 100);
        p.z = 0;
        var point = inventory.FindInvenPoint(Vector2Int.RoundToInt(rectTransform.localPosition / 100));

        if (point == null)
        {
            Destroy(gameObject);
            return;

        }

        if (inventory.CheckFills(InvenObject.bricks, point.Value))                                  
        {
            inventory.AddItem(InvenObject, point.Value);
            InvenPoint = point.Value;

            rectTransform.localPosition = p * 100;

            rectTransform.localPosition += new Vector3((rectTransform.rect.width - 100) / 2, (rectTransform.rect.height - 100) / 2);
            Setting();
        }
        else
        {

            Vector2Int prevP = Vector2Int.RoundToInt(prevPos / 100);
            var prev = inventory.FindInvenPoint(Vector2Int.RoundToInt((prevPos - new Vector3Int
                ((int)rectTransform.rect.width - 100,
                ((int)rectTransform.rect.height) - 100) / 2) / 100));

            inventory.AddItem(InvenObject, prev.Value);
            InvenPoint = prev.Value;

            transform.localPosition = prevPos;

            Setting();
        }



    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

        prevPos = transform.localPosition;

        isDrag = true;
        inventory.RemoveItem(InvenObject, InvenObject.originPos);

    }
}
