using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpansionManager : MonoBehaviour
{
    public static ExpansionManager Instance;

    [SerializeField] Transform plusObj;

    int _leftCnt = 0;

    private void Awake()
    {
        #region 싱글톤
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError($"{transform} : ExpansionManager is multiply running!");
            Destroy(gameObject);
        }
        #endregion
    }

    private void Update()
    {
        if(_leftCnt > 0)
        {

        }
    }
}