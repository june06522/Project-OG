using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLinkGuideQuest : GuideQuest
{
    public int linkPower;

    public override bool IsQuestComplete()
    {
        return CheckItemLink();
    }

    private bool CheckItemLink()
    {
        // 이 부분에서 무기 - 연결기 - 스킬에서
        // 무기 - 스킬로 연결된 기본값이 1이라고 할 때
        // linkPower만큼 연결되어 있으면 true 반환
        // 연결이 안 되어있거나, linkPower만큼 연결되어있지 않다면 false 반환
        return true; 
    }
}
