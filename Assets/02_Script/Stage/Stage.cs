using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
        waveCount--;
        StartCoroutine(MonsterSpawn());
    }

    // linked Enemy class's deadEvent
    private void HandleWaveClearCheck()
    {
        monsterCount--;
        if(monsterCount <= 0)
        {

            if(waveCount <= 0)
            {

                AppearGate();

            }
            else
                StartWave();

        }    
    }

    private void AppearGate()
    {
        // appear Gate ...it need tween
        if(NextStage.Count <= 1)
        {
            StageGate gate = Instantiate(stageGate, transform.position, Quaternion.identity);
            gate.SetStage(NextStage[Random.Range(0, NextStage.Count)]);
        }
        else if (NextStage.Count == 2)
        {
            // 2
            StageGate gate = Instantiate(stageGate,
                    transform.position - new Vector3(40, 0, 0), Quaternion.identity);
            gate.SetStage(NextStage[Random.Range(0, NextStage.Count)]);

            StageGate gate2 = Instantiate(stageGate,
                    transform.position + new Vector3(40, 0, 0), Quaternion.identity);
            gate2.SetStage(NextStage[Random.Range(0, NextStage.Count)]);
        }

        

        // effect or tween
    }

    IEnumerator MonsterSpawn()
    {
        isMonsterSpawning = true;
        WaveInfo waveInfo = waveList[waveCount];
        yield return new WaitForSeconds(0.1f);
        
        


        isMonsterSpawning = false;
    }

}
