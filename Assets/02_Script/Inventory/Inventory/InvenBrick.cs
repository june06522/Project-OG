using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvenBrick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [field: SerializeField] public InventoryObjectData InvenObject { get; private set; }
    public Vector2 InvenPoint { get; set; }

    public Vector3 prevPos;

    private WeaponInventory inventory;

    private bool isDrag;


    protected virtual void Awake()
    {

        InvenObject = Instantiate(InvenObject);
        InvenObject.Init(transform);
        inventory = FindObjectOfType<WeaponInventory>();

    }

    public virtual void Settings()
    {

    }

    public void Setting()
    {
        bool isControllerSetActive = GameManager.Instance.InventoryActive.inven.activeSelf == false;

        if (isControllerSetActive)
        {
            GameManager.Instance.InventoryActive.inven.SetActive(true);
        }

        Settings();

        if (isControllerSetActive)
            GameManager.Instance.InventoryActive.inven.SetActive(false);
    }

    private void Update()
    {

        if (isDrag)
        {

            transform.position = Input.mousePosition;

        }

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

        isDrag = false;

        Vector3Int p = Vector3Int.FloorToInt(transform.position / 100);
        var point = inventory.FindInvenPoint(Vector2Int.FloorToInt(transform.position / 100));

        //Debug.Log(point);

        if (point == null)
        {

            Destroy(gameObject);
            return;

        }

        // �߸� ��ġ �� ���ư���
        // �������� �߰��� �ȉ�(addItem���� �ɸ�)
        // ���̶��...
        // 

        if (inventory.CheckFills(InvenObject.bricks, point.Value))
        {
            Debug.Log("�׳� ����");
            inventory.AddItem(InvenObject, point.Value);
            InvenPoint = point.Value;

            transform.position = p * 100 + new Vector3Int(60, 40);

            Setting();
        }
        else
        {
            Debug.Log("���ļ� ����");
            Vector3Int prevP = Vector3Int.FloorToInt(prevPos / 100);
            var prev = inventory.FindInvenPoint(Vector2Int.FloorToInt(prevPos / 100));

            inventory.AddItem(InvenObject, prev.Value);
            InvenPoint = prev.Value;

            transform.position = prevP * 100 + new Vector3Int(60, 40);

            Setting();
        }

        Debug.Log(2);

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(1);
        prevPos = transform.position;

        isDrag = true;
        inventory.RemoveItem(InvenObject, InvenObject.originPos);

    }
}