using UnityEngine;

public class MagneticSpeaker : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        var obj = Instantiate(prefab, weaponTrm.position, Quaternion.identity);
        obj.transform.localScale = Vector3.one * 2 * power;
        obj.SetDamage(power * 2);

    }


}
