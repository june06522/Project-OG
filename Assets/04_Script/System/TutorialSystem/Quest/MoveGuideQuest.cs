using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGuideQuest : BaseTextGuideQuest
{
    public override bool IsQuestComplete()
    {
        return (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D));
    }
}
