using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMSound : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.value = DataManager.Instance.soundData.BGMSoundVal;
    }
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance._BGMSlider = slider;
            slider.onValueChanged.AddListener(SoundManager.Instance.BGSoundVolume);
        }
    }
}
