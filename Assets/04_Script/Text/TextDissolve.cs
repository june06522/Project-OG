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

public class TextDissolve : MonoBehaviour
{
    TextMeshProUGUI m_text;
    [SerializeField]
    Material textMat;

    Material mat;

    [SerializeField] TextType m_type;
    [SerializeField] Ease ease;
    [SerializeField] float duration;

    public TextType Type { get { return m_type; } }

    private readonly string shader = "_FullAlphaDissolveFade";
    private void Awake()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.material = Instantiate(m_text.material);
        mat = m_text.materialForRendering;
    }

    public void Init()
    {
        mat.SetFloat(shader, 0);
    }

    public Tween Dissolve(bool value)
    {
        //float width = m_text.preferredWidth;
        //Debug.Log(width);
        //float leftPos = m_text.rectTransform.anchoredPosition.x;
        //float rightPos = m_text.rectTransform.anchoredPosition.x + width;

        float startPos = 0f;
        float endPos = 1f;

        return DOTween.To(() => startPos, 
                  (value) => mat.SetFloat(shader, value),
                  endPos, 
                  duration);
    }
}
