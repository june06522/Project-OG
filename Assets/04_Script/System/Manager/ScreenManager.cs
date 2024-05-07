using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Utility;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] Material inventoryScreenMat;
    
    public static ScreenManager Instance;

    private readonly string scifiBlit = "ScifiBlit";

    Material tempMat;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<ScreenManager>();
            DontDestroyOnLoad(this);
        }
        else
            Destroy(gameObject);

        tempMat = Instantiate(inventoryScreenMat);

        //ScriptableRendererFeature renderFeature;
        //renderFeature = URPRendererUtility.GetScriptableRendererData()[0]
        //    .rendererFeatures.Find((feature) => String.Equals(feature.name, scifiBlit));

        //Cyan.Blit blit = renderFeature as Cyan.Blit;
        //blit.settings.blitMaterial = tempMat;
    }

    public Tween SetEffect(float power, float time, Ease ease)
    {
        float startPower = tempMat.GetFloat("_Power");
        return DOTween.To(() =>
            startPower,
            value => tempMat.SetFloat("_Power", value),
            power, time)
            .SetEase(ease);
    }
}
