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
    private float curRotateSpeed;

    private List<RotateClone> rotateClones;

    //private Dictionary<Transform, Weapon> _cloneDictionary; // ���⺰ �����ؾ� �� Ŭ�� �����ص� Dictionary.
    private Dictionary<Weapon, int> _weaponTrmDictionary; // 한 무기에 연결된 생성기 수.
    private Dictionary<Weapon, List<SendData>> _weaponSkillList;

    public List<RotateCloneInfo> CloneInfo; // Enum�� ClonePrefab ���ε� ���� ����.

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
                    StopAllCoroutines();
                    StartCoroutine(RotateStartSetting());
                }

                isRunning = value;
            }
        }
    }

    //RotateSkill���� ȣ��.

    public void SetCloneInfo(Weapon weapon, WeaponID id)
    {

        if(!_weaponTrmDictionary.ContainsKey(weapon))
        {
            _weaponTrmDictionary[weapon] = 1;
        }
        else
        {
            _weaponTrmDictionary[weapon]++; // 클론 갯수 증가.
        }
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

        // Ŭ�� ����.
        foreach (var info in _weaponTrmDictionary) 
        {
            for(int i = 0 ; i < info.Value; ++i)
            {
                RotateClone clone = Instantiate(GetClonePrefabInfo(info.Key.id), playerTrm); //재활용하는 걸로 바꿔야 함
                clone.Init(_weaponSkillList[info.Key]);
                rotateClones.Add(clone);

                List<SendData> sendData = SkillManager.Instance.GetSkillList(clone); // 스킬 캐싱.
                sendData.ForEach((data) =>
                {
                    SkillManager.Instance.RegisterSkill(data.TriggerID, clone, data);
                });
            }
        }

        // Ŭ�� ��ġ ���� & Dissolve ����
        int cloneCnt = rotateClones.Count;
        float modifyValue = (cloneCnt / radiusIncreaseFlag);

        curWidth = width + modifyValue * 0.5f;
        curHeight = height + modifyValue * 0.5f;
        curRotateSpeed = maxRotateSpeed + maxRotateSpeed * modifyValue;

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

    // 인벤토리 확정되는 시점에 초기화
    private void RotateEndSetting()
    {
        _endSetting = false;

        int cloneCnt = rotateClones.Count;
        
        for (int i = 0; i < cloneCnt; i++)
        {
            // rotateClones[i].Dissolve(dissolveTime, false);
            // rotateClones[i].EndDissolve(); //조금의 최적화.
            rotateClones[i].DestroyThis();
        }
        //Debug.Log("Clear");
        //foreach(var info in _cloneDictionary)
        //{
        //    _cloneDictionary[info.Key] = 0;
        //}
        _weaponSkillList.Clear();
        _weaponTrmDictionary.Clear();
        rotateClones.Clear();
    }

    private void SetRunning()
    {
        IsRunning = true;
    }
    private void SetIdle()
    {
        //Debug.Log(IsRunning);
        IsRunning = false;
    }

    private void Awake()
    {
        rotateClones = new List<RotateClone>();
        _weaponSkillList = new();
        _weaponTrmDictionary = new();
        curRotateSpeed = maxRotateSpeed;
    }

    private void Start()
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
            float addRotateValue = curRotateSpeed * Time.deltaTime;
            for (int i = 0; i < cloneCnt; i++)
            {
                RotateClone clone = rotateClones[i];
                Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clone.CurAngle,
                                curWidth, curHeight, theta);

                clone.CurAngle += addRotateValue;
                clone.Move(pos);

            }
        }
    }
}
