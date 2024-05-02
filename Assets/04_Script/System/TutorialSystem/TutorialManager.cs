using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [Header("Guide UI Info")]
    [SerializeField]
    private GuideUI _guideUI;
    public GuideUI TutorialUI => _guideUI;

    [Header("Guide Line")]
    [SerializeField]
    private Transform _guideRoot;
    private List<GuideQuest> _guideLine;
    private int _currentGuideIndex = 0;

    private GuideQuest _currentGuide;
    private bool _tutorialClear = false;

    private void Awake()
    {
        if (_guideRoot == null)
        {
            Debug.LogError("TutorialManager's GuideRoot is null");
            return;
        }

        _guideLine = _guideRoot.GetComponentsInChildren<GuideQuest>().ToList<GuideQuest>();

        if (_guideLine.Count <= 0)
        {
            Debug.LogError("GuideRoot does not have Quests");
            return;
        }

        _currentGuide = _guideLine[_currentGuideIndex];
        _currentGuide?.SetQuestSetting();
    }

    //Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _currentGuide.QuestComplete();
            _currentGuide = _guideLine[++_currentGuideIndex];
            _currentGuide.SetQuestSetting();
        }

    }

    private void FixedUpdate()
    {

        if (_tutorialClear)
            return;

        Debug.Log($"Run Quest {_currentGuide.name}");
        if (_currentGuide.IsQuestComplete())
        {
            _currentGuide.QuestComplete();
            Debug.Log($"Complete Quest {_currentGuide.name}");

            if (_currentGuideIndex == _guideLine.Count - 1)
            {
                _tutorialClear = true;
                AsyncSceneLoader.LoadScene("Play");
                return;
            }

            _currentGuide = _guideLine[++_currentGuideIndex];
            _currentGuide.SetQuestSetting();
        }

    }


}
