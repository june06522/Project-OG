using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemExplain : MonoBehaviour
{
    public static ItemExplain Instance;

    public enum ExplainType
    {
        None,
        Weapon,
        Generator,
        Connector,
    }

    [SerializeField] WeaponExplain weaponExplain;
    [SerializeField] GeneratorExplain generatorExplain;
    [SerializeField] ConnectorExplain connectorExplain;
    [SerializeField] Transform statInfo;

    [SerializeField] List<RateColor> colorList;

    public bool IsDrag = false;
    public bool UseMove = false;

    private Vector2 _curInvenPoint;

    private IconType curInfoType;
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

        _iconButtons = transform.Find("Icons").GetComponentsInChildren<IconButton>();
        _Title = transform.Find("TooltipPanel/Title").transform;
 
    }

    private IEnumerator Start()
    {

        yield return null;

        curInfoType = IconType.INVEN;
        _prevType = ExplainType.None;
        ActiveExplain(ExplainType.None);
        IsInven = false;

        OnInven();

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

    public void HoverConnector(Vector2 invenPoint, Sprite image, ItemRate evaluation)
    {

        if(IsCanRefreshInfo(invenPoint))
        {

            if (connectorExplain.gameObject.activeSelf == false)
            {
                ActiveExplain(ExplainType.Connector);
            }

            _curInvenPoint = invenPoint;
            connectorExplain.ON(invenPoint, image, GetRateColor(evaluation));
        }

    }

    private bool IsCanRefreshInfo(Vector2 invenPoint)
    {

        // 인벤이고 같은 아이템이 아니면 리프레시
        bool isCanRefresh = IsInven && (invenPoint != _curInvenPoint);
        return isCanRefresh;

    }

    public void HoverWeapon(Vector2 invenPoint, Sprite image, string name, float power, string explain, Tuple<GeneratorID, SkillUIInfo>[] skillList, ItemRate evaluation)
    {

        if(IsCanRefreshInfo(invenPoint))
        {

            if (weaponExplain.gameObject.activeSelf == false)
            {
                ActiveExplain(ExplainType.Weapon);
            }
        
            _curInvenPoint = invenPoint;
            weaponExplain.ON(invenPoint, image, name, power, explain, skillList, GetRateColor(evaluation));
        }

    }

    public void HoverGenerator(Vector2 invenPoint, Sprite image, string trigger, string explain, ItemRate evaluation, string name, string enforceInfo)
    {
        if (!IsInven) return;
        if (invenPoint == _curInvenPoint) return;
        if (generatorExplain.gameObject.activeSelf == false)
        {
            ActiveExplain(ExplainType.Generator);
        }

        _curInvenPoint = invenPoint;
        generatorExplain.ON(invenPoint, image, trigger, explain, GetRateColor(evaluation), name, enforceInfo);
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