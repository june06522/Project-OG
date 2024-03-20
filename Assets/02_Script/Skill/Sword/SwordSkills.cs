using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordSkill
{
    Yondu,
    SwordRain,
}

public class SwordSkills : MonoBehaviour
{
    [SerializeField]
    private SwordClone smallSwordClone;
    [SerializeField]
    private SwordClone bigSwordClone;

    [SerializeField]
    private TargetZone targetZone;

    [Header("Sword Make Setting")]
    [SerializeField]
    private float instantiateCount = 6;
    [SerializeField]
    private float dissolveTime = 0.5f;
    [SerializeField]
    private float delayTime = 0.5f;

    [SerializeField]
    private float attackRadius = 2f;
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

    private void Awake()
    {
        clones = new();
        ownerTrm = GameManager.Instance.player.transform;
#if UNITY_EDITOR
        Debug.Log("Running");
#endif
        Make();
    }

    float t = 0;
    void Update()
    {

        if (makeDone)
        {
            float angle = 0;
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
                    Vector3 pos = GetElipsePos(makePos, angle, tempWidth, tempHeight);
                    clone.CurAngle = angle;
                    angle += 360 / instantiateCount * Mathf.Deg2Rad;
                    clone.Move(pos);
                }
            }
            t += Time.deltaTime * rotateSpeed;
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
        Vector2 bigClonePos = new Vector2(makePos.x, makePos.y + 2f);

        SwordClone bigClone = Instantiate(bigSwordClone, bigClonePos, Quaternion.Euler(new Vector3(0, 0, 270)));
        bigClone.Setting(warningZoneFadeTime * 2f);
        bigClone.transform.DOMoveY(bigClone.transform.position.y - 1f, warningZoneFadeTime + 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() => bigClone.Attack(targetPos));

        yield return new WaitForSeconds(0.4f);
        TargetZone zone = Instantiate(targetZone, targetPos, Quaternion.identity);
        Vector2 targetScale = new Vector2(tempWidth * 2.5f, tempHeight * 2.5f);
        zone.Marking(warningZoneFadeTime, targetScale);

        yield return new WaitForSeconds(1.3f);

        for (int i = 0; i < clones.Count; i++)
        {
            SwordClone clone = clones[i];
            float x = UnityEngine.Random.Range(-1f, 1f);
            float y = UnityEngine.Random.Range(-1f, 1f);
            Vector2 _targetPos = GetElipsePos(targetPos, clone.CurAngle, tempWidth / 1.5f, tempHeight / 1.5f);
            clone.Attack(_targetPos);
            yield return new WaitForSeconds(0.07f);
        }

        Destroy(zone.gameObject);
    }

    //angle : radian
    private Vector2 GetElipsePos(Vector2 centerPos, float angle, float width, float height)
    {
        float cx = centerPos.x;
        float cy = centerPos.y;

        float theta = this.theta;

        float weight = angle;
        float x = cx + width * Mathf.Cos(t + weight);
        float y = cy + height * Mathf.Sin(t + weight);

        float dx = cx + (x - cx) * Mathf.Cos(theta) - (y - cy) * Mathf.Sin(theta);
        float dy = cy + (x - cx) * Mathf.Sin(theta) + (y - cy) * Mathf.Cos(theta);

        return new Vector2(dx, dy);
    }

    private void Make()
    {
        clones.Clear();
        makeDone = false;
        isAttack = false;

        tempWidth = width;
        tempHeight = height;

        t = 0;
        StopAllCoroutines();
        StartCoroutine(MakeCoroutine());
    }

    private IEnumerator MakeCoroutine()
    {
        makePos = ownerTrm.position + new Vector3(0, 3f, 0);

        for (int i = 0; i < instantiateCount; i++)
        {
            float angle = 360 / instantiateCount * Mathf.Deg2Rad * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            SwordClone clone = Instantiate(smallSwordClone, GetElipsePos(makePos, angle, this.width, this.height), Quaternion.Euler(0, 0, 270));
            clone.Setting(dissolveTime);

            clones.Add(clone);
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(delayTime);
        float targetWidth = width * 1.5f;
        float targetHeight = height * 1.5f;
        DOTween.To(() => tempWidth, (curWidth) => tempWidth = curWidth, targetWidth, rotateTime).SetEase(Ease.InOutQuint);
        DOTween.To(() => tempHeight, (curHeight) => tempHeight = curHeight, targetHeight, rotateTime).SetEase(Ease.InOutQuint);


        makeDone = true;
    }


}
