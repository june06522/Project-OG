using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class MonsterSpawnManager : MonoBehaviour
{
    public static MonsterSpawnManager Instance;

    [Header("Wave정보")]
    [SerializeField] List<WaveSO> essentialWaves; // 필수로 들어갈 wave 엘리트 wave같은거
    [SerializeField] List<WaveSO> waves;

    public WaveSO[,] selectWave;
    [HideInInspector]
    public MonsterSpawn monsterSpawn = new MonsterSpawn();

    public void Awake()
    {
        #region 싱글톤
        if (Instance == null)
            Instance = this;
        else
            Debug.LogError($"{transform} : MonsterSpawnManager is Multiple running!");
        #endregion
    }

    public void DecideWave(List<RoomInfo> useRooms, int height, int width)
    {
        #region 예외처리
        if (essentialWaves.Count + waves.Count < useRooms.Count - 1)
            Debug.LogError($"{transform} : You should add wave");
        #endregion

        selectWave = new WaveSO[height, width];

        for (int i = 0; i < useRooms.Count; i++)
        {
            if (useRooms[i].x == 0 && useRooms[i].y == 0)
                continue;

            if (essentialWaves.Count > 0)
            {
                selectWave[useRooms[i].y, useRooms[i].x] = essentialWaves[0];
                essentialWaves.Remove(essentialWaves[0]);
            }
            else
            {
                WaveSO tempWave = RandomSelect();
                selectWave[useRooms[i].y, useRooms[i].x] = tempWave;
                waves.Remove(tempWave);
            }
        }
    }

    private WaveSO RandomSelect()
    {
        int maxVal = 0;
        int randomVal;
        for(int i = 0; i < waves.Count; ++i)
        {
            maxVal += waves[i].percentage;
        }

        randomVal = Random.Range(0, maxVal);
        maxVal = 0;
        for(int i = 0;i < waves.Count; ++i)
        {
            maxVal += waves[i].percentage;

            if (randomVal > maxVal)
                return waves[i];
        }

        Debug.LogError($"{transform} : Random Select System is error");
        return null;
    }
}
