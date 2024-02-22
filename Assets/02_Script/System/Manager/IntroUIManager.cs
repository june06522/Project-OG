using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroUIManager : MonoBehaviour
{
    [SerializeField] GameObject _settingPanel;
    [SerializeField] GameObject _controlPanel;
    [SerializeField] GameObject _audioPanel;

    public void StartBtn(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void SettingBtn()
    {
        _settingPanel.SetActive(!_settingPanel.activeSelf);
    }

    public void ControlBtn()
    {
        _controlPanel.SetActive(!_controlPanel.activeSelf);
    }

    public void AudioBtn()
    {
        _audioPanel.SetActive(!_audioPanel.activeSelf);
    }

    public void QuitBtn() => Application.Quit();
}