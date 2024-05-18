using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RotateClone : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO _DataSO;
    private float damage = 10f;
    private float dissolveTime;

    Material material;
    SpriteRenderer spriteRenderer;
    protected Transform visualTrm;

    public float CurAngle { get; set; }
    public bool EndDissolve { get; set; } = false;

    private VisualEffect effect;

    private Tween dissolveTween;
    protected virtual void Awake()
    {
        visualTrm = transform.Find("Visual");
        spriteRenderer = visualTrm.GetComponent<SpriteRenderer>();
        effect = transform.Find("Flash").GetComponent<VisualEffect>();
        material = spriteRenderer.material;
    }

    public void Init(Vector2 scale)
    {
        transform.localScale = scale;

        this.damage = _DataSO.GetDamage();
        material.SetFloat("_FullGlowDissolveFade", 0);
    }

    public void Init()
    {
        this.damage = _DataSO.GetDamage();
        material.SetFloat("_FullGlowDissolveFade", 0);
        
        InvokeRepeating(nameof(Attack), 0.5f, _DataSO.AttackCoolDown.GetValue());
    }

    public void PlayAppearEffect() => effect.Play();

    public void Dissolve(float dissolveTime, bool on)
    {
        //PlayAppearEffect();
        dissolveTween.Kill();

        float initValue = on == true ? 0 : 1;

        material.SetFloat("_FullGlowDissolveFade", initValue);

        float value = material.GetFloat("_FullGlowDissolveFade");
        float endValue = Mathf.Abs(1 - initValue);
        float curDissolveTime 
            = dissolveTime * Mathf.Lerp(initValue, endValue, Mathf.Abs(initValue - value));
        
        dissolveTween = DOTween.To(() => value, x => material.SetFloat("_FullGlowDissolveFade", x), endValue, dissolveTime)
           .OnComplete(() =>
           {
               EndDissolve = true;
               if (on == false)
                   DestroyThis();
           });
        dissolveTween.Play();
    }

    protected virtual void Attack() { }

    public void Move(Vector3 movePos, bool Tween)
    {
        //float angle = Mathf.Atan2(weaponTrm.right.y, weaponTrm.right.x) * Mathf.Rad2Deg;
        Vector3 dir = movePos.normalized;
        dir.z = 0;

        if (Tween)
        {
        //    DOTween.To(() => transform.up, (vec) => transform.up = vec, dir, 0.25f).SetEase(Ease.InOutBack);
        //    DOTween.To(() => transform.localPosition, (vec) => transform.localPosition = vec, movePos, 0.25f).SetEase(Ease.InOutBack).OnComplete(() => Dissolve(false));

        }
        else
        {
            transform.right = dir;
            transform.localPosition = movePos;
            
        }
    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
