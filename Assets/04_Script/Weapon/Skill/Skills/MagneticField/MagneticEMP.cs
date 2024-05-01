using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticEMP : Skill
{
    [SerializeField] MagneticObject prefab;
    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        // 1.3f
        Debug.Log(2);
        StartCoroutine(Generate(target, power));

    }

    private IEnumerator Generate(Transform target, int power)
    {

        yield return new WaitForSeconds(0.5f);

        var obj = Instantiate(prefab, target.position, Quaternion.identity);
        obj.SetDamage((power + 1) * 10);

    }

}
