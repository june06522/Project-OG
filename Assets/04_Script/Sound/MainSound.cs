using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSound : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.value = DataManager.Instance.soundData.MasterSoundVal;
    }
    private void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.MainSlider = slider;
            slider.onValueChanged.AddListener(SoundManager.Instance.MasterSoundVolume);
        }
    }
}
