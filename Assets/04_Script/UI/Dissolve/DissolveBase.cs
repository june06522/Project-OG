using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class DissolveBase : MonoBehaviour
{
    [SerializeField] Ease ease;
    [SerializeField] float duration;

    protected float startPos = 0f;
    protected float endPos = 1f;
    
    protected Material mat;

    protected readonly string shader = "_SourceGlowDissolveFade";

    public DissolveParameters Parameters;

    protected virtual void Start()
    {
        Parameters = new DissolveParameters(mat, startPos, endPos, duration, ease, shader);
    }

    public abstract void Init(Color color = default);

    public virtual Tween Dissolve(bool on)
    {
        return Dissolver.Dissolve(Parameters, on);
    }
}
