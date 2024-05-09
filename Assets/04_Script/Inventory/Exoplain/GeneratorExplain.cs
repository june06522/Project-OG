using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorExplain : MonoBehaviour
{
    private Image _iamge;
    private TextMeshProUGUI _trigger;
    private TextMeshProUGUI _skillList;
    private TextMeshProUGUI _skillExplain;
    private Tooltip _toolTip;

    private void Awake()
    {
        _iamge = GameObject.Find("Image").GetComponent<Image>();
        _trigger = GameObject.Find("GeneratorTrigger").GetComponent<TextMeshProUGUI>();
        _skillExplain = GameObject.Find("GeneratorExplain").GetComponent<TextMeshProUGUI>();
        //_skillList = GameObject.Find("GeneratorSkillExplain").GetComponent<TextMeshProUGUI>();
        _toolTip = GetComponent<Tooltip>();
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        _toolTip.Init();
    }

    public void ON(Vector2 invenPos, Sprite image, string trigger, string skill)
    {
        _iamge.sprite = image;
        _trigger.text = $"{trigger}";
        _skillExplain.text = skill;

        _toolTip.On(invenPos);
        //if (skill != null )
        //{
        //    _skillList.text = skill;
        //    //for (int i = 1; i <= skillList.Length; i++)
        //    //{
        //    //    _skillList.text += $"{i}개 - {skillList[i - 1]}\n";
        //    //}
        //}
    }
}
