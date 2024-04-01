using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum SoundType
{
    Master,
    BGM,
    SFX
}

public class SoundManager : MonoBehaviour
{
    public AudioMixer _mixer;
    public AudioSource _bgSound;
    public AudioClip[] _bgList;

    public static SoundManager Instance { get; private set; }

    [SerializeField] Slider _mainSlider;
    [SerializeField] Slider _BGMSlider;
    [SerializeField] Slider _SFXSlider;

    SoundData _data;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //BGMPlay();
            SceneManager.sceneLoaded += OnSceneLoded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _data = DataManager.Instance.soundData;
        InitVolumeValue();
    }

    public void BGMPlay()
    {
        for (int i = 0; i < _bgList.Length; i++)
        {
            if (SceneManager.GetActiveScene().name == _bgList[i].name)
                BgSoundPlay(_bgList[i]);

        }
    }

    private void OnSceneLoded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < _bgList.Length; i++)
        {
            if (arg0.name == _bgList[i].name)
                BgSoundPlay(_bgList[i]);

        }
    }

    private void InitVolumeValue()
    {
        _mixer.SetFloat("Master", Mathf.Log10(_data.MasterSoundVal) * 20);
        _mixer.SetFloat("BGSound", Mathf.Log10(_data.BGMSoundVal) * 20);
        _mixer.SetFloat("SFXvolume", Mathf.Log10(_data.SFXSoundVal) * 20);
        _mainSlider.value = _data.MasterSoundVal;
        _BGMSlider.value = _data.BGMSoundVal;
        _SFXSlider.value = _data.SFXSoundVal;
    }

    public void MasterSoundVolume(float val)
    {
        _mixer.SetFloat("Master", Mathf.Log10(val) * 20);
        _data.MasterSoundVal = val;
        DataManager.Instance.soundData = _data;
        DataManager.Instance.SaveOption();
    }

    public void BGSoundVolume(float val)
    {
        _mixer.SetFloat("BGSound", Mathf.Log10(val) * 20);
        _data.BGMSoundVal = val;
        DataManager.Instance.soundData = _data;
        DataManager.Instance.SaveOption();
    }

    public void SFXVolume(float val)
    {
        _mixer.SetFloat("SFXvolume", Mathf.Log10(val) * 20);
        _data.SFXSoundVal = val;
        DataManager.Instance.soundData = _data;
        DataManager.Instance.SaveOption();
    }

    public void SFXPlay(string sfxName, AudioClip clip, float volume = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void SFXPlay(string sfxName, AudioClip clip, Transform parent, float volume = 0.5f)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        go.transform.parent = parent;
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = _mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip, float volume = 0.5f)
    {
        _bgSound.outputAudioMixerGroup = _mixer.FindMatchingGroups("BGSound")[0];

        _bgSound.clip = clip;
        _bgSound.loop = true;
        _bgSound.volume = volume;
        _bgSound.Play();
    }

    public void BgStop()
    {

        _bgSound.Stop();

    }

    public void PlayExplosion(AudioClip explosionClip)
        => StartCoroutine(PlayExplosionCo(explosionClip));

    public void FadeSound() => StartCoroutine(FadeSoundCo());

    IEnumerator FadeSoundCo()
    {
        while (_bgSound.volume > 0.0f)
        {
            _bgSound.volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator PlayExplosionCo(AudioClip explosionClip)
    {
        float startTime = Time.time;
        float endTime = startTime;

        while (endTime - startTime < 2.4f)
        {
            endTime = Time.time;
            SFXPlay("Explosion", explosionClip);
            yield return new WaitForSeconds(0.15f);
        }
    }
}   