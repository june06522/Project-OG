using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDiceSkill : Skill
{
    List<Shell> weaponList;

    float curtime = 0f;
    float time = 5f;

    private void Start()
    {
        weaponList = SkillContainer.Instance.GetList();
    }

    private void Update()
    {
        curtime += Time.deltaTime;
    }

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        //if (!GameManager.Instance.isPlay)
        //    return;

        Skill s = null;
        int i = 0, j = 0;

        while(s==null)
        {
            i = Random.Range(1,weaponList.Count);
            if (weaponList[i].skillList == null || weaponList[i].skillList.Count <= 1 || i == (int)GeneratorID.ErrorDice)
                continue;

            j = Random.Range(1, weaponList[i].skillList.Count);
            if (weaponList[i].skillList[j] != null)
                s = weaponList[i].skillList[j];
        }

        Debug.Log("스킬실행");
        if (WeaponExplainManager.triggerExplain[(GeneratorID)i] == TriggerID.Idle ||
        WeaponExplainManager.triggerExplain[(GeneratorID)i] == TriggerID.Move)
            StartCoroutine(Co(s, weaponTrm, target, power));
        else
            s.Excute(weaponTrm, target, power);
    }

    IEnumerator Co(Skill s,Transform weaponTrm, Transform target, int power)
    {
        curtime = 0.0f;
        while(curtime < time)
        {
            if(target != null && weaponTrm != null)
                s.Excute(weaponTrm, target, power);
            yield return null;
        }
        yield return null;
    }
}