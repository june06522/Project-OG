using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipDissolve : MonoBehaviour
{
    [SerializeField] Material tooltipMat;
    [SerializeField] Material iconShellMat;
    [SerializeField] Material iconShellMat2;
    [SerializeField] Material iconMat;
    [SerializeField] Material iconMat2;
    [SerializeField] Material imageMat;
    [SerializeField] Material textMat;
    [SerializeField] Material statTextMat;
    [SerializeField] Material invenBrickMat;

    [SerializeField] float duration = 0.4f;
    [SerializeField] Ease ease = Ease.Linear;


    private readonly string sourceGlowFade = "_SourceGlowDissolveFade";
    private readonly string directionalGlowFade = "_DirectionalGlowFadeFade";
    private readonly string alphaFade = "_FullAlphaDissolveFade";

    private readonly float _tooltipDissolveStart = 1;
    private readonly float _tooltipDissolveEnd = 0f;

    private readonly float _iconShellDissolveStart = 1.6f;
    private readonly float _iconShellDissolveEnd = -0.2f;

    private readonly float _iconDissolveStart = 1.6f;
    private readonly float _iconDissolveEnd = -0.2f;

    private readonly float _imageDissolveStart = 0;
    private readonly float _imageDissolveEnd = 1f;

    private readonly float _textDissolveStart = 0f;
    private readonly float _textDissolveEnd = 1f;

    private readonly float _invenBrickStart = 0f;
    private readonly float _invenBrickEnd = 1f;

    DissolveParameters _tooltipParameters;
    DissolveParameters _iconShellParameters;
    DissolveParameters _iconShell2Parameters;
    DissolveParameters _iconParameters;
    DissolveParameters _icon2Parameters;
    DissolveParameters _imageParameters;
    DissolveParameters _textParameters;
    DissolveParameters _statTextParameters;
    DissolveParameters _invenBrickParameters;

    Sequence seq;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _tooltipParameters = new DissolveParameters(tooltipMat, _tooltipDissolveStart, _tooltipDissolveEnd, duration, ease, directionalGlowFade);
        _iconShellParameters = new DissolveParameters(iconShellMat, _iconShellDissolveStart, _iconShellDissolveEnd, duration, ease, sourceGlowFade);
        _iconShell2Parameters = new DissolveParameters(iconShellMat2, _iconShellDissolveStart, _iconShellDissolveEnd, duration, ease, sourceGlowFade);
        _iconParameters = new DissolveParameters(iconMat, _iconDissolveStart, _iconDissolveEnd, duration, ease, sourceGlowFade);
        _icon2Parameters = new DissolveParameters(iconMat2, _iconDissolveStart, _iconDissolveEnd, duration, ease, sourceGlowFade);
        _imageParameters = new DissolveParameters(imageMat, _imageDissolveStart, _imageDissolveEnd, duration, ease, sourceGlowFade);
        _textParameters = new DissolveParameters(textMat, _textDissolveStart, _textDissolveEnd, duration, ease, alphaFade);
        _statTextParameters = new DissolveParameters(statTextMat, _textDissolveStart, _textDissolveEnd, duration, ease, alphaFade);
        _invenBrickParameters = new DissolveParameters(invenBrickMat, _invenBrickStart, _invenBrickEnd, 0.25f, ease, alphaFade);
    
    }

    public Tween BrickOn(bool value)
    {
        return seq.Join(Dissolver.Dissolve(_invenBrickParameters, value));
    }

    public Tween ImageOn(bool value)
    {
        return Dissolver.Dissolve(_imageParameters, value);
    }

    public Sequence On()
    {
        seq.Kill();
        seq = DOTween.Sequence();

        seq.Append(Dissolver.Dissolve(_tooltipParameters, true));
        seq.Join(Dissolver.Dissolve(_iconShellParameters, true));
        seq.Join(Dissolver.Dissolve(_iconShell2Parameters, true));
        seq.Join(Dissolver.Dissolve(_iconParameters, true));
        seq.Join(Dissolver.Dissolve(_icon2Parameters, true));
        seq.Join(Dissolver.Dissolve(_textParameters, true));
        seq.Join(Dissolver.Dissolve(_statTextParameters, true));
        seq.Append(ImageOn(true));
        return seq;  
    }

    public Sequence Off()
    {
        seq.Kill();
        seq = DOTween.Sequence();
        
        seq.Append(Dissolver.Dissolve(_tooltipParameters, false));
        seq.Join(Dissolver.Dissolve(_iconShellParameters, false));
        seq.Join(Dissolver.Dissolve(_iconShell2Parameters, false));
        seq.Join(Dissolver.Dissolve(_iconParameters, false));
        seq.Join(Dissolver.Dissolve(_icon2Parameters, false));
        seq.Join(Dissolver.Dissolve(_textParameters, false));
        seq.Join(Dissolver.Dissolve(_statTextParameters, false));
        seq.Join(ImageOn(false));
        return seq;
    }
}
