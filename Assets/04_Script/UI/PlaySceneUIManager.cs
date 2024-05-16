using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaySceneUIManager : MonoBehaviour
{
    public static PlaySceneUIManager Instance;

    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject settingPanel;
    [SerializeField] GameObject controlPanel;

    [HideInInspector]
    public bool isOpen = false;

    #region Init
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError($"{transform} : PlaySceneUIManager is Multiply running!");
            Destroy(gameObject);
        }
    }
    #endregion

    #region Input
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isOpen = !isOpen;
            if (isOpen)
                PausePanelOpen();
            else
                CloseBtn();
        }

    }
    #endregion

    private void PausePanelOpen()
    {
        PlayEFF();
        ScreenManager.Instance.SetEffect(0.5f, 0.5f, DG.Tweening.Ease.InQuart);
        pausePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void CloseBtn()
    {
        Time.timeScale = 1.0f;
        PlayEFF();
        ScreenManager.Instance.SetEffect(0, 0.5f, DG.Tweening.Ease.InQuart);
        pausePanel.SetActive(false);
    }

    public void ReGameBtn()
    {
        Time.timeScale = 1.0f;
        PlayEFF();
        SceneManager.LoadScene("Play");
    }

    public void MainMenuBtn()
    {
        Time.timeScale = 1.0f;
        PlayEFF();
        SceneManager.LoadScene("Intro");
    }

    public void ControlBtn()
    {
        PlayEFF();
        controlPanel.SetActive(true);
    }

    public void SettingOpen()
    {
        PlayEFF();
        settingPanel.SetActive(true);
    }

    public void QuitBtn()
    {
        Time.timeScale = 1.0f;
        PlayEFF();
        Application.Quit();
    }

    public void CloseSetting()
    {
        PlayEFF();
        settingPanel.SetActive(false);
    }

    public void CloseControl()
    {
        PlayEFF();
        controlPanel.SetActive(false);
    }

    private void PlayEFF()
    {
        PlaySceneEffectSound.Instance.PlayBtnClickSound();
    }
}