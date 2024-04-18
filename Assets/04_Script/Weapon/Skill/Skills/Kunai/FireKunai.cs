using System.Collections;
using UnityEngine;

// ÃâÇ÷
public class FireKunai : Skill
{

    [SerializeField] GameObject fire;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        Debug.Log(power);

        if (target.TryGetComponent<IHitAble>(out var h))
            StartCoroutine(BloodCo(h, target, power));

    }

    IEnumerator BloodCo(IHitAble h, Transform target, int power)
    {

        YieldInstruction wait = new WaitForSeconds(0.3f);

        var f = Instantiate(fire);
        f.transform.SetParent(target);

        for (int i = 0; i < power; i++)
        {

            yield return wait;
            h.Hit(power);

        }

        f.GetComponent<FireController>().SetEnd();

    }

}
