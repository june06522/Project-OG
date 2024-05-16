using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ImageDissolve : MonoBehaviour
{
    Image image;
    Material mat;
    
    [SerializeField] Ease ease;
    [SerializeField] float duration;

    private readonly string shader = "_SourceGlowDissolveFade";

    float startPos = 0f;
    float endPos = 1f;

    Color color;
    private void Awake()
    {
        image = GetComponent<Image>();
        image.material = Instantiate(image.material);
        mat = image.materialForRendering;
    }

    public void Init(Color color)
    {
        mat.SetFloat(shader, startPos);
        mat.SetColor("_HologramTint", color);
    }

    public Tween Dissolve()
    {
        return DOTween.To(() => startPos,
                  (value) => mat.SetFloat(shader, value),
                  endPos,
                  duration);
    }
}
