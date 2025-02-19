using UnityEngine;

public class GuidedEmp : Skill
{
    [SerializeField] EMPBomb empBomb;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        Vector3 targetPos = weaponTrm.position;

        if (target != null)
        {
            targetPos = target.position;
        }

        var obj = Instantiate(empBomb, weaponTrm.position, weaponTrm.rotation);
        obj.Throw(targetPos, power * 3, 0.8f);

        obj.transform.parent = target;

    }

}
