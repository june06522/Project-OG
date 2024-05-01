using UnityEngine;

public class MagneticSword : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Debug.Log(3);
        var obj = Instantiate(prefab, weaponTrm.position, Quaternion.identity);
        obj.SetDamage(power * power);
    }

}
