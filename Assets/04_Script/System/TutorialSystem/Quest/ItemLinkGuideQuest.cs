using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLinkGuideQuest : GuideQuest
{
    public int linkPower;

    ConnectVisible connectVisible;

    private void Start()
    {
        connectVisible = FindObjectOfType<ConnectVisible>();
    }

    public override bool IsQuestComplete()
    {
        return CheckItemLink();
    }

    private bool CheckItemLink()
    {
        return connectVisible.ConnectCnt >= linkPower;
    }
}
