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
    private SwordClone swordClone;
    [SerializeField]
    private WarningZone warningZone;

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
    private float warningZoneFadeTime = 0.5f;
    [SerializeField]
    private LayerMask layerMask;

    [Header("Elipse")]
    [SerializeField] private float width;
    [SerializeField] private float height;
    [SerializeField] [Range(-5,5)] private float theta;

    List<SwordClone> clones;
    Transform ownerTrm;

    private bool makeDone = false;
    private bool isAttack = false;
    private Vector2 makePos;

    private void Awake()
    {
        clones = new();
        ownerTrm = transform.parent;
    }

    float t = 0;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) 
        {
            Make();
        }

        if(makeDone) 
        {
            float angle = 0;
            for(int i = 0; i < clones.Count; i++) 
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
                    Vector3 pos = GetElipsePos(makePos, angle);
                    angle += 360 / instantiateCount * Mathf.Deg2Rad;
                    clone.Move(pos);
                }
            }
            t += Time.deltaTime * rotateSpeed;
        }

        if(t > rotateTime && isAttack == false)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        isAttack = true;

        WarningZone zone = Instantiate(warningZone, new Vector2(makePos.x, makePos.y - 4f), Quaternion.identity);
        zone.transform.localScale = new Vector2(width * 2.2f, height * 2.2f);
        zone.Marking(warningZoneFadeTime);
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < clones.Count; i++) 
        {
            SwordClone clone = clones[i];
            float x = UnityEngine.Random.Range(-0.5f, 0.5f);
            float y = UnityEngine.Random.Range(-0.5f, 0.5f);
            Vector3 targetPos = clone.transform.position - new Vector3(x, 4 + y, 0);
            clone.Attack(targetPos);
            yield return new WaitForSeconds(0.1f);
        }
    }

    //angle : radian
    private Vector2 GetElipsePos(Vector2 centerPos, float angle)
    {
        float cx = makePos.x;
        float cy = makePos.y;

        float width = this.width;
        float height = this.height;

        float theta = this.theta;

        float weight = angle;
        float x = cx + width * Mathf.Cos(t + weight);
        float y = cy + height * Mathf.Sin(t + weight);

        float dx = cx + (x - cx) * Mathf.Cos(theta) - (y - cy) * Mathf.Sin(theta);
        float dy = cy + (x - cx) * Mathf.Sin(theta) + (y - cy) * Mathf.Cos(theta);  
        
        return new Vector2( dx, dy );
    }

    private void Make()
    {
        clones.Clear();
        makeDone = false;
        isAttack = false;
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
            
            SwordClone clone = Instantiate(swordClone, GetElipsePos(makePos, angle), Quaternion.Euler(0,0,270));
            clone.Setting(dissolveTime, attackRadius, ownerTrm);

            clones.Add(clone);
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(delayTime);
        makeDone = true;
    }

    
}
