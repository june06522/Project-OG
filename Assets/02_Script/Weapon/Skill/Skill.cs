using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[Serializable]
public class Skill : MonoBehaviour
{
    protected bool isMaxPower;
    public virtual void Excute(Transform weaponTrm, Transform target, int power)
    {



    }

    /// <summary>
    /// 현재 연결된 연결부 갯수에 따른 스킬 강화를 진행
    /// </summary>
    /// <param name="power"></param>
    protected virtual void CurPowerInit(int power)
    {
        power = Mathf.Clamp(power, 1, 5);
        switch (power) 
        {
            case 1: Power1(); break;
            case 2: Power2(); break;
            case 3: Power3(); break;
            case 4: Power4(); break;
            case 5: Power5(); break;
        }   
    }

    public virtual void Power1() { }    
    public virtual void Power2() { }    
    public virtual void Power3() { }    
    public virtual void Power4() { }    
    public virtual void Power5() { }    

}
