using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomStageSystem : MonoBehaviour
{
    [Header("Stage Info")]

    // Stage changes as difficulty increases
    [SerializeField]
    private List<StageListSO> _stageLists = new List<StageListSO>();
    [SerializeField]
    private List<StageListSO> _bossStageLists = new List<StageListSO>();

    [SerializeField]
    private StageListSO _eventStageLists;

    [SerializeField]
    private int _stageCount = 0;

    private List<Stage> _randomStageList = new List<Stage>();

    [Header("Game Info")]
    [SerializeField]
    private Stage _startStage;
    private Stage _firstStage;

    private int _step = 0;
    private int _stageInterval = 50;
    private Vector3 _spawnPos = Vector3.zero;

    private void Start()
    {
        CreateStage();
    }

    public void CreateStage()
    {
        _spawnPos = _spawnPos + new Vector3(0, _stageInterval, 0);
        GameManager.Instance.player.position = _spawnPos;

        _randomStageList.Clear();

        // shuffle
        _randomStageList = _stageLists[_step].stages.ToList<Stage>();
        for(int i = 0; i < _randomStageList.Count; i++)
        {
            int randomIdx = Random.Range(i, _randomStageList.Count);

            Stage temp = _randomStageList[i];
            _randomStageList[i] = _randomStageList[randomIdx];
            _randomStageList[randomIdx] = temp;
        }

        _firstStage = Instantiate(_startStage, _spawnPos, Quaternion.identity);
        Stage lastStage = _firstStage;
        for (int i = 0; i < _stageCount; i++)
        {
            _spawnPos = _spawnPos + new Vector3(0, _stageInterval, 0);

            Stage stage = Instantiate(_randomStageList[i], _spawnPos, Quaternion.identity);

            lastStage.AddNextStage(stage);
            lastStage = stage;
        }

        // event
        _spawnPos = _spawnPos + new Vector3(0, _stageInterval, 0);
        Stage eventStage = Instantiate(RandomStage(_eventStageLists), _spawnPos, Quaternion.identity);
        lastStage.AddNextStage(eventStage);

        // boss
        _spawnPos = _spawnPos + new Vector3(0, _stageInterval, 0);
        Stage bossStage = Instantiate(RandomStage(_bossStageLists[_step]), _spawnPos, Quaternion.identity);
        eventStage.AddNextStage(bossStage);

        bossStage.OnGateEvent += ClearBossStage;
        _firstStage.AppearGate();

    }

    public void ClearBossStage()
    {
        _step++;

        ResetStage();
        CreateStage();
    }

    private void ResetStage()
    {
        _firstStage.DestroyStage();
    }

    private Stage RandomStage(StageListSO stageList)
    {
        List<Stage> stages = stageList.stages;
        return stages[Random.Range(0, stages.Count)];
    }

}
