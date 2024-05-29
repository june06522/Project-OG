using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ItemLinkGuideQuest : BaseTextGuideQuest
{
    public VideoClip GuideVideoClip;
    public int linkPower;

    ConnectVisible connectVisible;

    private void Start()
    {
        connectVisible = FindObjectOfType<ConnectVisible>();
    }

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();

        _tutorialUI.SetVideo(GuideVideoClip);
        _tutorialUI.SetVideoOnOff(true);
    }

    public override bool IsQuestComplete()
    {
        return CheckItemLink();
    }

    public override void QuestComplete()
    {
        _tutorialUI.SetVideoOnOff(false);
        base.QuestComplete();
    }

    private bool CheckItemLink()
    {
        return connectVisible.ConnectCnt >= linkPower;
    }
}
