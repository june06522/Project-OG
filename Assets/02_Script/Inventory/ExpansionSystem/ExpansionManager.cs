using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ExpansionManager : MonoBehaviour
{
    public static ExpansionManager Instance;

    [SerializeField] Transform tileParent;
    [SerializeField] InvenSlotBtn plusObj;
    [SerializeField] Canvas canvas;

    private int _leftCnt = 0;
    public int leftCnt => _leftCnt;

    Vector2Int[] dxy =
        { new Vector2Int(0,1),
        new Vector2Int(0,-1),
        new Vector2Int(1, 0),
        new Vector2Int(-1, 0) };

    private void Awake()
    {
        #region 싱글톤
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : ExpansionManager is multiply running!");
            Destroy(gameObject);
        }
        #endregion

        #region 예외처리
        if (tileParent == null)
            Debug.LogError($"{transform} : tileParent is null!");

        if (plusObj == null)
            Debug.LogError($"{transform} : plusObj is null!");

        if (canvas == null)
            Debug.LogError($"{transform} : canvas is null!");
        #endregion

    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
            AddSlotcnt(5);
        if (Input.GetKeyUp(KeyCode.X))
            UseSlot(5);

        if (Input.GetKeyUp(KeyCode.C))
        ShowAddTileBtn();
    }

    public void AddSlotcnt(int plusVal = 1)
    {
        _leftCnt += plusVal;
        ShowAddTileBtn();
    }

    public void UseSlot(int miusVal = 1)
    {
        _leftCnt -= miusVal;
        ShowAddTileBtn();
    }

    public void ShowAddTileBtn()
    {
        DeleteChild();
        if (_leftCnt > 0)
        {
            tileParent.gameObject.SetActive(true);
        }
        else
        {
            tileParent.gameObject.SetActive(false);
        }
        AddChild();
    }

    public void DeleteChild()
    {
        foreach (Transform child in tileParent.GetComponentInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }
    }

    public void AddChild()
    {
        List<SlotData> slot = GameManager.Instance.Inventory.GetSlot();

        foreach (SlotData slotData in slot)
        {
            if (slotData != null)
            {
                Vector2Int pos = slotData.point;
                for (int i = 0; i < 4; i++)
                {
                  
                    if (!GameManager.Instance.Inventory.IsExist(pos + dxy[i]))
                    {
                        CreateBtn(pos + dxy[i]);
                        //
                    }
                }
            }
        }

    }

    private void CreateBtn(Vector2Int point)
    {
        Vector3 pos = GameManager.Instance.Inventory.viewer.GetPosition(point);
        var obj = Instantiate(plusObj, tileParent);
        obj.pos = point;
        obj.transform.localPosition = pos;
    }
}