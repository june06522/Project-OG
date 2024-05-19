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

    private void Awake()
    {
        _iamge = transform.Find("Image").GetComponent<Image>();
        _trigger = transform.Find("Trigger").GetComponent<TextMeshProUGUI>();
        _skillExplain = transform.Find("Explain").GetComponent<TextMeshProUGUI>();
        _evaluation = transform.Find("Evaluation").GetComponent<TextMeshProUGUI>();
        _name = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        _toolTip = GetComponent<Tooltip>();
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        _toolTip.Init();
    }
    //스킬 이름
    //등급도 표시
    public void ON(Vector2 invenPos, Sprite image, string trigger, string skill, ItemRate evaluation, string name)
    {
        _iamge.sprite = image;
        _trigger.text = $"{trigger}";
        _skillExplain.text = skill;
        _evaluation.text = evaluation.ToString();
        _name.text = name;

        _toolTip.CurrentItemRate = evaluation;
        _toolTip.On(invenPos);
    }
}
