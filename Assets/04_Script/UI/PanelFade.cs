using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelFade : MonoBehaviour
{
    Image image;

    float targetAlpha = 0.4f;
    private void Awake()
    {
        image = GetComponent<Image>();   
    }

    public void Fade(bool fadeIn)
    {
        Color target = fadeIn ? new Color(0,0,0, targetAlpha) : new Color(0, 0, 0, 0);
        Color start = image.color;
        image.DOColor(target, 0.5f);
    }


}
