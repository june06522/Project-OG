using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChest : StageChest
{
    public override void OnInteract()
    {
        Open();
        FindObjectOfType<TutorialManager>().isOpenChest = true;
    }
}
