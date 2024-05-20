using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ExpansionManager : MonoSingleton<ExpansionManager>
{

    [SerializeField] Transform tileParent;
    [SerializeField] InvenSlotBtn plusObj;
    [SerializeField] Canvas canvas;
    [SerializeField] int _createCnt = 3;
    [SerializeField] TextMeshProUGUI leftText;

    private int _leftCnt = 0;
    public int leftCnt => _leftCnt;

    Vector2Int[] dxy =
    { new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, -1)};

    private void Awake()
    {

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
        if (Input.GetKeyUp(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
            AddSlotcnt(5);
    }

    public void AddSlotcnt(int plusVal = 1)
    {
        _leftCnt += plusVal;
        if (_leftCnt - plusVal <= 0)
            ShowAddTileBtn();
        if (leftText != null)
            leftText.text = $"추가 갯수: {_leftCnt}";
    }

    public void UseSlot(int miusVal = 1)
    {
        _leftCnt -= miusVal;
        if (_leftCnt + miusVal > 0)
        {
            ShowAddTileBtn();
            InventorySlotCenter.Instance.SetPos();
        }
        if (leftText != null)
            leftText.text = $"추가 갯수: {_leftCnt}";
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
        List<Vector2Int> addBlock = new List<Vector2Int>();

        foreach (SlotData slotData in slot)
        {

            if (slotData != null)
            {
                Vector2Int pos = slotData.point;
                for (int i = 0; i < 4; i++)
                {

                    if (!GameManager.Instance.Inventory.IsExist(pos + dxy[i]))
                    {
                        if (CanAdd(pos + dxy[i]))
                        {
                            addBlock.Add(pos + dxy[i]);
                            //CreateBtn(pos + dxy[i]);
                        }
                    }
                }
            }
        }

        Dictionary<Vector2Int, bool> dic = new();

        for (int i = 0; i < _createCnt; i++)
        {
            if (addBlock.Count == 0)
                return;
            int idx = Random.Range(0, addBlock.Count);
            if (!dic.ContainsKey(addBlock[idx]))
            {
                dic.Add(addBlock[idx], true);
                CreateBtn(addBlock[idx]);
            }
            else
                i--;
            addBlock.RemoveAt(idx);
        }

    }

    public bool CanAdd(Vector2Int pos)
    {
        Dictionary<Vector2Int, bool> dic = new Dictionary<Vector2Int, bool>();

        if (GameManager.Instance.Inventory.IsNewWidth(pos.y, false) && GameManager.Instance.Inventory.Height >= 12)
            return false;
        if (GameManager.Instance.Inventory.IsNewHeight(pos.x, false) && GameManager.Instance.Inventory.Width >= 12)
            return false;

        foreach (var item in GameManager.Instance.Inventory.GetSlot())
        {
            dic.Add(item.point, true);
        }

        int cnt = 0;
        foreach (var v in dxy)
        {
            if (dic.ContainsKey(pos + v))
                cnt++;
        }
        if (cnt >= 3)
            return true;
        return false;
    }

    private void CreateBtn(Vector2Int point)
    {
        Vector3 pos = GameManager.Instance.Inventory.viewer.GetPosition(point);
        var obj = Instantiate(plusObj, tileParent);
        obj.pos = point;
        obj.transform.localPosition = pos;
    }
}