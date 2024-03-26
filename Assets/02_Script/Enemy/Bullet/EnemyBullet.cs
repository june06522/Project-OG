using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public enum EEnemyBulletCurveType
{
    None = 1 << 0,
    Curve360 = 1 << 1,
    Curve270 = 1 << 2,
    Curve180 = 1 << 3,
    Curve90 = 1 << 4,
}
public enum EEnemyBulletSpeedType
{
    Linear = 1 << 0,
    Wind  = 1 << 1,
    Expo = 1 << 5,
}


public class EnemyBullet : MonoBehaviour
{
    [SerializeField]
    protected BulletDataSO _dataSO;
    [SerializeField]
    float speed = 3;
    [SerializeField]
    float endSpeed = 15;
    [SerializeField]
    float duration = 0.75f;
    float curSpeed = 0;
    //юс╫ц

    private void Start()
    {
        if(_dataSO != null)
            endSpeed = _dataSO.Speed;    
    }

    public void Shoot(Vector2 dir, EEnemyBulletSpeedType speedType = EEnemyBulletSpeedType.Linear, EEnemyBulletCurveType curveType = EEnemyBulletCurveType.None)
    {
        RotateBullet(dir);

        switch (speedType)
        {
            case EEnemyBulletSpeedType.Linear:
                curSpeed = Mathf.Lerp(speed, endSpeed, 0.5f);
                break;
            case EEnemyBulletSpeedType.Wind:
                DOTween.To(() => speed, x => curSpeed = x, endSpeed, duration).SetEase(Ease.InOutBack);
                break;
            case EEnemyBulletSpeedType.Expo:
                DOTween.To(() => speed, x => curSpeed = x, endSpeed, duration).SetEase(Ease.InExpo);
                break;

        }

        switch (curveType)
        {
            case EEnemyBulletCurveType.Curve360:
            case EEnemyBulletCurveType.Curve270:
            case EEnemyBulletCurveType.Curve180:
            case EEnemyBulletCurveType.Curve90:
                float angle = float.Parse(curveType.ToString().Substring(5))
                     + Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 180;

                Vector3 rotateValue = new Vector3(0, 0, angle);

                transform.DORotate(rotateValue, duration, RotateMode.Fast).SetLoops(-1);
                break;
        }

        if (curSpeed == 0) curSpeed = speed;
        if (this.gameObject.activeSelf) DestroyThis(3);
    }

    private void DestroyThis(float time = 0f)
        => Destroy(this.gameObject, time);

    private void RotateBullet(Vector2 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    public void Update()
    {
        transform.position += transform.up * curSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_dataSO == null) return;
        if (collision.CompareTag(_dataSO.HitAbleTag[0]))
        {
            IHitAble hitAble;
            if(collision.TryGetComponent<IHitAble>(out hitAble))
            {
                hitAble.Hit(_dataSO.Damage);
            }
        }
    }

}
