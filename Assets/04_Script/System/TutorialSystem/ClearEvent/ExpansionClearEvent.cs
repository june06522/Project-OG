using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionClearEvent : QuestClearEvent
{
    protected override void ClearEvent()
    {
        ExpansionManager.Instance.AddSlotcnt(2);
    }

}
