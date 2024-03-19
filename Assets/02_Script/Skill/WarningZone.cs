using DG.Tweening;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class WarningZone : MonoBehaviour
{
    Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        material.SetFloat("_Size", 0);
    }

    public void Marking(float time)
    {
        float t = 0;
        DOTween.To(() => t, cur => material.SetFloat("_Size", cur), 1, time).SetEase(Ease.InSine);
    }
}
