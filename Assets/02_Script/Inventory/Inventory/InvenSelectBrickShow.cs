using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenSelectBrickShow : MonoBehaviour
{
    private RectTransform _rect;
    private InventoryRaycast _invenRaycast;
    [SerializeField] Image _showUI;

    bool isOpen = false;

    public void Init()
    {
        _rect = _showUI.GetComponent<RectTransform>();
        _invenRaycast = GetComponent<InventoryRaycast>();
        SetDefalut();
    }

    private void LateUpdate()
    {
        isOpen = false;
        foreach (var v in _invenRaycast.InvenBricks)
        {
            if (v.IsDrag)
            {
                isOpen = true;
                Show();
                _showUI.sprite = v.Image.sprite;
                _showUI.rectTransform.sizeDelta = v.RectTransform.rect.size;


                Vector3 tempPos = v.RectTransform.localPosition;
                tempPos.x += (v.RectTransform.rect.width / 100 % 2 == 0) ? 50 : 0;
                tempPos.y += (v.RectTransform.rect.height / 100 % 2 == 0) ? 50 : 0;

                if (GameManager.Instance.Inventory.Width % 2 == 0)
                    tempPos.x -= 50;
                if (GameManager.Instance.Inventory.Height % 2 == 0)
                    tempPos.y -= 50;

                Vector3Int p = Vector3Int.RoundToInt(tempPos / 100);
                p.x -= (int)(v.RectTransform.rect.width / 200);
                p.y -= (int)(v.RectTransform.rect.height / 200);
                p.z = 0;
                Vector2Int p2 = Vector2Int.RoundToInt(tempPos / 100);
                p2.x -= (int)(v.RectTransform.rect.width / 200);
                p2.y -= (int)(v.RectTransform.rect.height / 200);
                var point = GameManager.Instance.Inventory.FindInvenPoint(p2);

                if (point == null || !GameManager.Instance.Inventory.CheckFills(v.InvenObject.bricks, point.Value))
                {
                    SetDefalut();
                }
                else
                {
                    _rect.localPosition = p * 100;

                    _rect.localPosition += new Vector3((_rect.rect.width - 100) / 2, (_rect.rect.height - 100) / 2);

                    if (GameManager.Instance.Inventory.Width % 2 == 0)
                        _rect.localPosition += new Vector3(50, 0);
                    if (GameManager.Instance.Inventory.Height % 2 == 0)
                        _rect.localPosition += new Vector3(0, 50);

                }
            }
        }

        if (!isOpen)
        {
            SetDefalut();
        }
    }

    private void SetDefalut()
    {
        _showUI.gameObject.SetActive(false);
    }

    private void Show()
    {
        _showUI.gameObject.SetActive(true);

    }
}
