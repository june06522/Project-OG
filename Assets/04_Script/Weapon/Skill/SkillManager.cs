using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SkillInfo
{
    public SkillInfo(Weapon weapon, SendData data)
    {
        this.weapon = weapon;
        this.data = data;
    }

    public Weapon weapon;
    public SendData data;
}

public class SkillManager : MonoSingleton<SkillManager>
{
    private Dictionary<TriggerID, List<SkillInfo>> skillList;

    private void Awake()
    {
        Init();
    }

    //초기화
    public void Init()
    {
        skillList = new();

        foreach(TriggerID id in Enum.GetValues(typeof(TriggerID)))
        {
            skillList.Add(id, new());
        }
    }

    //트리거 넘어오면 스킬 실행
    public void DetectTrigger(TriggerID id, Weapon weapon = null)
    {
        foreach(var skillInfo in skillList[id])
        {
            if(weapon != null)
            {
                if (skillInfo.weapon == weapon)
                    skillInfo.data.startWeapon = weapon;
                else
                    continue;
            }

            if(skillInfo.data.GetTrriger() && skillInfo.weapon != null)
            SkillContainer.Instance.GetSKill((int)skillInfo.weapon.id, (int)skillInfo.data.GeneratorID)?.
                Excute(skillInfo.weapon.transform,skillInfo.weapon.Target,
                skillInfo.data.Power,skillInfo.data);
        }
    }

    //스킬 등록
    public void RegisterSkill(TriggerID id, Weapon weapon, SendData data)
    {
        SkillInfo info = new SkillInfo(weapon, data);
        //Debug.Log(id);
        foreach(var skillInfo in skillList[id])
        {
            //Debug.Log($"{skillInfo.data.index} : {info.data.index}");
            if(skillInfo.data.index == info.data.index && skillInfo.weapon == info.weapon)
            {
                skillInfo.data.Power = Mathf.Max(skillInfo.data.Power, info.data.Power) ;
                return;
            }
        }
        skillList[id].Add(info);
    }
}