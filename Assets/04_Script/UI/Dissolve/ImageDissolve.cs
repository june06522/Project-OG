using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageDissolve : DissolveBase
{
    Image image;

    protected void Awake()
    {
        image = GetComponent<Image>();
        image.material = Instantiate(image.material);
        mat = image.materialForRendering;
    }

    public override void Init(Color color = default)
    {
        mat.SetFloat(shader, startPos);
        mat.SetColor("_HologramTint", color);
    }
}
