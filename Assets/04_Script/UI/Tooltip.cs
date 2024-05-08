using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Tooltip : MonoBehaviour
{
    [SerializeField] float duration;
    [SerializeField] Ease ease;

    private List<TextDissolve> titleTexts;
    private List<TextDissolve> mainTexts;
    private ImageDissolve image;

    private readonly string shader = "_DirectionalGlowFadeFade";
    private readonly float fadeEnd = 0;
    private readonly float fadeStart = 1.5f;

    Material mat;

    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        Image img = GetComponent<Image>();
        img.material = Instantiate(img.material);
        mat = img.materialForRendering;
        mat.SetFloat(shader, fadeStart); 
        

        image = transform.Find("DissolveImage").GetComponent<ImageDissolve>();
        TextDissolve[] texts = GetComponentsInChildren<TextDissolve>();
        
        titleTexts = (from text in texts
                     where text.Type == TextType.TITLE
                     select text).ToList();

        mainTexts = (from text in texts
                      where text.Type == TextType.MAIN
                      select text).ToList();

        Init();
    }

    public void Active(bool value)
    {
        this.gameObject.SetActive(value);

        Init();
    }

    private void Init()
    {
        mat.SetFloat(shader, fadeStart);
        titleTexts.ForEach((text) => text.Init());
        mainTexts.ForEach((text) => text.Init());
        image.Init();
    }

    private bool gang = false;
    public void On(Vector2 pos, Sprite _image, string trigger, string skill)
    {
        //if (gang) return;
        //gang = true;
        rectTransform.parent.position = pos;

        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => fadeStart,
                  (value) => mat.SetFloat(shader, value), 
                  fadeEnd,
                  duration));
        
        seq.Append(image.Dissolve());
        for(int i = 0; i < titleTexts.Count; ++i)
        {
            seq.Join(titleTexts[i].Dissolve(true));
        }

        //for (int i = 0; i < mainTexts.Count; ++i)
        //{
        //    seq.Insert(1f, mainTexts[i].Dissolve(true));
        //}

        //_iamge.sprite = image;
        //_trigger.text = $"트리거: {trigger}";

        //if (skill != null)
        //{
        //    _skillList.text = skill;
        //    //for (int i = 1; i <= skillList.Length; i++)
        //    //{
        //    //    _skillList.text += $"{i}개 - {skillList[i - 1]}\n";
        //    //}
        //}
    }
}
