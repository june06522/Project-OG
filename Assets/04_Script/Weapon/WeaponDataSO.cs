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

    public float coolTimeFactor = 0f;
    public float damagefactor = 0f;

    private Weapon owner;

    public bool isAttackCoolDown { get; protected set; }
    //public bool isSkillAttack = false;

    public void Init(Weapon owner)
    {

        //isSkillAttack = false;
        this.owner = owner;
        SynergyManager.Instance.OnSynergyChange += Change_Cool_N_Damage_Factor;

    }

    private void Change_Cool_N_Damage_Factor()
    {

        coolTimeFactor = SynergyManager.Instance.GetStatFactor(TriggerID.NormalAttack);
        damagefactor = SynergyManager.Instance.GetStatFactor(TriggerID.Kill);

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

        float coolTime = AttackCoolDown.GetValue() / (100f + coolTimeFactor * 100f + CoolDown) * 100f;
        yield return new WaitForSeconds(coolTime/*AttackCoolDown.GetValue() / (1f + CoolDown / 100f)*/);

        isAttackCoolDown = false;

    }

    public float GetDamage()
    {
        return (AddDamege + AttackDamage.GetValue()) * (1 + damagefactor);
    }

    private void OnDestroy()
    {

        SynergyManager.Instance.OnSynergyChange -= Change_Cool_N_Damage_Factor;

    }

}
