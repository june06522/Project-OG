using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : InvenWeapon
{

    [SerializeField] private Bullet kunai;
    [SerializeField] private float reboundVal;

    private SpriteRenderer _spriteRenderer;

    public float motionTime = 0.3f;
    private bool isReady = true;


    protected override void Awake()
    {

        base.Awake();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    [BindExecuteType(typeof(SendData))]
    public override void GetSignal([BindParameterType(typeof(SendData))] object signal)
    {
        var data = (SendData)signal;

        if (!sendDataList.ContainsKey(data.GeneratorID))
        {
            sendDataList.Add(data.GeneratorID, data);
        }
        else
        {
            sendDataList[data.GeneratorID].Power = sendDataList[data.GeneratorID].Power > data.Power ? sendDataList[data.GeneratorID].Power : data.Power;
        }
    }

    public override void Attack(Transform target)
    {
        if (!isReady) return;

        StartCoroutine(AttackCo());

    }

    IEnumerator AttackCo()
    {

        isReady = false;

        for (int i = 0; i < 3; i++)
        {
            transform.DOShakePosition(motionTime, 0.25f);/*.SetEase(Ease.Linear);*/
            yield return new WaitForSeconds(motionTime);

            Vector3 offset = Random.insideUnitCircle * reboundVal;
            var blt = Instantiate(kunai, transform.position + offset, transform.rotation);
            blt.Shoot(kunai.Data.Damage);
        }

        _spriteRenderer.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(0.5f);
        _spriteRenderer.color = new Color(1, 1, 1, 1);

        isReady = true;

    }

    protected override void RotateWeapon(Transform target)
    {

        if (target == null) return;

        var dir = target.position - transform.position;
        dir.Normalize();
        dir.z = 0;

        _spriteRenderer.flipY = dir.x switch
        {

            var x when x > 0 => false,
            var x when x < 0 => true,
            _ => _spriteRenderer.flipY

        };

        transform.right = dir;

    }

}
