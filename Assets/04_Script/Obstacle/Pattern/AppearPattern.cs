using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearPattern : MonoBehaviour
{
    [SerializeField]
    AppearWall wallPrefab;

    ObstacleTransform[] obstacleTrms;
    List<AppearWall> appearWalls = new();

    private bool _isEnd = false;
    public bool IsEnd => _isEnd;

    private int endWallEventCount;

    private void Awake()
    {
        obstacleTrms = GetComponentsInChildren<ObstacleTransform>();
    }

    private void Start()
    {
        for(int i = 0; i < obstacleTrms.Length; i++)
        {
            AppearWall wall = Instantiate(wallPrefab, obstacleTrms[i].GetPos(), obstacleTrms[i].GetRot());
            wall.WallAppearEndEvent += () => endWallEventCount++;
            
            appearWalls.Add(wall);
        }

        Init();
    }

    public void Init()
    {
        _isEnd = false;
        endWallEventCount = 0;
    }

    public void HandleStartPattern() => StartCoroutine(StartPattern());

    private IEnumerator StartPattern()
    {
        appearWalls.ForEach((wall) =>
        {
            wall.Appear();
        });

        yield return new WaitUntil(() => endWallEventCount == appearWalls.Count);

        _isEnd = true;
    }
}
