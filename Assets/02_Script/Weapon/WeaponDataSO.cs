using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Weapon/WeaponData")]
public class WeaponDataSO : ScriptableObject
{

    [field:SerializeField] public Stats AttackCoolDown { get; protected set; }
    [field:SerializeField] public Stats WeaponValue { get; protected set; }
    [field:SerializeField] public Stats AttackRange { get; protected set; }
    [field:SerializeField] public Stats ChangeGauge { get; protected set; }

    private Weapon owner;

    public bool isAttackCoolDown { get; protected set; }

    public void Init(Weapon owner)
    {

        this.owner = owner;

    }

    public void SetCoolDown()
    {

        owner.StartCoroutine(SetCoolDownCo());

    }

    private IEnumerator SetCoolDownCo()
    {

        isAttackCoolDown = true;

        yield return new WaitForSeconds(AttackCoolDown.GetValue());

        isAttackCoolDown = false;

    }

}
