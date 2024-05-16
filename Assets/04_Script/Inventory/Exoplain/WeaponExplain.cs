using System;
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
        _image = transform.Find("Image").GetComponent<Image>();
        _weaponName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _power = transform.Find("Power").GetComponent<TextMeshProUGUI>();
        _explain = transform.Find("Explain").GetComponent<TextMeshProUGUI>();
        _skillList = transform.Find("SkillExplain").GetComponent<TextMeshProUGUI>();
        _evaluation = transform.Find("Evaluation").GetComponent<TextMeshProUGUI>();
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
    public void ON(Vector2 pos, Sprite image, string name, float power, string explain, Tuple<string, int>[] skillList, string evaluation)
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
                _skillList.text += $"{skillList[i].Item1} {skillList[i].Item2}레벨 \n";
            }
        }

        _toolTip.On(pos);
    }
}