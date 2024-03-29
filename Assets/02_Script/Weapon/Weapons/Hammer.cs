using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : InvenWeapon
{

    [SerializeField] ParticleSelfDestroyer effect;
    private bool isAttack = false;

    public override void Attack(Transform target)
    {

        DOTween.Sequence().
            Append(transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z - 90 * transform.localScale.y), 0.1f).SetEase(Ease.Linear)).
            Append(transform.DORotate(new Vector3(0, 0, transform.rotation.eulerAngles.z), 0.1f).SetEase(Ease.Linear));

        StartCoroutine(AttackTween());

    }

    private IEnumerator AttackTween()
    {

        isAttack = true;
        yield return new WaitForSeconds(0.09f);
        var obj = Instantiate(effect, transform.position + transform.up * 1.5f * transform.localScale.y, Quaternion.identity);
        obj.Attack(Data.AttackDamage.GetValue());
        yield return new WaitForSeconds(0.21f);
        isAttack = false;   

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {

        //var data = (SendData)signal;
        //SkillContainer.Instance.GetSKill((int)id, (int)data.GeneratorID)?.Excute(transform, target, data.Power);

        var data = (SendData)signal;

        if (!sendDataList.ContainsKey(data))
        {
            sendDataList.Add(data, data.Power);
        }
        else
        {
            sendDataList[data] = sendDataList[data] > data.Power ? sendDataList[data] : data.Power;
        }

    }

    protected override void RotateWeapon(Transform target)
    {

        if (target == null) return;
        if (isAttack == true) return;

        var dir = target.position - transform.position;
        dir.Normalize();
        dir.z = 0;

        transform.localScale = dir.x switch
        {

            var x when x > 0 => new Vector3(1, 1, 1),
            var x when x < 0 => new Vector3(1, -1, 1),
            _ => transform.localScale

        };

        transform.right = dir;

    }

}
