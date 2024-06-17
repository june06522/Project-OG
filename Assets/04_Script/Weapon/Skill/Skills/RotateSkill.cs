using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkill : Skill
{
    [SerializeField] WeaponID _ID;

    RotateSkillManager rotateManager;

    int cloneCnt = 0;
    private void Awake()
    {
        rotateManager = FindObjectOfType<RotateSkillManager>();
    }

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Weapon weapon = weaponTrm.GetComponent<Weapon>();

        // 넘어온 무기가 rotateClone이면 생성 X
        if (weapon is not RotateClone)
        {
            //int count = Mathf.Clamp((power + 1) / 2, 1, 5);
            int count = power;
            rotateManager.SetCloneInfo(weapon, _ID, count);
        }
        else
        {
            Debug.Log("Is RotateClone");
        }
    }


    //Power Init
    public override void Power1()
    {
    }

    public override void Power2()
    {
    }

    public override void Power3()
    {
    }

    public override void Power4()
    {
    }

    public override void Power5()
    {
    }


}
