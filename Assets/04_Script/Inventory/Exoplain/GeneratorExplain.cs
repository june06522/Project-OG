using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorExplain : MonoBehaviour
{
    private Image _iamge;
    private TextMeshProUGUI _trigger;
    private TextMeshProUGUI _skillExplain;
    private Tooltip _toolTip;

    private void Awake()
    {
        _iamge = GameObject.Find("Image").GetComponent<Image>();
        _trigger = GameObject.Find("GeneratorTrigger").GetComponent<TextMeshProUGUI>();
        _skillExplain = GameObject.Find("GeneratorExplain").GetComponent<TextMeshProUGUI>();
        _toolTip = GetComponent<Tooltip>();
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        _toolTip.Init();
    }
    //스킬 이름
    //등급도 표시
    public void ON(Vector2 invenPos, Sprite image, string trigger, string skill)
    {
        _iamge.sprite = image;
        _trigger.text = $"{trigger}";
        _skillExplain.text = skill;

        _toolTip.On(invenPos);
    }
}
