using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class TestObj : MonoBehaviour, IInteractable
{
    [SerializeField] Item[] items;

    public bool one = true;
    SpriteRenderer spriteRenderer;
    private WeaponInventory inventory;
    Transform parent;

    Rigidbody2D rb2d;
    BoxCollider2D boxCollider;

    public event Action<Transform> OnInteractItem;

    [SerializeField]
    int index = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        inventory = FindObjectOfType<WeaponInventory>();

        if (items[0].GetBrick().InvenObject.colorMat != null)
            parent = GameManager.Instance.invenAddType.generator;
        else if (items[0].GetBrick().InvenObject.sendPoints.Count == 0)
            parent = GameManager.Instance.invenAddType.weapon;
        else
            parent = GameManager.Instance.invenAddType.connector;

        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0.0f;

        transform.gameObject.layer = LayerMask.NameToLayer("Interactable");
        spriteRenderer = items[index].GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SetIndex(-1);
            else if(Input.GetKeyDown(KeyCode.RightArrow))
                SetIndex(1);
        }
    }

    private void SetIndex(int num)
    {
        index += num;

        if (index < 0)
            index += items.Length;
        if (index >= items.Length)
            index -= items.Length;

        spriteRenderer = items[index].GetComponent<SpriteRenderer>();
    }

    public void OnInteract()
    {
        var point = inventory.CheckItemAuto(items[index].GetBrick().InvenObject);
        if (point != null)
        {
            PlaySceneEffectSound.Instance.PlayItemEat();

            var obj = Instantiate(items[index].GetBrick(), Vector3.zero, Quaternion.identity, parent);
            obj.GetComponent<Image>().enabled = false;
            inventory.AddItem(obj.InvenObject, Vector2Int.FloorToInt(point.Value));
            obj.Setting();
            obj.transform.localPosition = (point.Value * 100) - (new Vector2(inventory.StartWidth, inventory.StartHeight) * 50) + new Vector2(50, 50);
            obj.transform.localPosition += new Vector3((obj.GetComponent<RectTransform>().rect.width - 100) / 2, (obj.GetComponent<RectTransform>().rect.height - 100) / 2);
            obj.GetComponent<Image>().enabled = true;

            if (one == true)
            {
                OnInteractItem?.Invoke(transform);
            }
        }
    }
}
