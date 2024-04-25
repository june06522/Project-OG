using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private CinemachineVirtualCamera cam;

    private CinemachineBasicMultiChannelPerlin perlin;

    [SerializeField]
    private Camera _minimapCamera;

    [SerializeField]
    private Shockwave _shockwave;
    [SerializeField]
    private Volume _damageVolume;
    Coroutine _damageVolumeCoroutine;
    float _currentVolumeEndValue = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        cam = GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>();
        perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shockwave(Vector2 pos, float strength, float endValue, float time, bool forceShockwave = false)
    {
        if (_shockwave == null)
            return;

        if (forceShockwave || _shockwave.IsPlay == false)
        {
            _shockwave.transform.position = pos;
            _shockwave.PlayShockwave(strength, endValue, time);
        }
    }
    public void DamageVolume(float endValue, float time)
    {
        if (_damageVolumeCoroutine != null && _damageVolume.weight > endValue)
        {
            return;
        }

        if (_damageVolumeCoroutine != null)
        {
            StopCoroutine(_damageVolumeCoroutine);
        }
        _damageVolumeCoroutine = StartCoroutine(DamageVolumeCoroutine(endValue, time));
    }

    private IEnumerator DamageVolumeCoroutine(float endValue, float time)
    {
        _damageVolume.weight = endValue;
        yield return new WaitForSeconds(time);
        _damageVolume.weight = 0f;
    }

    public void CameraShake(float shakeIntensity, float shakeTime)
    {
        StartCoroutine(CameraShakeCo(shakeIntensity, shakeTime));
    }
    public void StopCameraShake()
    {
        StopAllCoroutines();
        perlin.m_AmplitudeGain = 0; // 노이즈의 진폭
        perlin.m_FrequencyGain = 0; // 노이즈의 주파수
    }

    private IEnumerator CameraShakeCo(float shakeIntensity, float shakeTime)
    {
        perlin.m_AmplitudeGain += shakeIntensity; // 노이즈의 진폭
        perlin.m_FrequencyGain += shakeIntensity; // 노이즈의 주파수

        yield return new WaitForSeconds(shakeTime);

        perlin.m_AmplitudeGain = Mathf.Clamp(perlin.m_AmplitudeGain - shakeIntensity, 0, 100);
        perlin.m_FrequencyGain = Mathf.Clamp(perlin.m_FrequencyGain - shakeIntensity, 0, 100);
    }

    public void SetMinimapCameraPostion(Vector3 worldPos)
    {
        worldPos.z = -10;
        if(_minimapCamera != null)
        {

            _minimapCamera.transform.position = worldPos;

        }
    }
}
