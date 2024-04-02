using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXSound : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.value = DataManager.Instance.soundData.SFXSoundVal;
    }
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SFXSlider = slider;
            slider.onValueChanged.AddListener(SoundManager.Instance.SFXVolume);
        }
    }
}
