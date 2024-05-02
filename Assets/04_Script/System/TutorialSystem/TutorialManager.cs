using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialManager : MonoSingleton<TutorialManager>
{
    [Header("Guide UI Info")]
    [SerializeField]
    private GuideUI _guideUI;
    public GuideUI TutorialUI => _guideUI;

    [Header("Guide Line")]
    [SerializeField]
    private List<GuideQuest> _guideLine;
    private int _currentGuideIndex = 0;

    private GuideQuest _currentGuide;
    private bool _tutorialClear = false;

    private void Awake()
    {
        _currentGuide = _guideLine[_currentGuideIndex];
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
                return;
            }

            _currentGuide = _guideLine[++_currentGuideIndex];
            _currentGuide.SetQuestSetting();
        }

    }

    
}
