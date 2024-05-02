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
    [SerializeField] float maxRotateSpeed = 1000f;

    [SerializeField] float dissolveTime = 0.5f;

    [Header("Eclipse")]
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float theta;
    [SerializeField] float radiusIncreaseFlag = 5;

    private float curWidth;
    private float curHeight;

    private List<RotateClone> rotateClones;

    private Dictionary<WeaponID, int> _cloneDictionary; // 무기별 생성해야 할 클론 저장해둔 Dictionary.
    public List<RotateCloneInfo> CloneInfo; // Enum과 ClonePrefab 바인딩 정보 모음.

    private bool _endSetting = false;
    [SerializeField]
    private bool isRunning = false;
   
    public bool IsRunning
    {
        get { return isRunning; }
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
                    //RotateStartSetting();
                    StopAllCoroutines();
                    StartCoroutine(RotateStartSetting());
                }

                isRunning = value;
            }
        }
    }

    //RotateSkill에서 호출.
    public void SetCloneInfo(WeaponID id, int count)
    {
        _cloneDictionary[id] = count;
        Debug.Log($"{id.ToString()}: {count}");
    }

    public RotateClone GetClonePrefabInfo(WeaponID id)
    {
        RotateCloneInfo cloneInfo = CloneInfo.Find((clone) => clone.ID == id);
        if (cloneInfo.Prefab != null)
        {
            return cloneInfo.Prefab;
        }
        return null;
    }

    private IEnumerator RotateStartSetting()
    {
        yield return new WaitForEndOfFrame();
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

        curWidth = width + (cloneCnt/ radiusIncreaseFlag) * 0.5f;
        curHeight = height + (cloneCnt/ radiusIncreaseFlag) * 0.5f;

        for (int i = 0; i < cloneCnt; i++)
        {
            float angle = 360 / cloneCnt * i;
            //Debug.Log("Angle : " + angle);
            Vector2 pos = Eclipse.GetElipsePos(Vector2.zero, angle, curWidth, curHeight, theta);

            rotateClones[i].CurAngle = angle;
            rotateClones[i].transform.localPosition = pos;
            rotateClones[i].transform.up = pos.normalized;

            //rotateClones[i].PlayAppearEffect();
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
            // rotateClones[i].Dissolve(dissolveTime, false);
            rotateClones[i].DestroyThis();
        }
        //Debug.Log("Clear");
        //foreach(var info in _cloneDictionary)
        //{
        //    _cloneDictionary[info.Key] = 0;
        //}
        _cloneDictionary = new();
        rotateClones.Clear();
    }

    private void SetRunning() => IsRunning = true;
    private void SetIdle()
    {
        //Debug.Log(IsRunning);
        IsRunning = false;
    }

    private void Awake()
    {
        rotateClones = new List<RotateClone>();
        _cloneDictionary = new();

        foreach(var type in Enum.GetValues(typeof(WeaponID)))
        {
            _cloneDictionary[(WeaponID)type] = 0;
        }
    }

    private void OnEnable()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnMove += SetRunning;
            PlayerController.EventController.OnIdle += SetIdle;

        }
    }

    private void OnDisable()
    {
        if (PlayerController.EventController != null)
        {

            PlayerController.EventController.OnMove -= SetRunning;
            PlayerController.EventController.OnIdle -= SetIdle;

        }
    }

    private void Update()
    {
        curWidth = width;
        curHeight = height;
        if (isRunning && _endSetting)
        {
            int cloneCnt = rotateClones.Count;
            //Debug.Log($"ang : " + cloneCnt);
            float addRotateValue = maxRotateSpeed * Time.deltaTime;
            for (int i = 0; i < cloneCnt; i++)
            {
                RotateClone clone = rotateClones[i];
                Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clone.CurAngle,
                                curWidth, curHeight, theta);

                clone.CurAngle += addRotateValue;
                clone.Move(pos, false);
            }
        }
    }
}
