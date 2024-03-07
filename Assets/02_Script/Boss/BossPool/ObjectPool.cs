using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private ObjectPoolData[] poolData;

    private readonly Dictionary<ObjectPoolType, ObjectPoolData> _objectPoolDataMap = new();

    private readonly Dictionary<ObjectPoolType, Queue<GameObject>> _pool = new();

    private void Awake()
    {
        Instance = this;

        Init();
    }

    private void Init()
    {
        foreach(var data in poolData)
        {
            _objectPoolDataMap.Add(data.ObjectType, data);
        }

        foreach(var data in _objectPoolDataMap)
        {
            _pool.Add(data.Key, new Queue<GameObject>());

            var objectPoolData = data.Value;

            for(int i = 0; i < objectPoolData.prefabCount; i++)
            {
                var objectPool = CreateNewObject(data.Key);
                _pool[data.Key].Enqueue(objectPool);
            }
        }
    }

    private GameObject CreateNewObject(ObjectPoolType type)
    {
        var newObj = Instantiate(_objectPoolDataMap[type].prefab, transform);
        newObj.SetActive(false);

        return newObj;
    }

    public GameObject GetObject(ObjectPoolType type, Transform trans = null)
    {
        if(_pool[type].Count > 0)
        {
            var obj = _pool[type].Dequeue();
            obj.transform.SetParent(trans);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = CreateNewObject(type);
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(trans);
            return newObj;
        }
    }

    public void ReturnObject(ObjectPoolType type, GameObject obj)
    {
        if(obj.activeSelf)
        {
            foreach (ObjectPoolData opd in poolData)
            {
                if (opd.ObjectType == type)
                {
                    obj.transform.localScale = opd.prefab.transform.localScale;
                }
            }
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            _pool[type].Enqueue(obj);
        }
    }

    public void ReturnObject(Transform trans)
    {
        int childCnt = trans.childCount;
        for (int i = 0; i < childCnt; i++)
        {
            if (trans.GetChild(i).gameObject == null)
            {
                Debug.Log(i);
                return;
            }
            ReturnObject(trans.GetChild(i).gameObject);
        }
        Debug.Log("dd");
    }

    public void ReturnObject(GameObject obj)
    {
        if(obj.activeSelf)
        {
            string objName = obj.name.Remove(obj.name.Length - 7);
            foreach (ObjectPoolData opd in poolData)
            {
                if (opd.prefab.name == objName)
                {
                    obj.transform.localScale = opd.prefab.transform.localScale;
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(transform);
                    _pool[opd.ObjectType].Enqueue(obj);
                }
            }
        }
    }
}
