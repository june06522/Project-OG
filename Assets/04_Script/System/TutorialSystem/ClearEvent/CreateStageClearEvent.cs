using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageClearEvent : QuestClearEvent
{
    [SerializeField]
    private RandomStageSystem _stageGenerator;

    protected override void ClearEvent()
    {
        _stageGenerator.CreateStage();
    }
}
