using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponExplain : MonoBehaviour
{
    [SerializeField] List<RateColor> colorList;

    private Image _image;
    private TextMeshProUGUI _name;
    private TextMeshProUGUI _power;
    private TextMeshProUGUI _explain;
    private TextMeshProUGUI[] _skillList;
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
        _skillList = underTrm.Find("SkillExplain").GetComponentsInChildren<TextMeshProUGUI>();
        //_toolTip = GetComponent<Tooltip>();
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        //_toolTip.Init();
    }

    //스킬리스트 페어로 바꾸기
    //등급도 표시
    public void ON(Vector2 pos, Sprite image, string name, float power, string explain, Tuple<GeneratorID, SkillUIInfo>[] skillList, RateColor evaluation)
    {
        _image.sprite = image;
        _name.text = $"{name}";
        _power.text = $"공격력: {power}";
        _explain.text = explain;
        _evaluation.text = evaluation.Rate.ToString();
        _evaluation.color = evaluation.color;

        if (skillList != null)
        {
            skillList.Reverse();
            for (int i = 0; i < skillList.Length; i++)
            {
                if(i < _skillList.Length)
                {
                    _skillList[i].text = $"{WeaponExplainManager.generatorName[skillList[i].Item1]} {skillList[i].Item2.Level}레벨";
                    _skillList[i].color = GetRateColor(skillList[i].Item2.Rate);
                }
                else
                {
                    _skillList[_skillList.Length - 1].text += "\n";
                }
                   
                //if (i != 0 && i % 2 == 0) _skillList.text += "\n";
                //else _skillList.text += "\t";
                //if (i != 0 && i % 2 == 0) _skillList.text += "\n";
                //else _skillList.text += "\t";
            }
        }

        //_toolTip.CurrentItemRate = evaluation;
        //_toolTip.On(pos);
    }

    public Color GetRateColor(ItemRate currentItemRate)
    {
        return colorList.Find((item) => item.Rate == currentItemRate).color;
    }


}