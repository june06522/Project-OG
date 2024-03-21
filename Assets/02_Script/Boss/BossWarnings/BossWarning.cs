using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWarning : MonoBehaviour
{
    [SerializeField]
    private float f_returnTime;

    protected virtual void OnEnable()
    {
        StartCoroutine(ObjectPool.Instance.ReturnObject(this.gameObject, f_returnTime));
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }
}
