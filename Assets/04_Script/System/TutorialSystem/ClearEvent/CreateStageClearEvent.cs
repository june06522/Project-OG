
using System.Collections;
using UnityEngine;

public class CreateStageClearEvent : QuestClearEvent
{
    [SerializeField]
    private RandomStageSystem _stageGenerator;
    [SerializeField]
    private PlayerHP _playerHP;

    private StageTransition _stageTransition;

    private void Start()
    {
        _stageTransition = FindObjectOfType<StageTransition>();
    }

    protected override void ClearEvent()
    {
        if (_playerHP != null)
            _playerHP.RestoreHP(100000);

        if(_stageGenerator != null)
        {

            StartCoroutine(CreateStageEventCo());

        }
    }

    IEnumerator CreateStageEventCo()
    {

        SoundManager.Instance.BgStop();
        _stageTransition.StartTransition(1f);
        yield return new WaitForSeconds(2.5f);
        _stageGenerator.CreateStage();
        yield return new WaitForSeconds(2f);
        _stageTransition.EndTransition(1.5f);


    }

}
