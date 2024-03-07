using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    public CinemachineVirtualCamera cam;

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator CameraShake(float shakeIntensity, float shakeTime)
    {
        CinemachineBasicMultiChannelPerlin perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = shakeIntensity; // 노이즈의 진폭
        perlin.m_FrequencyGain = shakeIntensity; // 노이즈의 주파수

        yield return new WaitForSeconds(shakeTime);

        perlin.m_AmplitudeGain = 0;
        perlin.m_FrequencyGain = 0;
    }
}
