using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponExplain : MonoBehaviour
{
    private Image _image;
    private TextMeshProUGUI _weaponName;
    private TextMeshProUGUI _power;
    private TextMeshProUGUI _explain;
    private TextMeshProUGUI _skillList;
    private TextMeshProUGUI _evaluation;
    private Tooltip _toolTip;

    private void Awake()
    {
        _image = GameObject.Find("Image").GetComponent<Image>();
        _weaponName = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        _power = GameObject.Find("WeaponPower").GetComponent<TextMeshProUGUI>();
        _explain = GameObject.Find("WeaponExplain").GetComponent<TextMeshProUGUI>();
        _skillList = GameObject.Find("WeaponSkillExplain").GetComponent<TextMeshProUGUI>();
        _evaluation = GameObject.Find("WeaponEvaluation").GetComponent<TextMeshProUGUI>();
        _toolTip = GetComponent<Tooltip>();
    
    }

    private void Start()
    {
        Active(false);
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        _toolTip.Init();
    }
    //스킬리스트 페어로 바꾸기
    //등급도 표시
    public void ON(Vector2 pos, Sprite image, string name, float power, string explain, string[] skillList, string evaluation)
    {
        _image.sprite = image;
        _weaponName.text = $"{name}";
        _power.text = $"공격력: {power}";
        _explain.text = explain;
        _evaluation.text = evaluation;

        if (skillList != null)
        {
            _skillList.text = "";
            for (int i = 0; i < skillList.Length; i++)
            {
                _skillList.text += skillList[i] + '\n';
            }
        }

        _toolTip.On(pos);
    }
}