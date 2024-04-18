using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryRaycast : MonoBehaviour
{
    private InvenBrick[] invenBricks;
    public InvenBrick[] InvenBricks => invenBricks;

    InventorySize invensize;

    private void Awake()
    {
        invensize = FindObjectOfType<InventorySize>();
    }

    private void Update()
    {
        invenBricks = GetComponentsInChildren<InvenBrick>();
        foreach (InvenBrick brick in invenBricks)
        {
            RectTransform rectTransform = brick.GetComponent<RectTransform>();

            float x = (int)rectTransform.rect.width / 100;// * invensize.ratio;
            float y = (int)rectTransform.rect.height / 100;// * invensize.ratio;
            float len = GameManager.Instance.Inventory.tileRength;// * invensize.ratio;
            if (len == 0)
                Debug.LogError("len 0 : 무한루프");
            bool isOpen = false;
            Vector2Int invenPos = new Vector2Int(-1, -1);
            Vector2 pos = rectTransform.position;// * invensize.ratio; 
            pos -= new Vector2(x * len / 2, y * len / 2);
            Vector2 curPos = Camera.main.ScreenToWorldPoint(Input.mousePosition ) ;
            //첫번재 가설 비율이 잘못된다.. 이미 스케일에 적용을했어

            //두번째 가설 스케일과 좌표의 적용방식이 다르다.


            //Debug.Log($"{pos} : {curPos}");
            
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
