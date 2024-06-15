using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuideQuest : MonoBehaviour
{
    public event Action OnQuestComplete;

    protected BTutorialManager _tutorialManager;
    protected GuideUI _tutorialUI;

    protected virtual void Awake()
    {
        _tutorialManager = BTutorialManager.Instance;

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
