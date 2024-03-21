using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
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
    public Transform spawnPos;
    public Enemy enemy;
}

[System.Serializable]
public struct DelayedSpawnMonster
{
    public float delay;
    public List<MonsterSpawnInfo> enemyList;
}

[System.Serializable]
public struct WaveInfo
{
    
    public List<Transform> spawnPoints;
    public List<MonsterSpawnInfo> mobInfo;

    public List<Transform> delayedSpawnPoints;
    public List<DelayedSpawnMonster> delayedSpawnMobInfo;

    // anchored spawn monster
    public List<AnchoredMonsterSpawnInfo> anchoredMobInfo;
}

public enum StageType
{
    EnemyStage = 0,
    EventStage = 1,
    BossStage = 2
}

public class Stage : MonoBehaviour
{
    public List<Stage> NextStage { get; private set; } = new List<Stage>();
    public event Action OnStageClearEvent;
    public event Action OnGateEvent;

    [Header("Stage Info")]
    public StageGate stageGate;
    private List<GameObject> stageItems = new List<GameObject>();

    [SerializeField]
    private Chest _stageChest;

    [SerializeField]
    private StageType _stageType = StageType.EnemyStage;
    public StageType ThisStageType => _stageType;

    [SerializeField]
    private ParticleSystem _enemySpawnParticle;

    [Header("Wave Info")]
    // waveInfo, wave count is list count
    public List<WaveInfo> waveList;
    private int monsterCount;
    private int waveCount = 0;

    private bool isMonsterSpawning = false;

    public void AddNextStage(Stage stage)
    {
        NextStage.Add(stage);
    }

    public void StartWave()
    {
        if(_stageType == StageType.EventStage)
        {

            AppearGate();
            return;

        }
        else if (_stageType == StageType.BossStage)
        {
            AppearGate(); //test code
            // After Creating Boss, Delete AppearGate
            return;
        }

        // Wave Start
        StartCoroutine(MonsterSpawn());
        waveCount++;
    }

    private void AppearChest()
    {
        if (_stageChest != null)
        {

            GameObject chest = Instantiate(_stageChest, transform.position - new Vector3(0, 3, 0), Quaternion.identity).gameObject;
            stageItems.Add(chest);

        }
    }

    // linked Enemy class's deadEvent
    private void HandleWaveClearCheck()
    {
        monsterCount--;
        if(monsterCount <= 0 && isMonsterSpawning == false)
        {
            if(_stageType == StageType.BossStage)
            {
                OnStageClearEvent?.Invoke();
                return;
            }    

            if(waveCount >= waveList.Count)
            {

                AppearGate();
                AppearChest();
            }
            else
            {
                isMonsterSpawning = true;
                StartWave();
            }

        }    
    }

    public void AppearGate()
    {

        // appear Gate ...it need tween
        if(_stageType == StageType.BossStage || NextStage.Count == 0)
        {
            SpawnGate(null);
        }
        else if (NextStage.Count == 1)
        {
            SpawnGate(NextStage[0]);
        }
        else if (NextStage.Count == 2)
        {
            SpawnGate(NextStage[0], -new Vector3(2, 0, 0));
            SpawnGate(NextStage[1], new Vector3(2, 0, 0));
        }

        // effect or tween
    }

    private void HandleGateEvent()
    {
        OnGateEvent?.Invoke();  
    }

    private void SpawnGate(Stage stage, Vector3 offset = new Vector3())
    {
        StageGate gate = Instantiate(stageGate, transform.position + offset, Quaternion.identity);
        gate.OnGateEvent += HandleGateEvent;
        gate.OnMoveEndEvent += HandleDestroyGate;
        stageItems.Add(gate.gameObject);

        if(stage != null)
        {

            gate.SetStage(stage);

        }
    }
    public void DestroyStage()
    {
        foreach(var stage in NextStage)
        {
            stage.DestroyStage();
        }
        Destroy(gameObject);
    }
    private void HandleDestroyGate()
    {
        foreach(GameObject obj in stageItems)
        {
            Destroy(obj);
        }
    }

