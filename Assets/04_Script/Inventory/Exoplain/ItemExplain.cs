using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ExplainType
{
    None,
    Weapon,
    Generator,
    Connector,
}


public class ItemExplain : MonoBehaviour
{
    public static ItemExplain Instance;
        
    [SerializeField] Image panelImage;
    [SerializeField] WeaponExplain weaponExplain;
    [SerializeField] GeneratorExplain generatorExplain;
    [SerializeField] ConnectorExplain connectorExplain;
    [SerializeField] Transform statInfo;

    [SerializeField] List<RateColor> colorList;

    private InventoryActive inventoryActive;

    public bool isDrag = false;

    public bool useMove = false;

    Vector2 curInvenPoint;

    IconType curInfoType;
    private IconButton[] _iconButtons;

    private Transform _Title;

    private ExplainType _prevType;
    public bool IsInven = true;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform}: Item Explain is multiply running!");
        }


        inventoryActive = FindObjectOfType<InventoryActive>();
        _iconButtons = transform.Find("Icons").GetComponentsInChildren<IconButton>();
        _Title = transform.Find("Title").transform;
    }

    private void Start()
    {
        curInfoType = IconType.INVEN;
        _prevType = ExplainType.None;
        ActiveExplain(ExplainType.None);
    }

    private void ActiveExplain(ExplainType type)
    {
        bool activeWeapon, activeGenerator, activeConnector;
        activeWeapon = activeGenerator = activeConnector = false;

        switch (type)
        {
            case ExplainType.None:
                break;
            case ExplainType.Weapon: activeWeapon = true; 
                break;
            case ExplainType.Generator: activeGenerator = true;
                break;
            case ExplainType.Connector: activeConnector = true; 
                break;
        }

        if(type != ExplainType.None)
            _prevType = type;

        weaponExplain.gameObject.SetActive(activeWeapon);
        generatorExplain.gameObject.SetActive(activeGenerator);
        connectorExplain.gameObject.SetActive(activeConnector);

    }

    private void Update()
    {
        if (useMove)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.x += (GameManager.Instance.player.transform.position.x - pos.x < 0) ? -2.5f : 2.5f;


            pos.y += (GameManager.Instance.player.transform.position.y - pos.y < 0) ? -2.8f : 2.8f;
            transform.position = pos;

            if (!inventoryActive.IsOn)
            {
                HoverEnd();
            }
        }

        if (panelImage.enabled == false)
        {
            HoverEnd();
        }
    }

    public void HoverConnector(Vector2 invenPoint, Sprite image, ItemRate evaluation)
    {
        if (!IsInven) return;
        if (invenPoint == curInvenPoint) return;
        if(connectorExplain.gameObject.activeSelf == false)
        {
            ActiveExplain(ExplainType.Connector);   
        }

        curInvenPoint = invenPoint;
        connectorExplain.ON(invenPoint, image, GetRateColor(evaluation));
    }

    public void HoverWeapon(Vector2 invenPoint, Sprite image, string name, float power, string explain, Tuple<GeneratorID, SkillUIInfo>[] skillList, ItemRate evaluation)
    {
        if (!IsInven) return;
        if (invenPoint == curInvenPoint) return;
        if (weaponExplain.gameObject.activeSelf == false)
        {
            ActiveExplain(ExplainType.Weapon);
        }
        
        curInvenPoint = invenPoint;
        weaponExplain.ON(invenPoint, image, name, power, explain, skillList, GetRateColor(evaluation));
    }

    public void HoverGenerator(Vector2 invenPoint, Sprite image, string trigger, string explain, ItemRate evaluation, string name)
    {
        if (!IsInven) return;
        if (invenPoint == curInvenPoint) return;
        if (generatorExplain.gameObject.activeSelf == false)
        {
            ActiveExplain(ExplainType.Generator);
        }

        curInvenPoint = invenPoint;
        generatorExplain.ON(invenPoint, image, trigger, explain, GetRateColor(evaluation), name);
    }

    public void HoverEnd()
    {
        //weaponExplain.Active(false);
        //generatorExplain.Active(false);   
    }

    public void OnInfo()
    {
        if (IsInven == false) return;

        IsInven = false;
        _iconButtons[(int)IconType.INFO].OnConnect();
        _iconButtons[(int)IconType.INVEN].DisConnect();

        _Title.gameObject.SetActive(false);
        ActiveExplain(ExplainType.None);

        statInfo.gameObject.SetActive(true);
    }

    public void OnInven()
    {
        if (IsInven == true) return;

        IsInven = true; 
        _iconButtons[(int)IconType.INFO].DisConnect();
        _iconButtons[(int)IconType.INVEN].OnConnect();

        _Title.gameObject.SetActive(true);
        ActiveExplain(_prevType);

        statInfo.gameObject.SetActive(false);
    }

    public bool IsOn() => weaponExplain.gameObject.activeSelf;

    public RateColor GetRateColor(ItemRate currentItemRate)
    {
        return colorList.Find((item) => item.Rate == currentItemRate);
    }

    public void SetCurrentInfoType(IconType iconType)
    {
        switch (iconType)
        {
            case IconType.INFO:
                OnInfo();
                break;
            case IconType.INVEN:
                OnInven();
                break;
        }

    }
}