using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponExplain : MonoBehaviour
{
    private Image _image;
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _power;
    private TextMeshProUGUI _explain;
    private TextMeshProUGUI _skillList;
    private TextMeshProUGUI _evaluation;
    //private Tooltip _toolTip;

    private void Awake()
    {
        Transform rootTrm = transform.Find("TooltipUp");
        _image = rootTrm.Find("ImagePanel/Image").GetComponent<Image>();
        _evaluation = rootTrm.Find("Evaluation").GetComponent<TextMeshProUGUI>();
        _name = rootTrm.Find("Name").GetComponent<TextMeshProUGUI>();
        _power = rootTrm.Find("Power").GetComponent<TextMeshProUGUI>();
        
        Transform middleTrm = transform.Find("TooltipMiddle");
        _explain = middleTrm.Find("Explain").GetComponent<TextMeshProUGUI>();
        
        Transform underTrm = transform.Find("TooltipUnder");
        _skillList = underTrm.Find("SkillExplain").GetComponent<TextMeshProUGUI>();
        //_toolTip = GetComponent<Tooltip>();
    }

    private void Start()
    {
        Active(false);
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        //_toolTip.Init();
    }
    //스킬리스트 페어로 바꾸기
    //등급도 표시
    public void ON(Vector2 pos, Sprite image, string name, float power, string explain, Tuple<GeneratorID, int>[] skillList, ItemRate evaluation)
    {
        _image.sprite = image;
        _name.text = $"{name}";
        _power.text = $"공격력: {power}";
        _explain.text = explain;
        _evaluation.text = evaluation.ToString();

        if (skillList != null)
        {
            _skillList.text = "";
            for (int i = 0; i < skillList.Length; i++)
            {
                _skillList.text += $"{WeaponExplainManager.generatorName[skillList[i].Item1]} {skillList[i].Item2}레벨\n";
                //if (i != 0 && i % 2 == 0) _skillList.text += "\n";
                //else _skillList.text += "\t";
                //if (i != 0 && i % 2 == 0) _skillList.text += "\n";
                //else _skillList.text += "\t";
            }
        }

        //_toolTip.CurrentItemRate = evaluation;
        //_toolTip.On(pos);
    }
}