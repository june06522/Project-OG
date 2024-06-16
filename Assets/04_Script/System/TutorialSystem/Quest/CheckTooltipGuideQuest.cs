using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTooltipGuideQuest : BaseTextGuideQuest
{

    public override bool IsQuestComplete()
    {
        return ItemExplain.Instance.IsOn();
    }
}
