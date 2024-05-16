using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTooltipGuideQuest : BaseTextGuideQuest
{
    ItemExplain weaponExplain;

    private void Start()
    {
        weaponExplain = FindObjectOfType<ItemExplain>();
    }

    public override bool IsQuestComplete()
    {
        return weaponExplain.IsOn();
    }
}
