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

    public List<SendData> skillList;

    private Collider2D[] enemyArr;
    LayerMask enemyLayer;

    protected override void Awake()
    {
        base.Awake();
        visualTrm = transform.Find("Visual");
        spriteRenderer = visualTrm.GetComponent<SpriteRenderer>();

        material = spriteRenderer.material;

        enemyArr = new Collider2D[50];
        enemyLayer = LayerMask.GetMask("TriggerEnemy", "Enemy", "Boss");
    }

    private void OnEnable()
    {
        this.damage = Data.GetDamage();
        material.SetFloat("_FullGlowDissolveFade", 0);
    }
    public void Init(List<SendData> skillList)
    {
        this.skillList = skillList;
        skillList.ForEach((data) =>
        {
            SkillManager.Instance.RegisterSkill(data.TriggerID, this, data);
        });
    }

    private void Update()
    {
        CheckEnemy();
    }
    public void CheckEnemy()
    {

        int cnt = Physics2D.OverlapCircle(
          transform.position,
          Data.AttackRange.GetValue(),
          new ContactFilter2D { layerMask = enemyLayer, useLayerMask = true, useTriggers = true }, enemyArr);

        Debug.Log($"{enemyArr.Length} : {cnt}");
        if (cnt != 0)
        {

            Run(FindCloseEnemy(cnt));

        }
        else
        {

            Run(null);

        }

    }

    public Transform FindCloseEnemy(int enemyCount)
    {

        float minDist = float.MaxValue;
        Transform curTarget = null;

        for (int i = 0; i < enemyCount; i++)
        {

            float dist = Vector2.Distance(enemyArr[i].transform.position, transform.position);

            if (minDist > dist)
            {

                minDist = dist;
                curTarget = enemyArr[i].transform;

            }

        }

        return curTarget;

    }


    public override void Run(Transform target, bool isSkill = false)
    {
        if (enabled == false) return;

        Debug.Log("Gang");
        this.target = target;

        if ((!Data.isAttackCoolDown || isSkill))
        {
            if (!Data.isAttackCoolDown)
                Data.SetCoolDown();

            EventTriggerManager.Instance?.BasicAttackExecute(this);

            Debug.Log("Gang222");
            Attack(target);

        }

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
                   enabled = false;
           });
        dissolveTween.Play();
    }

    public override void Attack(Transform target) { }

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
