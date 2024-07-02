using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
    [SerializeField] Material invenMat;

    [SerializeField] float duration = 0.4f;
    [SerializeField] Ease ease = Ease.Linear;
    [SerializeField] Ease invenEase = Ease.InSine;

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

    private readonly float _synergyMatStart = 0f;
    private readonly float _synergyMatEnd = 5f;

    private readonly float _invenStart = 0f;
    private readonly float _invenEnd = 20f;
    private readonly float _invenEasingTime = 0.5f;

    DissolveParameters _tooltipParameters;
    DissolveParameters _iconShellParameters;
    DissolveParameters _iconShell2Parameters;
    DissolveParameters _iconParameters;
    DissolveParameters _icon2Parameters;
    DissolveParameters _imageParameters;
    DissolveParameters _textParameters;
    DissolveParameters _statTextParameters;
    DissolveParameters _invenBrickParameters;
    DissolveParameters _invenParameters;

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
        _invenParameters = new DissolveParameters(invenMat, _invenStart, _invenEnd, _invenEasingTime, invenEase, sourceGlowFade);
    }

    public Tween InvenOn(bool value)
    {
        return Dissolver.Dissolve(_invenParameters, value);
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


    [MenuItem("MyMenu/Do InvenON with a Shortcut Key #o")]
    static void DisplayOn()
    {

        TooltipDissolve dissolve = FindObjectOfType<TooltipDissolve>(); 

        dissolve.Init();
        dissolve.Display(true);
    }

    [MenuItem("MyMenu/Do InvenOFF with a Shortcut Key #p")]
    static void DisplayOff()
    {
        TooltipDissolve dissolve = FindObjectOfType<TooltipDissolve>();
        dissolve.Init();
        dissolve.Display(false);
    }


    public void Display(bool value)
    {
        if(value)
        {
            _tooltipParameters.On();
            _iconShellParameters.On();
            _iconShell2Parameters.On();
            _iconParameters.On();
            _icon2Parameters.On();
            _imageParameters.On();
            _textParameters.On();
            _statTextParameters.On();
            _invenBrickParameters.On();
            _invenParameters.On();
        }
        else
        {
            _tooltipParameters.Off();
            _iconShellParameters.Off();
            _iconShell2Parameters.Off();
            _iconParameters.Off();
            _icon2Parameters.Off();
            _imageParameters.Off();
            _textParameters.Off();
            _statTextParameters.Off();
            _invenBrickParameters.Off();
            _invenParameters.Off();
        }

    }
}