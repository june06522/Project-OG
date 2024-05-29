using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// inspector에서 보이는 변수들
public partial class CrabBoss
{
    public GameObject crabLaserCollector;

    public GameObject crabLeftNipper;
    public GameObject crabRightNipper;

    public GameObject leftJoints;
    public GameObject rightJoints;

    public GameObject leftEye;
    public GameObject rightEye;

    public GameObject instantWalls;

    [Header("Transform")]
    public Transform leftFirePos;
    public Transform rightFirePos;

    public Transform leftGuidePos;
    public Transform rightGuidePos;

    public Transform mouthFirePos;

    [Header("SpriteRenderer")]
    public List<SpriteRenderer> spriteRendererList;

    [Header("Color")]
    public Color eyesColor;

    [Header("Animator")]
    public Animator animator;
}

public partial class CrabBoss : Boss
{
    [HideInInspector] public readonly int laserShooting = Animator.StringToHash("IsLaserShooting");
    [HideInInspector] public readonly int swing = Animator.StringToHash("IsSwing");
    [HideInInspector] public readonly int bubbleShooting = Animator.StringToHash("IsBubbleShooting");
    [HideInInspector] public readonly int leftPunching = Animator.StringToHash("IsLeftPunching");
    [HideInInspector] public readonly int die = Animator.StringToHash("Die");
    [HideInInspector] public readonly int start = Animator.StringToHash("Start");

    [HideInInspector] public bool idleAnimationEnd;

    private BossState _curBossState;

    private BossFSM _bossFSM;

    private CrabPattern _pattern;

    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        _pattern = GetComponent<CrabPattern>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _curBossState = BossState.Idle;
        _bossFSM = new BossFSM(new CIdleState(this, _pattern));
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Update()
    {
        base.Update();

        ChangeState();

        _bossFSM.UpdateBossState();
    }

    private IEnumerator EyesAnimation(float animationTime)
    {
        bool left = true;
        int animationCount = 0;
        Vector3 originEyeSize = leftEye.transform.localScale;

        while(!IsDie)
        {
            if(animationCount > 2)
            {
                animationCount = 0;
            }

            if(left)
            {
                ScaleChangeAnimationTypes(ref animationCount, leftEye, originEyeSize, animationTime, 0.5f);
                yield return new WaitForSeconds(animationTime);
                left = false;
            }
            else
            {
                ScaleChangeAnimationTypes(ref animationCount, rightEye, originEyeSize, animationTime, 0.5f);
                yield return new WaitForSeconds(animationTime);
                left = true;
            }
            yield return null;
        }
    }

    private void ScaleChangeAnimationTypes(ref int num, GameObject obj, Vector3 originSize, float animationTime, float changeValue)
    {
        float biggerValue = 1 + changeValue;
        float smallerValue = 1 - changeValue;

        switch(num)
        {
            case 0:
                obj.transform.DOScaleX(originSize.x * biggerValue, animationTime / 3).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        obj.transform.DOScaleX(originSize.x * smallerValue, animationTime / 3).SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            obj.transform.DOScaleX(originSize.x, animationTime / 3).SetEase(Ease.InOutSine);
                        });
                    });
                num++;
                break;
            case 1:
                obj.transform.DOScaleY(originSize.y * biggerValue, animationTime / 3).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        obj.transform.DOScaleY(originSize.y * smallerValue, animationTime / 3).SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            obj.transform.DOScaleY(originSize.y, animationTime / 3).SetEase(Ease.InOutSine);
                        });
                    });
                num++;
                break;
            case 2:
                obj.transform.DOScale(originSize * biggerValue, animationTime / 3).SetEase(Ease.InOutSine)
                    .OnComplete(() =>
                    {
                        obj.transform.DOScale(originSize * smallerValue, animationTime / 3).SetEase(Ease.InOutSine)
                        .OnComplete(() =>
                        {
                            obj.transform.DOScale(originSize, animationTime / 3).SetEase(Ease.InOutSine);
                        });
                    });
                num++;
                break;
            default:
                break;
        }
    }

    private void ChangeState()
    {
        switch (_curBossState)
        {
            case BossState.Idle:
                if(idleAnimationEnd)
                {
                    ChangeBossState(BossState.Alive);
                    StartCoroutine(EyesAnimation(0.6f));
                }
                break;
            case BossState.Alive:
                if(IsDie)
                {
                    ChangeBossState(BossState.Dead);
                }
                break;
        }
    }

    public void CrabReturnAll()
    {
        int childCount;
        GameObject[] objs;

        childCount = crabLaserCollector.transform.childCount;
        objs = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            objs[i] = crabLaserCollector.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < childCount; i++)
        {
            if (objs[i] == null)
                continue;
            ObjectPool.Instance.ReturnObject(objs[i]);
        }
    }

    private void ChangeBossState(BossState nextBossState)
    {
        _curBossState = nextBossState;

        switch (_curBossState)
        {
            case BossState.Alive:
                _bossFSM.ChangeBossState(new CAliveState(this, _pattern));
                break;
            case BossState.Dead:
                _bossFSM.ChangeBossState(new CDeadState(this, _pattern));
                break;
        }
    }

    public void StartAnimationEnded()
    {
        idleAnimationEnd = true;
    }

    public void BossIdlePos()
    {
        if(idleAnimationEnd)
        {
            transform.localPosition = new Vector3(0, 9.5f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(0, 35f, 0);
        }
    }

    public void CallRoar()
    {
        StartCoroutine(Roar());
    }

    private IEnumerator Roar()
    {
        animator.speed = 0;
        CameraManager.Instance.CameraShake(10, 1);
        yield return new WaitForSeconds(1);
        animator.speed = 1;
    }
}
