using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractGuideQuest : GuideQuest
{
    public Transform itemSpawnPos;
    public Item spawnItemPrefab;
    private Item spawnItem;

    private bool _isQuestEnd = false;

    public override void SetQuestSetting()
    {
        base.SetQuestSetting();

        if (spawnItemPrefab == null)
            return;

        spawnItem = Instantiate(spawnItemPrefab, itemSpawnPos.position, Quaternion.identity);
        spawnItem.OnInteractItem += HandleInteractItem;
    }

    private void HandleInteractItem(Transform transform)
    {
        spawnItem.OnInteractItem -= HandleInteractItem;

        _isQuestEnd = true;
    }

    public override bool IsQuestComplete()
    {
        return _isQuestEnd;
    }
}
