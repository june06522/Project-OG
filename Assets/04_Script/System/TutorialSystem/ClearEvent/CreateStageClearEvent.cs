using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateStageClearEvent : QuestClearEvent
{
    [SerializeField]
    private RandomStageSystem _stageGenerator;
    [SerializeField]
    private PlayerHP _playerHP;

    protected override void ClearEvent()
    {
        if (_playerHP != null)
            _playerHP.RestoreHP(100000);

        if(_stageGenerator != null)
            _stageGenerator.CreateStage();
    }
}
