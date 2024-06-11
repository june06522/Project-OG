using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/FloorInfoSO")]
public class FloorInfoSO : ScriptableObject
{
    [Header("Floor Info")]
    public string FloorName;
    public List<string> FloorTip;

    public Color FloorPlayerColor = Color.white;
    public Color FloorColor       = Color.white;

    public List<StageType> PrintStageInfo;

    [Header("StartStage")]
    public Stage StartStage;

    [Header("EnemySpawnStageList")]
    public StageListSO EnemySpawnStageList;

    [Header("EventStageList")]
    public StageListSO EventStageList;
    public Stage ShopStage;

    [Header("BossStageList")]
    public StageListSO BossStageList;

}
