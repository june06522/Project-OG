using UnityEngine;

public class BigSword : Skill
{

    [SerializeField] SwordSkills prefab;
    [SerializeField] float damage;

    private float curDamage;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        CurPowerInit(power);
        SwordSkills obj = Instantiate(prefab, weaponTrm.position, Quaternion.identity);
        obj.transform.SetParent(weaponTrm.transform);
        obj.Make(weaponTrm.parent, curDamage, Vector2.zero);
    }

    public override void Power1()
    {
        curDamage = damage;
    }

    public override void Power2()
    {
        curDamage = damage * 2f;
    }

    public override void Power3()
    {
        curDamage = damage * 3f;
    }

    public override void Power4()
    {
        curDamage = damage * 4f;
    }

    public override void Power5()
    {
        curDamage = damage * 5f;
    }
}
