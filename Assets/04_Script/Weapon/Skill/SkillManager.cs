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
public delegate void Regist();

public class SkillManager : MonoSingleton<SkillManager>
{
    public event Regist OnRegistEndEvent;

    private Dictionary<TriggerID, List<SkillInfo>> _skillList;

    private void Awake()
    {
        Init();
    }

    //초기화
    public void Init()
    {
        _skillList = new();

        foreach (TriggerID id in Enum.GetValues(typeof(TriggerID)))
        {
            _skillList.Add(id, new());
        }
    }

    //트리거 넘어오면 스킬 실행
    public void DetectTrigger(TriggerID id, Weapon weapon = null)
    {
        for (int i = 0; i < _skillList[id].Count; i++)
        {
            if (_skillList[id][i].weapon == null)
            {
                _skillList[id].Remove(_skillList[id][i]);
                i--;
                continue;
            }

            if (weapon != null)
            {
                if (_skillList[id][i].weapon == weapon)
                    _skillList[id][i].data.startWeapon = weapon;
                else
                    continue;
            }

            if (_skillList[id][i].data.GetTrriger() && _skillList[id][i].weapon != null)
            {

                SkillContainer.Instance.GetSKill((int)_skillList[id][i].weapon.id, (int)_skillList[id][i].data.GeneratorID)?.
                    Excute(_skillList[id][i].weapon.transform, _skillList[id][i].weapon.Target,
                    _skillList[id][i].data.Power, _skillList[id][i].data);
            }
        }
        foreach (var skillInfo in _skillList[id])
        {

        }
    }

    //스킬 등록
    public void RegisterSkill(TriggerID id, Weapon weapon, SendData data)
    {
        SkillInfo info = new SkillInfo(weapon, data);
        foreach (var skillInfo in _skillList[id])
        {
            if (skillInfo.data.index == info.data.index && skillInfo.weapon == info.weapon)
            {
                skillInfo.data.Power = Mathf.Max(skillInfo.data.Power, info.data.Power);
                return;
            }
        }

        _skillList[id].Add(info);
    }

    //스킬 받아오기
    public List<SendData> GetSkillList(Weapon weapon)
    {
        List<SendData> skillData = new();

        foreach (List<SkillInfo> info in _skillList.Values)
        {
            foreach (SkillInfo skill in info)
            {
                if (skill.weapon == weapon)
                    skillData.Add(skill.data);
            }
        }

        return skillData;
    }

    public void RegistEndEvent()
    {
        OnRegistEndEvent?.Invoke();
        EventTriggerManager.Instance.RegistExecute();
    }
}