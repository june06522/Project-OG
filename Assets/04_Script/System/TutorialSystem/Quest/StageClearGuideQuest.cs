using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageClearGuideQuest : GuideQuest
{
    private bool _isEndQuest = false;

    [SerializeField]
    private Stage _stage;

    private void Start()
    {
        _stage.OnStageClearEvent += () => { _isEndQuest = true; };
    }

    public override bool IsQuestComplete()
    {
        return _isEndQuest;
    }
}
