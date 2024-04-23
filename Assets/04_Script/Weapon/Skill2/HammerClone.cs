using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.VFX;

public class HammerClone : MonoBehaviour
{
    [SerializeField]
    private AudioClip _clip;
    private float rotateSpeed = 20f;
    private float damage = 10f;
    private float dissolveTime;

    Material material;
    SpriteRenderer spriteRenderer;

    public float CurAngle { get; set; }
    public bool EndDissolve { get; set; } = false;

    Color baseColor = new Color(5.58f, 5.58f, 5.58f, 1);
    Color baseEdgeColor = new Color(60f, 60f,   60f, 1);

    Color legendColor = new Color(3.811765f, 2.753964f, 0.2394826f, 1);
    Color legendEdgeColor = new Color(47.93726f, 40.74229f, 2.760784f);

    Color frozenColor = new Color(0f, 2.47273f, 4.179571f, 1);
    Color frozenEdgeColor = new Color(0f, 51.51302f, 73.42529f);

    public bool Frozen { get; set;}
    private VisualEffect effect;

    protected virtual void Awake()
    {
        spriteRenderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        material = spriteRenderer.material;
        effect = transform.Find("Flash").GetComponent<VisualEffect>();
    }

    public void Init(float rotateSpeed, float dissolveTime, float damage, float initAngle, Vector2 scale, bool frozen, bool isMaxPower)
    {
        this.rotateSpeed = rotateSpeed;
        this.dissolveTime = dissolveTime;
        this.damage = damage;
        this.CurAngle = initAngle;
        this.Frozen = frozen;

        transform.localScale = scale;

        material.SetFloat("_RainbowFade", 0f);
        material.SetFloat("_FrozenFade", 0f);
        if(frozen)
        {
            material.SetFloat("_FrozenFade", 1f);
            material.SetColor("_FullGlowDissolveEdgeColor", frozenEdgeColor);
            effect.SetVector4("Color01", frozenColor);
            this.Frozen = true;
        }
        else
        {
            if(isMaxPower)
            {
                material.SetFloat("_RainbowFade", 1f);
                material.SetColor("_FullGlowDissolveEdgeColor", legendEdgeColor);
                effect.SetVector4("Color01", legendColor);
            }
            else
            {
                material.SetColor("_FullGlowDissolveEdgeColor", baseEdgeColor);
                effect.SetVector4("Color01", baseColor);

            }
            this.Frozen = false;

        }
        
        effect.Play();
    }

    public void Dissolve(bool on)
    {
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
            SoundManager.Instance.SFXPlay("HitHammerRotate", _clip, 0.25f);
            if(Frozen)
            {
                IDebuffReciever debuffReciever;
                if(collision.TryGetComponent<IDebuffReciever>(out debuffReciever))
                {
                    debuffReciever.SetDebuff(EDebuffType.Frozen, 2f);
                    debuffReciever.DebuffEffect(EDebuffType.Frozen, 2f);
                }
            }

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

    public void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
