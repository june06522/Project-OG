using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryOpenGuideQuest : GuideQuest
{
    public override bool IsQuestComplete()
    {
        return Input.GetKey(KeyCode.Tab);
    }
}
