using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{

    [field: SerializeField] public Stats AttackCoolDown { get; protected set; }
    [field: SerializeField] public Stats AttackRange { get; protected set; }
    [field: SerializeField] public Stats AttackDamage { get; protected set; }
    //[field:SerializeField] public Stats WeaponValue { get; protected set; }
    //[field:SerializeField] public Stats ChangeGauge { get; protected set; }

    public float CoolDown = 0f;
    public float AddDamege = 0f;

    private Weapon owner;

    public bool isAttackCoolDown { get; protected set; }
    //public bool isSkillAttack = false;

    public void Init(Weapon owner)
    {

        //isSkillAttack = false;
        this.owner = owner;

    }

    public void SetCoolDown()
    {

        owner.StartCoroutine(SetCoolDownCo());

    }

    public float GetOriginCool() => AttackCoolDown.GetValue();

    public float GetCool() => AttackCoolDown.GetValue() / (1f + CoolDown / 100f);

    private IEnumerator SetCoolDownCo()
    {

        isAttackCoolDown = true;
        yield return new WaitForSeconds(AttackCoolDown.GetValue() /(1f + CoolDown / 100f));

        isAttackCoolDown = false;

    }

    public float GetDamage()
    {
        return AddDamege + AttackDamage.GetValue();
    }
}
