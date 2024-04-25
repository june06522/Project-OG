using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ESwordSkill
{
    NONE,
    BIGSWORD,
    SWORDRAIN
}

public class SwordSkills : MonoBehaviour
{
    [SerializeField]
    private SmallSwordClone smallSwordClone;
    [SerializeField]
    private BigSwordClone bigSwordClone;

    [SerializeField]
    private TargetZone targetZone;

    [Header("Sword Make Setting")]
    [SerializeField]
    private ESwordSkill skills;
    [SerializeField]
    private float instantiateCount = 6;
    [SerializeField]
    private float dissolveTime = 0.5f;
    [SerializeField]
    private float delayTime = 0.5f;

    [SerializeField]
    private float rotateSpeed = 2f;
    [SerializeField]
    private float rotateTime = 1f;
    [SerializeField]
    private float yPosValue = 3f;
    [SerializeField]
    private float warningZoneFadeTime = 0.5f;

    [Header("Elipse")]
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField][Range(-5, 5)] private float theta;

    List<SwordClone> clones;
    Transform ownerTrm;

    private bool makeDone = false;
    private bool isAttack = false;
    private Vector2 makePos;

    private float tempWidth;
    private float tempHeight;

    private float damage;
    float t = 0;

    private void Awake()
    {
        clones = new();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    Make();
        //}

        if (makeDone)
        {
            for (int i = 0; i < clones.Count; i++)
            {
                SwordClone clone = clones[i];
                if (clone.EndAttack)
                {
                    clone.DestroyThis();
                    clones.Remove(clone);
                    i -= 1;
                    continue;
                }
                else
                {
                    Vector3 pos = Eclipse.GetElipsePos(makePos, clone.CurAngle, 
                                    tempWidth, tempHeight, this.theta);
                    float t = Time.deltaTime * rotateSpeed;
                    clone.CurAngle += t;
                    clone.Move(pos);
                }
            }
            this.t += Time.deltaTime;
        }

        if (t > rotateTime && isAttack == false)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttack = true;
        Vector2 targetPos = new Vector2(makePos.x, makePos.y - yPosValue - height);
        
        if(skills.HasFlag(ESwordSkill.BIGSWORD))
        {
            MakeBigClone(targetPos);

            yield return new WaitForSeconds(0.4f);
            TargetZone zone = Instantiate(targetZone, targetPos, Quaternion.identity);
            Vector2 targetScale = new Vector2(width * 3f, height * 3f);
            zone.Marking(warningZoneFadeTime, targetScale);
            Destroy(zone.gameObject, warningZoneFadeTime + 0.5f);
            
            yield return new WaitForSeconds(1.3f);
        }

        if (skills.HasFlag(ESwordSkill.SWORDRAIN))
        {

            for (int i = 0; i < clones.Count; i++)
            {
                SwordClone clone = clones[i];
                float x = UnityEngine.Random.Range(-1f, 1f);
                float y = UnityEngine.Random.Range(-1f, 1f);
                Vector2 _targetPos = Eclipse.GetElipsePos(targetPos, clone.CurAngle, tempWidth / 1.5f, tempHeight / 1.5f, theta);
                clone.Attack(_targetPos);
                yield return new WaitForSeconds(0.07f);
            }
        }

    }

    private void MakeBigClone(Vector2 targetPos)
    {
        Vector2 bigClonePos = new Vector2(makePos.x, makePos.y + 2f);

        SwordClone bigClone = Instantiate(bigSwordClone, bigClonePos, Quaternion.Euler(new Vector3(0, 0, 270)));
        bigClone.Setting(warningZoneFadeTime * 2f, width * 1.5f, height * 1.5f, makePos, damage);
        bigClone.transform.DOMoveY(bigClone.transform.position.y - 1f, warningZoneFadeTime + 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() => bigClone.Attack(targetPos));
    }

    //angle : radian

    public void Make(Transform ownerTrm, float damage, Vector2 radius)
    {
        clones.Clear();

        this.ownerTrm = ownerTrm;
        this.damage = damage;

        makeDone = false;
        isAttack = false;

        tempWidth = width;
        tempHeight = height;

        t = 0;
        makePos = ownerTrm.position + new Vector3(0, 3f, 0);

        StopAllCoroutines();
        if(skills.HasFlag(ESwordSkill.SWORDRAIN) && skills.HasFlag(ESwordSkill.BIGSWORD))
        {
            StartCoroutine(MakeSmallClone(true));
        }
        else
        {
            if(skills.HasFlag(ESwordSkill.SWORDRAIN))
            {
                StartCoroutine(MakeSmallClone(false));
            }
            else if(skills.HasFlag(ESwordSkill.BIGSWORD))
            {
                StartCoroutine(Attack());
            }
        }

    }


    private IEnumerator MakeSmallClone(bool widthChange)
    {
        for (int i = 0; i < instantiateCount; i++)
        {
            float angle = 360 / instantiateCount * Mathf.Deg2Rad * i;
            Vector2 pos = Eclipse.GetElipsePos(makePos, angle, this.width, this.height, this.theta);
            
            SwordClone clone = Instantiate(smallSwordClone, pos, Quaternion.Euler(0, 0, 270));
            clone.Setting(dissolveTime, width, height, makePos, damage / 5);
            clone.CurAngle = angle;

            clones.Add(clone);
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(delayTime);

        if (widthChange)
        {
            float targetWidth = width * 1.5f;
            float targetHeight = height * 1.5f;
            DOTween.To(() => tempWidth, (curWidth) => tempWidth = curWidth, targetWidth, rotateTime).SetEase(Ease.InOutQuint);
            DOTween.To(() => tempHeight, (curHeight) => tempHeight = curHeight, targetHeight, rotateTime).SetEase(Ease.InOutQuint);
        }
        makeDone = true;
    }
}
