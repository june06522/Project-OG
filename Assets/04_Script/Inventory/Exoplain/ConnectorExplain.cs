using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectorExplain : MonoBehaviour
{
    private Image _iamge;
    private TextMeshProUGUI _evaluation;
    private TextMeshProUGUI _name;

    private void Awake()
    {
        Transform rootTrm = transform.Find("TooltipUp");
        _iamge = rootTrm.Find("ImagePanel/Image").GetComponent<Image>();
        _evaluation = rootTrm.Find("Evaluation").GetComponent<TextMeshProUGUI>();
        _name = rootTrm.Find("Name").GetComponent<TextMeshProUGUI>();
    }   

    public void ON(Vector2 invenPos, Sprite image, RateColor evaluation)
    {
        _iamge.sprite = image;
        _evaluation.text = evaluation.Rate.ToString();
        _evaluation.color = evaluation.color;
        _name.text = "연결기";
    }
}
