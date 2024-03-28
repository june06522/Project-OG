using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

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
        if (on) return;
        float initValue = on == true ? 0 : 1;

        material.SetFloat("_FullGlowDissolveFade", initValue);

        float value = initValue;    
        float endValue = Mathf.Abs(1 - initValue);
        DOTween.To(() => value, x => material.SetFloat("_FullGlowDissolveFade", x), endValue, dissolveTime)
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

    public void Move(Vector3 movePos, bool Tween)
    {
        //float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;
        Vector3 dir = movePos.normalized;
        dir.z = 0;

        if(Tween) 
        {
            DOTween.To(() => transform.up, (vec) => transform.up = vec, dir, 0.25f).SetEase(Ease.InOutBack);
            DOTween.To(() => transform.localPosition, (vec) => transform.localPosition = vec, movePos, 0.25f).SetEase(Ease.InOutBack).OnComplete(()=>Dissolve(false));
            
        }
        else
        {
            transform.up = dir;
            transform.localPosition = movePos;
        }
    }
}
