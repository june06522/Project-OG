using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        pausePanel.SetActive(true);
    }

    public void CloseBtn()
    {
        PlayEFF();
        pausePanel.SetActive(false);
    }

    public void ReGameBtn()
    {
        PlayEFF();
        SceneManager.LoadScene("Play");
    }

    public void MainMenuBtn()
    {
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