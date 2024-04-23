using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDiceSkill : Skill
{
    List<Shell> weaponList;

    private void Start()
    {
        weaponList = SkillContainer.Instance.GetList();
    }

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        Skill s = null;
        int i, j;

        while(s==null)
        {
            i = Random.Range(1,weaponList.Count);
            if (weaponList[i] == null || weaponList[i].skillList.Count > 1 || i == (int)GeneratorID.ErrorDice)
                continue;

            j = Random.Range(1, weaponList[i].skillList.Count);
            if (weaponList[i].skillList[j] != null)
                s = weaponList[i].skillList[j];
        }

        s.Excute(weaponTrm,target,power);
    }
}