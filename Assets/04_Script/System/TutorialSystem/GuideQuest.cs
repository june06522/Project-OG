using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuideQuest : MonoBehaviour
{
    public event Action OnQuestComplete;

    protected TutorialManager _tutorialManager;
    protected GuideUI _tutorialUI;

    protected virtual void Awake()
    {
        _tutorialManager = TutorialManager.Instance;

        if (_tutorialManager != null)
            _tutorialUI = _tutorialManager.TutorialUI;
    }

    public virtual void SetQuestSetting()
    {
        
    }

    public abstract bool IsQuestComplete();

    public virtual void QuestComplete()
    {
        OnQuestComplete?.Invoke();
    }
}
