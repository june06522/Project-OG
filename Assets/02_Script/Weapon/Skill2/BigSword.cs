using UnityEngine;

public class BigSword : Skill
{

    [SerializeField] GameObject prefab;

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        var obj = Instantiate(prefab, weaponTrm.position, Quaternion.identity);              
    }

}