    IEnumerator MonsterSpawn()
    {
        isMonsterSpawning = true;
        WaveInfo waveInfo = waveList[waveCount];
        #region Setting SpawnPos
        // Monster List
        List<Enemy> enemyList = new List<Enemy>();
        for(int i = 0; i < waveInfo.mobInfo.Count; ++i)
        {
            for(int j = 0; j < waveInfo.mobInfo[i].count; ++j)
            {
                enemyList.Add(waveInfo.mobInfo[i].enemy);
            }
            
        }

        // Monster Shuffle
        for(int i = 0; i < enemyList.Count; ++i)
        {
            int randomIdx = Random.Range(i, enemyList.Count);

            Enemy tempEnemy = enemyList[i];
            enemyList[i] = enemyList[randomIdx];
            enemyList[randomIdx] = tempEnemy;
        }

        // SpawnPoint Shuffle
        List<Transform> spawnPoints = waveInfo.spawnPoints;
        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            int randomIdx = Random.Range(i, spawnPoints.Count);

            Transform tempPoint = spawnPoints[i];
            spawnPoints[i] = spawnPoints[randomIdx];
            spawnPoints[randomIdx] = tempPoint;
        }

        int minValue = Math.Min(enemyList.Count, spawnPoints.Count);
        #endregion
        #region Multi Spawn
        // SpawnPoint Particle
        for (int i = 0; i < minValue; ++i)
        {
            Instantiate(_enemySpawnParticle, spawnPoints[i].position, Quaternion.identity).Play();
        }

        for (int i = 0; i < waveInfo.anchoredMobInfo.Count; ++i)
        {
            Instantiate(_enemySpawnParticle, waveInfo.anchoredMobInfo[i].spawnPos.position, Quaternion.identity).Play();
        }

        yield return new WaitForSeconds(0.2f);
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        for(int i = 0; i < minValue; ++i)
        {
            monsterCount++;
            Enemy spawnEnemy = Instantiate(enemyList[i], spawnPoints[i].position + offset, Quaternion.identity);
            spawnEnemy.DeadEvent += HandleWaveClearCheck;
        }
        for (int i = 0; i < waveInfo.anchoredMobInfo.Count; ++i)
        {
            monsterCount++;
            Enemy spawnEnemy = Instantiate(waveInfo.anchoredMobInfo[i].enemy, waveInfo.anchoredMobInfo[i].spawnPos.position, Quaternion.identity);
            spawnEnemy.DeadEvent += HandleWaveClearCheck;
        }
        #endregion
        #region Delayed Spawn Monster
        
        // Delayed Spawn
        for(int idx = 0; idx < waveInfo.delayedSpawnMobInfo.Count; ++idx)
        {
            // 딜레이
            yield return new WaitForSeconds(waveInfo.delayedSpawnMobInfo[idx].delay);

            // spawn Points Suffle
            List<Transform> delaySpawnList = waveInfo.spawnPoints.ToList<Transform>();
            for (int i = 0; i < delaySpawnList.Count; ++i)
            {
                int randomIdx = Random.Range(i, delaySpawnList.Count);

                Transform tempSpawn = delaySpawnList[i];
                delaySpawnList[i] = delaySpawnList[randomIdx];
                delaySpawnList[randomIdx] = tempSpawn;
            }
            
            // setting spawn enemy List
            List<Enemy> spawnEnemies = new List<Enemy>();
            for (int i = 0; i < waveInfo.delayedSpawnMobInfo[idx].enemyList.Count; ++i)
            {
                for (int cnt = 0; cnt < waveInfo.delayedSpawnMobInfo[idx].enemyList[i].count; ++cnt)
                {
                    spawnEnemies.Add(waveInfo.delayedSpawnMobInfo[idx].enemyList[i].enemy);
                }
            }
            // setting monster
            minValue = Math.Min(delaySpawnList.Count, spawnEnemies.Count);

            // Shuffle enemy List
            for (int i = 0; i < spawnEnemies.Count; ++i)
            {
                int randomIdx = Random.Range(i, spawnEnemies.Count);

                Enemy tempSpawnEnemy = spawnEnemies[i];
                spawnEnemies[i] = spawnEnemies[randomIdx];
                spawnEnemies[randomIdx] = tempSpawnEnemy;
            }

            // 파티클 작업
            for (int i = 0; i < minValue; ++i)
            {
                Instantiate(_enemySpawnParticle, delaySpawnList[i].position, Quaternion.identity).Play();
            }

            yield return new WaitForSeconds(0.2f);

            // 스폰 연속으로 진행
            for (int i = 0; i < minValue; ++i)
            {
                monsterCount++;
                Enemy spawnEnemy = Instantiate(spawnEnemies[i], delaySpawnList[i].position + offset, Quaternion.identity);
                spawnEnemy.DeadEvent += HandleWaveClearCheck;
            }

            
        }


        #endregion


        isMonsterSpawning = false;
    }
}
