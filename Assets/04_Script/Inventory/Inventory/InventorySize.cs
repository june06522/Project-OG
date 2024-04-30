using UnityEngine;

public class InventorySize : MonoBehaviour
{
    RectTransform rect;
    ConnectVisible connectVisible;

    [HideInInspector]
    public float ratio;
    [HideInInspector]
    public float positionRatio;

    public Transform slotPrt;

    private void Awake()
    {
        connectVisible = GetComponent<ConnectVisible>();
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {

        SetInvenScale();
        SettingLineRender();
    }

    private void SetInvenScale()
    {
        float x = GetSize();
        x = Mathf.Min(x, 12) + 2;

        float size;

        //9개일때 100 - 200 / 9
        //8개일때 100 - 100 / 8 -> 87
        //7개 일때 700 이면 개당 100
        //6개일때 100 + 100 / 6 -> 1.17f
        //5개일때 100 + 200 / 5 -> 1.4f
        
        if (x == 7)
            size = 1f;
        else if (x < 7)
            size = 1 + ((7 - x) / x);
        else
            size = 1 - ((x - 7) / x);


        rect.localScale = new Vector3(size, size, 1);
    }


    private void SettingLineRender()
    {
        if (GetSize() > 7)
        {
            connectVisible.mulX = (0.2f + (7 - GetSize()) * 0.04f);// * ratio;
        }
        else
        {
            connectVisible.mulY = (0.2f + (7 - GetSize()) * 0.02f);// * ratio;
        }
    }

    public int GetSize()
    {
        return GameManager.Instance.Inventory.GetInvenSize();// + (ExpansionManager.Instance.leftCnt > 0 ? 2 : 0);
    }
}