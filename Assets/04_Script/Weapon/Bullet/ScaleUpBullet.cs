using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpBullet : Bullet
{
    float rotateSpeed;
    float weight;
    public void Init(float damage, float power, float rotateSpeed)
    {
        curDamage = damage;
        this.rotateSpeed = rotateSpeed;
        
        transform.localScale = Vector3.one * (Mathf.Pow(power, 2)/2) + Vector3.one;

        BulletData bulletData = Data;
        bulletData.Speed *= power * 0.5f;
        Data = bulletData;

        StartCoroutine(ReleaseBullet());

        weight = 0;
    }

    public void Update()
    {
        transform.Rotate(Vector3.forward * (rotateSpeed - weight / 5), Space.Self);
        
        BulletData bulletData = Data;
        bulletData.Speed += weight;
        Data = bulletData;

        weight += Time.deltaTime * 5;
    }

    protected override void HitOther()
    {
        Debug.Log("Gang");
    }

    private IEnumerator ReleaseBullet()
    {

        yield return new WaitForSeconds(3f);
        Release();

    }

}
