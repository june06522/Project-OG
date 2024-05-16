using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGuideQuest : GuideQuest
{

    [SerializeField]
    private Item _interactItem;

    private bool _isQuestEnd = false;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();

        _interactItem.OnInteractItem += HandleInteractItem;
    }

    private void HandleInteractItem(Transform transform)
    {
        _interactItem.OnInteractItem -= HandleInteractItem;

        _isQuestEnd = true;
    }

    public override bool IsQuestComplete()
    {
        return _isQuestEnd;
    }
}
