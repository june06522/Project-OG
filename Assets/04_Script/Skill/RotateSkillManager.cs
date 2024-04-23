using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RotateCloneInfo
{
    public WeaponID ID;
    public RotateClone Prefab;
}

public class RotateSkillManager : MonoBehaviour
{
    [SerializeField] float minRotateTime = 5f;
    [SerializeField] float maxRotateTime = 15f;
    [SerializeField] float minRotateSpeed = 100f;
    [SerializeField] float maxRotateSpeed = 1000f;

    [SerializeField] float dissolveTime = 0.5f;

    [Header("Eclipse")]
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float theta;

    private float curWidth;
    private float curHeight;

    private List<RotateClone> rotateClones;

    private Dictionary<WeaponID, int> _cloneDictionary; // 무기별 생성해야 할 클론 저장해둔 Dictionary.
    public List<RotateCloneInfo> CloneInfo; // Enum과 ClonePrefab 바인딩 정보 모음.

    private bool _endSetting = false;
    private bool isRunning = false;
    public bool IsRunning
    {
        get { return IsRunning; }
        set
        {
            if (value != isRunning)
            {
                if(value == false)
                {
                    RotateEndSetting();
                }
                else
                {
                    RotateStartSetting();
                }

                isRunning = value;
            }
        }
    }

    //RotateSkill에서 호출.
    public void SetCloneInfo(WeaponID id, int count)
    {
        _cloneDictionary[id] = count;
    }

    public RotateClone GetClonePrefabInfo(WeaponID id)
    {
        RotateCloneInfo cloneInfo = CloneInfo.Find((clone) => clone.ID == id);
        if (cloneInfo.Prefab == null)
        {
            return cloneInfo.Prefab;
        }
        return null;
    }

    private void RotateStartSetting()
    {
        Transform playerTrm = GameManager.Instance.player;

        // 클론 생성.
        foreach (var info in  _cloneDictionary) 
        {
            for(int i = 0 ; i < info.Value; ++i)
            {
                RotateClone clone = Instantiate(GetClonePrefabInfo(info.Key), playerTrm);
                clone.Init();
                rotateClones.Add(clone);
            }
        }

        // 클론 위치 세팅 & Dissolve 세팅
        int cloneCnt = rotateClones.Count;
        for (int i = 0; i < cloneCnt; i++)
        {
            float angle = 360 / cloneCnt * i;

            Vector2 pos = Eclipse.GetElipsePos(Vector2.zero, angle * Mathf.Deg2Rad, width, height, theta);

            rotateClones[i].transform.localPosition = pos;
            rotateClones[i].transform.up = pos.normalized;

            rotateClones[i].Dissolve(dissolveTime, true);
        }

        _endSetting = true;
    }

    private void RotateEndSetting()
    {
        _endSetting = false;

        int cloneCnt = rotateClones.Count;
        
        for (int i = 0; i < cloneCnt; i++)
        {
            rotateClones[i].Dissolve(dissolveTime, false);
        }

        rotateClones.Clear();
    }

    private void Awake()
    {
        rotateClones = new List<RotateClone>();
    }

    private void Update()
    {
        if (IsRunning && _endSetting)
        {
            int cloneCnt = rotateClones.Count;
            float addRotateValue = maxRotateSpeed * Time.deltaTime;
            for (int i = 0; i < cloneCnt; i++)
            {
                RotateClone clone = rotateClones[i];
                Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clone.CurAngle * Mathf.Deg2Rad,
                                curWidth, curHeight, theta);

                clone.CurAngle += addRotateValue;
                clone.Move(pos, false);
            }
        }
    }
}
