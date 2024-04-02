using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRaycast : MonoBehaviour
{
    private InvenBrick[] invenBricks;
    public InvenBrick[] InvenBricks => invenBricks;

    private void Update()
    {
        invenBricks = GetComponentsInChildren<InvenBrick>();
        Debug.Log(invenBricks.Length);
        foreach (InvenBrick brick in invenBricks)
        {
            RectTransform rectTransform = brick.GetComponent<RectTransform>();

            int x = (int)rectTransform.rect.width / 100;
            int y = (int)rectTransform.rect.height / 100;
            float len = GameManager.Instance.Inventory.tileRength;

            bool isOpen = false;
            Vector2Int invenPos = new Vector2Int(-1, -1);
            Vector2 pos = rectTransform.position;
            pos -= new Vector2(x * len / 2, y * len / 2);
            Vector2 curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            while (curPos.x > pos.x)
            {
                pos.x += len;
                invenPos.x++;
            }
            while (curPos.y > pos.y)
            {
                pos.y += len;
                invenPos.y++;
            }
            foreach (var v in brick.InvenObject.bricks)
            {
                if (v.point == invenPos)
                {
                    isOpen = true;
                }    
            }
            Image image = brick.GetComponent<Image>();

            if (isOpen)
            {
                image.raycastTarget = true;
            }
            else
            {

                image.raycastTarget = false;
            }
        } 

    }
}
