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

    private void Awake()
    {
        _iamge = GameObject.Find("GeneratorImage").GetComponent<Image>();
        _trigger = GameObject.Find("GeneratorTrigger").GetComponent<TextMeshProUGUI>();
        _skillList = GameObject.Find("GeneratorSkillExplain").GetComponent<TextMeshProUGUI>();
    }

    public void ON(Sprite image, string trigger, string[] skillList)
    {
        _iamge.sprite = image;
        _trigger.text = $"트리거: {trigger}";

        if(skillList != null )
        {
            _skillList.text = "";
            for (int i = 1; i <= skillList.Length; i++)
            {
                _skillList.text += $"{i}개 - {skillList[i - 1]}\n";
            }
        }
    }
}
