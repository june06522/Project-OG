using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomSize
{ 
    Small,
    Normal,
    Big,
}

[System.Serializable]
public struct MonsterWaveInfo
{
    public GameObject monsterObj; // 나중에 몬스터 공통 스크립트로
    [Header("등장 확률")]
    [Range(0f, 1f)] public float percentage;
}

[CreateAssetMenu(menuName = "SO/Wave/WaveData")]
public class WaveSO : ScriptableObject
{
    [Header("등장할 wave 갯수")]
    [Range(1, 5)] public int waveCnt = 1;

    [Header("등장할 몬스터 갯수 범위")]
    public int minCnt= 1;
    public int maxCnt= 10;


    [Header("몬스터 정보")]
    public List<MonsterWaveInfo> monsterWaveInfo;

    [Header("방크기")]
    public RoomSize roomSize = RoomSize.Normal;
}