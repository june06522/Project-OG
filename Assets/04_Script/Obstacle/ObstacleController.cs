using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    [SerializeField] AppearWall _appearWall;
    [SerializeField] Thorn _thorn;
    [SerializeField] Cannon _cannon;

    ObstacleTransform[] _appearWallTrms;
    ObstacleTransform[] _thornTrms;
    ObstacleTransform[] _cannonTrms;

    List<AppearWall> _curAppearWalls = new();
    List<Thorn> _curThrons = new();
    List<Cannon> _curCannons = new();

    private void Awake()
    {
        _appearWallTrms = transform.Find("WallTrms").GetComponentsInChildren<ObstacleTransform>();
        _thornTrms = transform.Find("ThronTrms").GetComponentsInChildren<ObstacleTransform>();
        _cannonTrms = transform.Find("CannonTrms").GetComponentsInChildren<ObstacleTransform>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            MakeWalls();
        }

        if(Input.GetKeyDown(KeyCode.T)) 
        {
            MakeThorns();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            MakeCannons();
            
        }
    }

    private void MakeThorns()
    {
        //Debug
        _curAppearWalls.ForEach((obstacle)=> Destroy(obstacle.gameObject));
        _curCannons.ForEach((obstacle) => Destroy(obstacle.gameObject));
        _curThrons.ForEach((obstacle) => Destroy(obstacle.gameObject));

        _curAppearWalls.Clear();
        _curCannons.Clear();
        _curThrons.Clear();
    
        for(int i = 0; i < _thornTrms.Length; i++)
        {
            Vector3 pos = _thornTrms[i].GetPos();   
            Quaternion rot = _thornTrms[i].GetRot();

            _curThrons.Add(Instantiate(_thorn, pos, rot));
        }
    }

    private void MakeCannons()
    {
        //Debug
        _curAppearWalls.ForEach((obstacle) => Destroy(obstacle.gameObject));
        _curCannons.ForEach((obstacle) => Destroy(obstacle.gameObject));
        _curThrons.ForEach((obstacle) => Destroy(obstacle.gameObject));

        _curAppearWalls.Clear();
        _curCannons.Clear();
        _curThrons.Clear();

        for (int i = 0; i < _cannonTrms.Length; i++)
        {
            Vector3 pos = _cannonTrms[i].GetPos();
            Quaternion rot = _cannonTrms[i].GetRot();

            _curCannons.Add(Instantiate(_cannon, pos, rot));
        }
    }

    private void MakeWalls()
    {
        //Debug
        _curAppearWalls.ForEach((obstacle) => Destroy(obstacle.gameObject));
        _curCannons.ForEach((obstacle) => Destroy(obstacle.gameObject));
        _curThrons.ForEach((obstacle) => Destroy(obstacle.gameObject));

        _curAppearWalls.Clear();
        _curCannons.Clear();
        _curThrons.Clear();

        for (int i = 0; i < _appearWallTrms.Length; i++)
        {
            Vector3 pos = _appearWallTrms[i].GetPos();
            Quaternion rot = _appearWallTrms[i].GetRot();

            _curAppearWalls.Add(Instantiate(_appearWall, pos, rot));
        }
    }
}
