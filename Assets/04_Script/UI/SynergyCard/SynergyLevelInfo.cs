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

    Image image;

    private void Awake()
    {

        _levelTxt = transform.Find("Level").GetComponent<TextMeshProUGUI>();

        image = GetComponent<Image>();
        image.material = Instantiate(image.material);
        material = image.material;
        
    }

    private void Start()
    {  
        dissolveParameters = new DissolveParameters(material, 0f, 1f, 0.5f, DG.Tweening.Ease.Linear, shader);
        material.SetFloat(shader, 1f);
        _on = false;
        
    }

    public void Init(int level)
    {
        Level = level;
        _levelTxt.text = level.ToString();
    }

    public void On()
    {

        image.color = Color.white;
        //dissolveParameters.On();
        _on = true;
    
    }

    public void Off()
    {

        image.color = Color.black;
        //dissolveParameters.Off();
        _on = false;
        
    }
}
