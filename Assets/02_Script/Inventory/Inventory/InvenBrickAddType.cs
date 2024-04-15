using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenBrickAddType : MonoBehaviour
{
    public Transform slot;
    public Transform weapon;
    public Transform connector;
    public Transform generator;

    #region 예외처리
    private void Awake()
    {
        if (slot == null)
            Debug.LogError($"{transform} : slot is null!");

        if (weapon == null)
            Debug.LogError($"{transform} : weapon is null!");

        if (connector == null)
            Debug.LogError($"{transform} : connector is null!");

        if (generator == null)
            Debug.LogError($"{transform} : generator is null!");
    }
    #endregion
}