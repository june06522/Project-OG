using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public enum TextType
{
    TITLE,
    MAIN
}

public class TextDissolve : DissolveBase
{

    [SerializeField] TextType m_type;
    public TextType Type { get { return m_type; } }

    private new readonly string shader = "_FullAlphaDissolveFade";

    TextMeshProUGUI m_text;
    
    public override void Init(Color color = default)
    {
        mat.SetFloat(shader, 0);
    }
}
