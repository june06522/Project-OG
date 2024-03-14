using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UIElements;

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
    private float instantiateCount = 6;
    [SerializeField]
    private float dissolveTime = 0.5f;
    [SerializeField]
    private float delayTime = 0.5f;

    [SerializeField]
    private float instantiateRadius = 2f;
    [SerializeField]
    private float attackRadius = 2f;

    [SerializeField]
    private LayerMask layerMask;

    [Header("Elipse")]
    [SerializeField] private float width;
    [SerializeField] private float height;

    List<SwordClone> clones;
    Transform ownerTrm;

    private bool makeDone = false;

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

            float cx = makePos.x;
            float cy = makePos.y;

            float width = this.width;
            float height = this.height;

            float theta = Mathf.Deg2Rad;

            clones.ForEach((clone) =>
            {
                float x = cx + width * Mathf.Cos(t);
                float y = cy + height * Mathf.Sin(t);

                float dx = cx + (x - cx) * Mathf.Cos(theta) - (y - cy) * Mathf.Sin(theta);
                float dy = cy + (x - cx) * Mathf.Sin(theta) + (y - cy) * Mathf.Cos(theta);

                clone.transform.position = new Vector2(dx, dy);
            });

            t += Time.deltaTime;
        }

        

    }

    private void Make()
    {
        clones.Clear();
        makeDone = false;
        StopAllCoroutines();
        StartCoroutine(MakeCoroutine());
     
    }
    Vector2 makePos;
    private IEnumerator MakeCoroutine()
    {
         makePos = ownerTrm.position;
        
        for (int i = 0; i < instantiateCount; i++)
        {
            float angle = 360 / instantiateCount * Mathf.Deg2Rad * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * instantiateRadius;
            
            SwordClone clone = Instantiate(swordClone, makePos + dir, Quaternion.Euler(0,0,270));
            clone.Setting(dissolveTime, attackRadius, ownerTrm);
            //clone.transform.localPosition = dir;

            clones.Add(clone);
            yield return new WaitForSeconds(delayTime);
        }

        yield return new WaitForSeconds(delayTime);
        makeDone = true;
    }

    
}
