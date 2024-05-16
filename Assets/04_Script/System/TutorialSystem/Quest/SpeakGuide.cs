using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakGuide : BaseTextGuideQuest
{
    [SerializeField]
    private bool _isResetSpeak = true;

    private bool _isEndSpeak = false;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();

        if(_isResetSpeak == false)
            FAED.InvokeDelay(() => { _isEndSpeak = true; }, guideTextTime + 3f);
        else
        {

            _isEndSpeak = true;
            TutorialManager.Instance.TutorialUI.ResetGuideText();
        }
    }

    public override bool IsQuestComplete()
    {
        return InputNextKey();
    }

    private bool InputNextKey()
    {

        _isEndSpeak = _isEndSpeak || Input.GetKeyDown(KeyCode.Return);

        return _isEndSpeak;
    }

}
