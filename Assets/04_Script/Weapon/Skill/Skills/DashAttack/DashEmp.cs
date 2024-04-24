using UnityEngine;

public class DashEmp : Skill
{

    [SerializeField] EMPBomb empBomb;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {

        if (target == null) return;
        Vector3 targetPos = target.position;
        var obj = Instantiate(empBomb, weaponTrm.position, weaponTrm.rotation);
        obj.renderers.ForEach(r => r.color = Color.blue);
        obj.transform.localScale *= 1 + power * 0.2f;
        obj.Throw(targetPos, power * 9);

    }

}
