using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponExplain : MonoBehaviour
{
    private Image          _iamge;
    private TextMeshProUGUI _weaponName;
    private TextMeshProUGUI _power;
    private TextMeshProUGUI _explain;
    private TextMeshProUGUI _skillList;

    private void Awake()
    {
        _iamge = GameObject.Find("WeaponImage").GetComponent<Image>();
        _weaponName = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();
        _power = GameObject.Find("WeaponPower").GetComponent<TextMeshProUGUI>();
        _explain = GameObject.Find("WeaponExplain").GetComponent<TextMeshProUGUI>();
        _skillList = GameObject.Find("WeaponSkillExplain").GetComponent<TextMeshProUGUI>();
    }

    public void ON(Sprite image, string name, float power, string explain, string[] skillList)
    {
        _iamge.sprite = image;
        _weaponName.text = $"이름: {name}";
        _power.text = $"공격력: {power}";
        _explain.text = explain;

        if(skillList != null)
        {
            _skillList.text = "";
            for(int i = 0; i < skillList.Length; i++)
            {
                _skillList.text += skillList[i] + '\n';
            }
        }
    }
}