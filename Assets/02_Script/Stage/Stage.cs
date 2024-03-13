using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonsterSpawnInfo
{
    public Enemy enemy;
    public int count;
}

[System.Serializable]
public struct AnchoredMonsterSpawnInfo
{
    public Vector2 spawnPos;
    public Enemy enemy;
}

[System.Serializable]
public struct WaveInfo
{
    public List<Vector2> spawnPoints;
    public List<MonsterSpawnInfo> mobInfo;

    // anchored spawn monster
    public List<AnchoredMonsterSpawnInfo> anchoredMobInfo;
}

public class Stage : MonoBehaviour
{
    public List<Stage> NextStage { get; private set; }

    [Header("Stage Info")]
    public StageGate stageGate;

    // waveInfo, wave count is list count
    public List<WaveInfo> waveList;
    private int monsterCount;
    private int waveCount;

    private bool isMonsterSpawning = false;

    private void Start()
    {

        waveCount = waveList.Count;

    }

    public void AddNextStage(Stage stage)
    {
        NextStage.Add(stage);
    }

    private void StartWave()
    {
        // Wave Start
        StartCoroutine(MonsterSpawn());
    }

    private void NextWave()
    {
        waveCount--;
    }

    // linked Enemy class's deadEvent
    private void HandleWaveClearCheck()
    {
        monsterCount--;
        if(monsterCount <= 0)
        {

            if(waveCount == 0)
            {

                AppearGate();

            }
            else
                NextWave();

        }    
    }

    private void AppearGate()
    {
        // appear Gate ...it need tween
        StageGate gate = Instantiate(stageGate);
        // effect or tween
    }

    IEnumerator MonsterSpawn()
    {
        isMonsterSpawning = true;
        WaveInfo waveInfo = waveList[waveCount - 1];
        yield return new WaitForSeconds(0.1f);
        
        


        isMonsterSpawning = false;
    }

}
