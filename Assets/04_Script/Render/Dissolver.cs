using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct DissolveParameters
{
    public Material Mat;
    public float StartPos;
    public float EndPos;
    public float Duration;
    public Ease Ease;
    public string Shader;

    public DissolveParameters(Material mat, float startPos, float endPos, float duration, Ease ease, string shader)
    {
        Mat = mat;
        StartPos = startPos;
        EndPos = endPos;
        Duration = duration;
        Shader = shader;
        Ease = ease;

        //Init
        mat.SetFloat(shader, StartPos);
    }
}

public class Dissolver
{
    public static Tween Dissolve(DissolveParameters parameters, bool on)
    {
        if (parameters.Mat == null)
            return null;

        float startPos = on ? parameters.StartPos : parameters.EndPos;
        float endPos = on ? parameters.EndPos : parameters.StartPos;
        float Duration = parameters.Duration;
        Ease ease = parameters.Ease;    
        
        return DOTween.To(
                  getter : () => startPos,
                  setter : (value) => parameters.Mat.SetFloat(parameters.Shader, value),
                  endValue : endPos,
                  duration : Duration)
            .SetEase(ease);
    }
}
