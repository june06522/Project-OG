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
    private TextMeshProUGUI _evaluation;
    private TextMeshProUGUI _name;
    private Tooltip _toolTip;
    private TextMeshProUGUI _connectEnforceInfo;

    private void Awake()
    {
        Transform rootTrm = transform.Find("TooltipUp");
        _iamge = rootTrm.Find("ImagePanel/Image").GetComponent<Image>();
        _name = rootTrm.Find("Name").GetComponent<TextMeshProUGUI>();
        _evaluation = rootTrm.Find("Evaluation").GetComponent<TextMeshProUGUI>();

        Transform middleTrm = transform.Find("TooltipMiddle");
        _trigger = middleTrm.Find("Trigger").GetComponent<TextMeshProUGUI>();
        _skillExplain = middleTrm.Find("Explain").GetComponent<TextMeshProUGUI>();
        
        Transform underTrm = transform.Find("TooltipUnder");
        _connectEnforceInfo = underTrm.Find("EnforceInfo").GetComponent<TextMeshProUGUI>();
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        //_toolTip.Init();
    }
    //스킬 이름
    //등급도 표시
    public void ON(Vector2 invenPos, Sprite image, string trigger, string skill, RateColor evaluation, string name, string enforceInfo)
    {
        _iamge.sprite = image;
        _trigger.text = $"발동조건 : {trigger}";
        _skillExplain.text = skill;
        _name.text = name;
        _evaluation.text = evaluation.Rate.ToString();
        _evaluation.color = evaluation.color;
        _connectEnforceInfo.text = enforceInfo;

        //_toolTip.CurrentItemRate = evaluation;
        //_toolTip.On(invenPos);
    }
}
