using UnityEngine;

public class MagneticSpeaker : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {

        var obj = Instantiate(prefab, weaponTrm.position, Quaternion.identity);
        obj.transform.localScale = Vector3.one + Vector3.one * (0.5f * power);
        obj.SetDamage(power * 2);

    }


}
