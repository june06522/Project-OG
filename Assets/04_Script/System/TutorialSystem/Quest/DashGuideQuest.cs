using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashGuideQuest : GuideQuest
{
    public override bool IsQuestComplete()
    {
        return Input.GetKey(KeyCode.Space);
    }
}
