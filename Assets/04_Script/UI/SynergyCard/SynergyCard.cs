using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyCard : MonoBehaviour
{
    private SynergyCardSO _synergyCardSO;
    public SynergyCardSO SynergyCardSO => _synergyCardSO;

    private Image _image;
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _description;
    private Transform _levelTrm;

    public TriggerID GetID => _synergyCardSO.TriggerID;

    WeaponInventory _weaponInventory;
    SynergyInfo _synergyInfo;

    SynergyLevelInfo[] _synergyLevelInfos;
    List<InvenBrick> _generatorBricks;

    Material mat;
    DissolveParameters _synergyParameters;
    private readonly string shader = "_FullAlphaDissolveFade";

    public bool IsFocus;
    public int CurLevel;

    private void Awake()
    {

        Transform textTrm = transform.Find("Text");
        _name = textTrm.Find("Name").GetComponent<TextMeshProUGUI>();
        _description = textTrm.Find("Description").GetComponent<TextMeshProUGUI>();
        _levelTrm = transform.Find("Level");

        _image = GetComponent<Image>();
        _image.material = Instantiate(_image.material);
        mat = _image.material;

        _synergyParameters = new DissolveParameters(mat, 0f, 1f, 0.4f, Ease.Linear, shader);
        _weaponInventory = FindObjectOfType<WeaponInventory>();
        _generatorBricks = new();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(FocusOn);
    }

    public void Update()
    {
        //LevelUpdate
        CheckUpdateInfo();
    }

    private void CheckUpdateInfo()
    {
        int synergyLevel = SynergyManager.Instance.GetSynergyLevel(GetID);
        if (synergyLevel != CurLevel)
        {
            float percent = SynergyManager.Instance.GetStatFactor(GetID) * 100f;
            CurLevel = synergyLevel;
            UpdateInfo(synergyLevel, percent);
        }
    }

    public void Init(SynergyInfo synergyInfo, SynergyCardSO cardSO)
    {

        this._synergyInfo = synergyInfo;
        this._synergyCardSO = cardSO;
        gameObject.name = cardSO.Name;

        _name.text = cardSO.GetName(0);
        _description.text = cardSO.GetDescription(0);
  
        CurLevel = SynergyManager.Instance.GetSynergyLevel(GetID);
        //SynergeLevel 정보 초기화.
        List<int> levelTable = SynergyManager.Instance.GetLevelTable(GetID);
        int levelCount = levelTable.Count;

        _synergyLevelInfos = new SynergyLevelInfo[levelCount];
        Debug.Log($"{GetID} : {levelCount}");
        for(int i = 0; i < _synergyLevelInfos.Count(); i++)
        {
            SynergyLevelInfo temp = Instantiate(synergyInfo.GetSynergeLevelInfoPrefab, _levelTrm);
            temp.Init((int)levelTable[i]);
            _synergyLevelInfos[i] = temp;
        }

    }

    public void UpdateInfo(int level, float percent)
    {

        Setting(level, percent);
        for(int i = 0; i < _synergyLevelInfos.Count(); i++)
        {
            if(_synergyLevelInfos[i].Level <= level)
            {
                _synergyLevelInfos[i].On();
            }
            else
            {
                _synergyLevelInfos[i].Off();
            }
        }

    }

    public void Setting(int level, float percent)
    {
        _name.text = _synergyCardSO.GetName(level);
        _description.text = _synergyCardSO.GetDescription(percent);
    }

    // card가 눌렸을 때 실행하는 이벤트
    public void FocusOn()
    {
        //if (IsFocus == true) return; // 중복 입력 방지
        IsFocus = true;

        _synergyInfo.SetCurrentSynergyCard(this);
       
        // 같은 트리거를 가진 오브젝트만 가져옴
        _generatorBricks.Clear();
        _generatorBricks = (from invenBrickData in _weaponInventory.GetContainer()
                                           where GetID == GetTrigger(invenBrickData.generatorID)
                                           select invenBrickData.invenBrick).ToList();

        _generatorBricks.ForEach((brick) =>
        {
            brick.FocusOn();
        });

        _synergyInfo.SetTriggerText(GetID);
    }

    public void FocusOff()
    {
        IsFocus = false;
        _generatorBricks.ForEach((brick) =>
        {
            brick.FocusOff();
        });
    }

    public void On()
    {
        Color color = _image.color;
        color.a = 1;
        _image.color = color;

        _name.enabled = true;
        _description.enabled = true;
        _levelTrm.gameObject.SetActive(true);
        Dissolver.Dissolve(_synergyParameters, true);
        //_levelTrm.gameObject.SetActive(true);
    }

    public void Off()
    {
        Color color = _image.color;
        color.a = 0;
        _image.color = color;

        _name.enabled = false;
        _description.enabled = false;
        _levelTrm.gameObject.SetActive(false);
        Dissolver.Dissolve(_synergyParameters, false); 
        //_levelTrm.gameObject.SetActive(false);
    }

    private TriggerID GetTrigger(in GeneratorID generatorID)
    {
        return WeaponExplainManager.triggerExplain[generatorID];
    }
}
