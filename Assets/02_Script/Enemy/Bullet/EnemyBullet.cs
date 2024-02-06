using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBullet : MonoBehaviour
{
    float speed = 3;
    float endSpeed = 30;
    float curSpeed = 0;
    float duration = 0.75f;
    Vector3 dir;
    //юс╫ц
    public void Shoot(Vector2 dir)
    {
        this.dir = dir.normalized;
        DOTween.To(() => speed, x => curSpeed = x, endSpeed, duration).SetEase(Ease.InExpo);
        Destroy(this.gameObject, 3f);
    }

    public void Update()
    {
        transform.position += dir * curSpeed * Time.deltaTime;
    }


}
