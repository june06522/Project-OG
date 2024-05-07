using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakGuide : GuideQuest
{
    [Header("Speak Time")]
    public float speakTime = 1f;

    private bool _isEndSpeak = false;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();
        FAED.InvokeDelay(() =>
        {
            _isEndSpeak = true;
        }, speakTime + guideTextTime);
    }

    public override bool IsQuestComplete()
    {
        return _isEndSpeak;
    }

    


}
