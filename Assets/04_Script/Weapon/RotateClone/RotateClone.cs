using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RotateClone : Weapon
{
    private float damage;
    private float dissolveTime;

    Material material;
    SpriteRenderer spriteRenderer;
    protected Transform visualTrm;

    public float CurAngle { get; set; }
    public bool EndDissolve { get; set; } = false;

    private Tween dissolveTween;

    public List<SendData> SkillList;

    protected override void Awake()
    {
        base.Awake();
        visualTrm = transform.Find("Visual");
        spriteRenderer = visualTrm.GetComponent<SpriteRenderer>();

        material = spriteRenderer.material;
    }

    public void Init(Vector2 scale)
    {
        transform.localScale = scale;

        this.damage = Data.GetDamage();
        material.SetFloat("_FullGlowDissolveFade", 0);
    }

    public void Init(List<SendData> skillList)
    {
        this.SkillList = skillList;
        this.damage = Data.GetDamage();
        material.SetFloat("_FullGlowDissolveFade", 0);

        InvokeRepeating(nameof(Attack), 0.5f, Data.AttackCoolDown.GetValue());
    }

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
    public override void Attack(Transform target) { } // ¾È¾¸

    public void Move(Vector3 movePos)
    {

        Vector3 dir = movePos.normalized;
        dir.z = 0;

        transform.right = dir;
        transform.localPosition = movePos;

    }

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
