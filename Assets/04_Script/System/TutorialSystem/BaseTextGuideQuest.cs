using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTextGuideQuest : GuideQuest
{

    [Header("Guide Text")]
    public string guideText;
    public float guideTextTime;

    public override void SetQuestSetting()
    {
        _tutorialUI.SetGuideText(guideText, guideTextTime);
    }

}
