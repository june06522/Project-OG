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
    [Header("Audio Info")]
    public AudioMixer Mixer;
    public AudioSource BgSound;
    public AudioClip DefaultBGM;

    public static SoundManager Instance { get; private set; }

    [Header("Sound UI Slider")]
    public Slider MainSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    private Dictionary<StageType, AudioClip> _stageBGMSoundContainer = new Dictionary<StageType, AudioClip>();

    [Header("Stage BGMSound Clip")]
    public AudioClip StartStageBGMClip;
    public AudioClip EnemyStageBGMClip;
    public AudioClip BossStageBGMClip;

    public AudioClip EventStageBGMClip;
    public AudioClip ShopStageBGMClip;

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

        // Set StageBGM
        _stageBGMSoundContainer.Add(StageType.Start,        StartStageBGMClip);
        _stageBGMSoundContainer.Add(StageType.EnemyStage,   EnemyStageBGMClip);
        _stageBGMSoundContainer.Add(StageType.BossStage,    BossStageBGMClip);
        _stageBGMSoundContainer.Add(StageType.EventStage,   EventStageBGMClip);
        _stageBGMSoundContainer.Add(StageType.Shop,         ShopStageBGMClip);

    }

    public void BGMPlay()
    {
        BgSoundPlay(DefaultBGM);
    }

    public void BGMPlay(StageType stageType)
    {
        if (_stageBGMSoundContainer[stageType] == null)
        {
            Debug.LogError("SoundManager's value, StageSound is null");
            return;
        }

        BgSoundPlay(_stageBGMSoundContainer[stageType], 0.6f);
    }

    private void OnSceneLoded(Scene arg0, LoadSceneMode arg1)
    {
        BGMPlay();
    }

    private void InitVolumeValue()
    {
        Mixer.SetFloat("Master", Mathf.Log10(_data.MasterSoundVal) * 20);
        Mixer.SetFloat("BGSound", Mathf.Log10(_data.BGMSoundVal) * 20);
        Mixer.SetFloat("SFXvolume", Mathf.Log10(_data.SFXSoundVal) * 20);
        //MainSlider.value = _data.MasterSoundVal;
        //BGMSlider.value = _data.BGMSoundVal;
        //SFXSlider.value = _data.SFXSoundVal;
    }

    public void MasterSoundVolume(float val)
    {
        Mixer.SetFloat("Master", Mathf.Log10(val) * 20);
        _data.MasterSoundVal = val;
        DataManager.Instance.soundData = _data;
        DataManager.Instance.SaveOption();
    }

    public void BGSoundVolume(float val)
    {
        Mixer.SetFloat("BGSound", Mathf.Log10(val) * 20);
        _data.BGMSoundVal = val;
        DataManager.Instance.soundData = _data;
        DataManager.Instance.SaveOption();
    }

    public void SFXVolume(float val)
    {
        Mixer.SetFloat("SFXvolume", Mathf.Log10(val) * 20);
        _data.SFXSoundVal = val;
        DataManager.Instance.soundData = _data;
        DataManager.Instance.SaveOption();
    }

    public void SFXPlay(string sfxName, AudioClip clip, float volume = 0.5f)
    {
        if (clip == null)
        {
            Debug.LogWarning($"{sfxName}'s clip is null");
            return;
        }

        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("SFX")[0];
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
        audioSource.outputAudioMixerGroup = Mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BgSoundPlay(AudioClip clip, float volume = 0.5f)
    {
        if (BgSound == null)
            return;
            BgSound.outputAudioMixerGroup = Mixer.FindMatchingGroups("BGSound")[0];

        BgSound.clip = clip;
        BgSound.loop = true;
        BgSound.volume = volume;
        BgSound.Play();
    }

    public void BgStop()
    {

        BgSound.Stop();

    }

    public void PlayExplosion(AudioClip explosionClip)
        => StartCoroutine(PlayExplosionCo(explosionClip));

    public void FadeSound() => StartCoroutine(FadeSoundCo());

    IEnumerator FadeSoundCo()
    {
        while (BgSound.volume > 0.0f)
        {
            BgSound.volume -= 0.01f;
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