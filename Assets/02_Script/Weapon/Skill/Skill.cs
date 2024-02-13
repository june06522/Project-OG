using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill : MonoBehaviour
{

    public virtual void Excute(Transform weaponTrm, Transform target, int power)
    {
        Debug.Log($"{weaponTrm} 아직 스킬 안만들어짐");
    }

}
