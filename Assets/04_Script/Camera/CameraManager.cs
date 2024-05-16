using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;

public class CameraManager : MonoSingleton<CameraManager>
{
    private CinemachineBrain _brain;

    private CinemachineVirtualCamera cam;
    private CinemachineVirtualCamera _defaultCam;

    private CinemachineBasicMultiChannelPerlin perlin;
    private CinemachineBasicMultiChannelPerlin _defaultPerlin;

    [SerializeField]
    private Camera _minimapCamera;

    [SerializeField]
    private Shockwave _shockwave;
    [SerializeField]
    private Volume _damageVolume;
    [SerializeField]
    private Volume _playerHitDamageVolume;

    Coroutine _damageVolumeCoroutine;
    Coroutine _playerDamageVolumeCoroutine;

    private void Awake()
    {
        _defaultCam = cam = GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>();
        _defaultPerlin = perlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        _brain = Camera.main.transform.GetComponent<CinemachineBrain>();
    }

    public void SetOtherCam(CinemachineVirtualCamera changeCam, bool forceSet = false)
    {
        if (forceSet)
        {
            _brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Cut;
        }
        else
        {
            _brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.Linear;
        }

        cam.Priority = 0;
        changeCam.Priority = 10;
        cam = changeCam;

        CinemachineBasicMultiChannelPerlin tempPerlin = changeCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if(tempPerlin != null)
            perlin = tempPerlin;
    }
    public void SetDefaultCam()
    {
        cam.Priority = 0;
        _defaultCam.Priority = 10;

        cam = _defaultCam;
        perlin = _defaultPerlin;
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
        _damageVolumeCoroutine = StartCoroutine(DamageVolumeCoroutine(_damageVolume, endValue, time));
    }
    public void PlayerDamageVolume(float endValue, float time)
    {
        if (_playerDamageVolumeCoroutine != null)
        {
            StopCoroutine(_playerDamageVolumeCoroutine);
        }
        _playerDamageVolumeCoroutine = StartCoroutine(DamageVolumeCoroutine(_playerHitDamageVolume, endValue, time));
    }

    private IEnumerator DamageVolumeCoroutine(Volume volume, float endValue, float time)
    {
        volume.weight = endValue;
        yield return new WaitForSeconds(time);
        volume.weight = 0f;
        _playerDamageVolumeCoroutine = null;
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
