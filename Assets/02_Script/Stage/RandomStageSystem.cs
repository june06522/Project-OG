using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using TMPro;
using UnityEngine;

[System.Serializable]
public struct FloorList
{
    public List<FloorInfoSO> floors;
}

public class RandomStageSystem : MonoBehaviour
{
    [Header("Stage Info")]

    // Stage changes as difficulty increases
    [SerializeField]
    private List<FloorList> _floorStageList = new List<FloorList>();

    [Header("Game Info")]
    [SerializeField]
    private TextMeshProUGUI _floorTitle;
    [SerializeField]
    private TextMeshProUGUI _floorTipText;

    private Stage _firstStage;
    private int _step = 0;
    private int _stageInterval = 80;
    private Vector3 _spawnPos = Vector3.zero;

    private void Start()
    {
        CreateStage();
    }

    public void CreateStage()
    {
        _spawnPos = _spawnPos + new Vector3(0, _stageInterval, 0);
        GameManager.Instance.player.position = _spawnPos;

        FloorInfoSO floorInfo = GetRandomFloor(_floorStageList[_step].floors);
        PrintStage(floorInfo);
        StartStageEvent(floorInfo);
    }

    private void StartStageEvent(FloorInfoSO floorInfo)
    {
        if (_floorTitle == null || _floorTipText == null)
            return;

        // Title
        _floorTitle.text = floorInfo.FloorName;

        // Tip
        _floorTipText.text = floorInfo.FloorTip[Random.Range(0, floorInfo.FloorTip.Count)];

        // Anim
        _floorTitle.color = Color.white;
        _floorTipText.color = Color.white;

        Sequence seq = DOTween.Sequence();
        seq.Append(_floorTitle.rectTransform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutElastic));
        seq.Append(_floorTipText.rectTransform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.InOutQuint));

        seq.Append(_floorTitle.rectTransform.DOScale(Vector3.one, 2f))
            .Join(_floorTitle.DOFade(0f, 2f))
            .Join(_floorTipText.rectTransform.DOScale(Vector3.one, 2f))
            .Join(_floorTitle.DOFade(0f, 2f));
    }

    private void PrintStage(FloorInfoSO floorInfo)
    {

        #region List Setting
        List<StageType> printStageInfo = floorInfo.PrintStageInfo;

        List<Stage> stages = GetStageList(floorInfo, StageType.EnemyStage);
        List<Stage> eventStages = GetStageList(floorInfo, StageType.EventStage);
        List<Stage> bossStages = GetStageList(floorInfo, StageType.BossStage);

        Queue<Stage> enemyStageQ = ShuffleStageList(stages);
        Queue<Stage> eventStageQ = ShuffleStageList(eventStages);
        Queue<Stage> bossStageQ = ShuffleStageList(bossStages);
        #endregion

        #region Create Stages
        Stage lastStage = _firstStage = Instantiate(floorInfo.StartStage, _spawnPos, Quaternion.identity);
        

        for (int i = 0; i < printStageInfo.Count; ++i)
        {
            if (i == 0 && printStageInfo[i] == StageType.Start)
                continue;

            // current Spawn Stage
            Stage stage = null;

            switch (printStageInfo[i])
            {
                case StageType.EnemyStage:
                    stage = enemyStageQ.Dequeue();
                    break;
                case StageType.EventStage:
                    stage = eventStageQ.Dequeue();
                    break;
                case StageType.BossStage:
                    stage = bossStageQ.Dequeue();
                    break;
                case StageType.Shop:
                    stage = floorInfo.ShopStage;
                    break;
                case StageType.Start:
                    stage = floorInfo.StartStage;
                    break;
            }

            _spawnPos = _spawnPos + new Vector3(0, _stageInterval, 0);
            stage = Instantiate(stage, _spawnPos, Quaternion.identity);
            lastStage.AddNextStage(stage);
            lastStage = stage;
        }
        #endregion

        _firstStage.AppearGate();
        lastStage.OnGateEvent += ClearBossStage;
        
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

    private Queue<Stage> ShuffleStageList(List<Stage> stages)
    {
        Queue<Stage> stageQueue = new Queue<Stage>();

        for (int i = 0; i < stages.Count; ++i)
        {
            int randomIdx = Random.Range(i, stages.Count);

            Stage temp = stages[randomIdx];
            stages[randomIdx] = stages[i];

            stageQueue.Enqueue(temp);
        }

        return stageQueue;
    }
    private FloorInfoSO GetRandomFloor(List<FloorInfoSO> floorList)
    {
        return Instantiate(floorList[Random.Range(0, floorList.Count)]);
    }
    private List<Stage> GetStageList(FloorInfoSO floorInfo, StageType stageType)
    {
        List<Stage> returnList = null;
        switch (stageType)
        {
            case StageType.EnemyStage:
                returnList = floorInfo.EnemySpawnStageList.stages.ToList<Stage>();
                break;
            case StageType.EventStage:
                returnList = floorInfo.EventStageList.stages.ToList<Stage>();
                break;
            case StageType.BossStage:
                returnList = floorInfo.BossStageList.stages.ToList<Stage>();
                break;
        }

        return returnList;
    }
}
