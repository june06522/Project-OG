using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakGuide : GuideQuest
{
    private bool _isEndSpeak = false;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();
        FAED.InvokeDelay(() => { _isEndSpeak = true; }, guideTextTime + 3f);
    }

    public override bool IsQuestComplete()
    {
        return InputNextKey();
    }

    private bool InputNextKey()
    {

        _isEndSpeak = _isEndSpeak || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0);

        return _isEndSpeak;
    }

}
