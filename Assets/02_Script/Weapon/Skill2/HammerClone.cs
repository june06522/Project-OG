using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HammerClone : MonoBehaviour
{
    private float rotateSpeed = 20f;
    private float damage = 10f;
    private float dissolveTime;

    Material material;
    SpriteRenderer spriteRenderer;

    public float CurAngle { get; set; }
    public bool EndDissolve { get; set; } = false;

    protected virtual void Awake()
    {
        spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
    }

    public void Init(float rotateSpeed, float dissolveTime, float damage, float initAngle)
    {
        this.rotateSpeed = rotateSpeed;
        this.dissolveTime = dissolveTime;
        this.damage = damage;
        this.CurAngle = initAngle;
    }

    public void Dissolve(bool on)
    {
        float initValue = on == true ? 0 : 1;

        material.SetFloat("_SourceGlowDissolveFade", initValue);

        float value = initValue;
        float endValue = Mathf.Abs(1 - initValue);
        DOTween.To(() => value, x => material.SetFloat("_SourceGlowDissolveFade", x), endValue, dissolveTime)
           .OnComplete(() =>
           {
               EndDissolve = true;
               if (on == false)
                   Destroy(this.gameObject);
           });
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHitAble hitAble;
        if (collision.TryGetComponent<IHitAble>(out hitAble))
        {
            hitAble.Hit(damage);
        }
    }

    public void Move(Vector2 movePos)
    {
        //float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;
        Vector2 dir = movePos.normalized;
        transform.up = dir;
        transform.localPosition = movePos;
    }
}
