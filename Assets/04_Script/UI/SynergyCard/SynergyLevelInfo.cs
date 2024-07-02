using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SynergyLevelInfo : MonoBehaviour
{
    private TextMeshProUGUI _levelTxt;
    private readonly string shader = "_OuterOutlineFade";

    Material material;
    DissolveParameters dissolveParameters;

    public int Level;
    private bool _on;

    private void Awake()
    {
        _levelTxt = transform.Find("Level").GetComponent<TextMeshProUGUI>();

        Image image = GetComponent<Image>();
        image.material = Instantiate(image.material);
        material = image.material;
        
        material.SetFloat(shader, 0f);
    }

    private void Start()
    {  
        dissolveParameters = new DissolveParameters(material, 0f, 1f, 0.5f, DG.Tweening.Ease.Linear, shader);
        _on = false;
    }

    public void Init(int level)
    {
        Level = level;
        _levelTxt.text = level.ToString();
    }

    public void On()
    {
        if(!_on)
        {
            dissolveParameters.On();
            _on = true;
        }
    }

    public void Off()
    {
        if(_on)
        {
            dissolveParameters.Off();
            _on = false;
        }
    }
}
