using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRotateSkill : Skill
{
    [SerializeField] float minRotateTime = 5f;
    [SerializeField] float maxRotateTime = 15f;
    [SerializeField] float minRotateSpeed = 100f;
    [SerializeField] float maxRotateSpeed = 1000f;

    [SerializeField] float dissolveTime = 0.5f;
    [SerializeField] float damage = 10f;
    [SerializeField] int power = 1;

    [Header("Eclipse")]
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float theta;

    [SerializeField] HammerClone hammerClone;
    public int minHammerCount = 6;

    private List<HammerClone> clones;
    private float rotateTimer;

    private int curhammerCount;
    private float curhammerRotateSpeed;
    private float curhammerRotateTime;

    private float curWidth;
    private float curHeight;
    private float curTheta;

    private float curDamage;

    private bool running;

    private bool isFrozen;

    private Vector2 cloneScale;

    private float curPower;
    private void Awake()
    {
        clones = new List<HammerClone>(minHammerCount);
    }

    private void OnEnable()
    {
        rotateTimer = 0;
        running = false;
        curPower = -1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Excute(null, null, power);
        }

        if(running) 
        {
            bool endRotate = rotateTimer > curhammerRotateTime;

            float addRotateValue = curhammerRotateSpeed * Time.deltaTime;
            for(int i = 0; i < clones.Count; i++) 
            {
                if(endRotate)
                {
                    float reRotateValue = Mathf.Clamp(addRotateValue * 30, 0, 60);
                    clones[i].CurAngle -= reRotateValue;
                    Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clones[i].CurAngle * Mathf.Deg2Rad,
                                  curWidth, curHeight, curTheta);
                    clones[i].Move(pos, true);
                    clones.Remove(clones[i]);
                    i -= 1;
                    continue;
                }
                else
                {
                    clones[i].CurAngle += addRotateValue;
                    Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clones[i].CurAngle * Mathf.Deg2Rad,
                                  curWidth, curHeight, curTheta);
                    clones[i].Move(pos, false);
                }
            }

            if(endRotate)
            {
                curPower = -1f;
                running = false;
                clones.Clear();
            }

            rotateTimer += Time.deltaTime;
        }
    }

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        if(power > curPower)
        {
            if(clones != null)
            {
                for(int i = 0; i < clones.Count; i++)
                {
                    clones[i].DestroyThis();
                }
                clones.Clear();
                curPower = power;
            }
            CurPowerInit(power);
        }
        
        if(clones.Count > 0)
        {
            rotateTimer = 0;
            return;
        }

        rotateTimer = 0;
        // 스킬이 사용되고 있지 않는 상태면
        for(int i = 0; i < curhammerCount; i++) 
        {
            float angle = 360 / curhammerCount * i;

            Vector2 pos = Eclipse.GetElipsePos(Vector2.zero, angle * Mathf.Deg2Rad, curWidth, curHeight, curTheta);
            HammerClone clone = Instantiate(hammerClone, GameManager.Instance.player.transform);
            clone.transform.localPosition = pos;
            clone.transform.up = pos.normalized;

            clone.Init(curhammerRotateSpeed, dissolveTime, curDamage, angle, cloneScale, isFrozen, isMaxPower);
            clones.Add(clone);
        }

        StopCoroutine("SetRunning");
        StartCoroutine("SetRunning");
    }

    IEnumerator SetRunning()
    {
        yield return new WaitForSeconds(dissolveTime);
        running = true;
    }

    //Power Init
    public override void Power1()
    {
        curhammerCount = minHammerCount;
        curhammerRotateSpeed = minRotateSpeed;
        curhammerRotateTime = minRotateTime;
        curWidth = width;
        curHeight = height;
        curTheta = theta;
        curDamage = damage;
        cloneScale = Vector2.one;

        isFrozen = false;
        isMaxPower = false;
    }

    public override void Power2()
    {
        curhammerCount = minHammerCount + 1;
        curhammerRotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, 0.2f);
        curhammerRotateTime = Mathf.Lerp(minRotateTime, maxRotateTime, 0.2f);
        curWidth = width * 1.1f;
        curHeight = height * 1.1f;
        curTheta = theta;
        curDamage = damage;
        cloneScale = Vector2.one;

        isFrozen = false;
        isMaxPower = false;
    }

    public override void Power3()
    {
        curhammerCount = minHammerCount + 1;
        curhammerRotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, 0.25f);
        curhammerRotateTime = Mathf.Lerp(minRotateTime, maxRotateTime, 0.5f);
        curWidth = width * 1.3f;
        curHeight = height * 1.3f;
        curTheta = theta;
        curDamage = damage * 1.5f;
        cloneScale = Vector2.one * 1.5f;

        isFrozen = true;
        isMaxPower = false;
    }

    public override void Power4()
    {
        curhammerCount = minHammerCount + 3;
        curhammerRotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, 0.25f);
        curhammerRotateTime = Mathf.Lerp(minRotateTime, maxRotateTime, 0.5f);
        curWidth = width * 1.5f;
        curHeight = height * 1.5f;
        curTheta = theta;
        curDamage = damage * 2.5f;
        cloneScale = Vector2.one * 2f;

        isFrozen = true;
        isMaxPower = false;
    }

    public override void Power5()
    {
        curhammerCount = 3;
        curhammerRotateSpeed = Mathf.Lerp(minRotateSpeed, maxRotateSpeed, 0.2f);
        curhammerRotateTime = Mathf.Lerp(minRotateTime, maxRotateTime, 0.5f);
        curWidth = 2f;
        curHeight = 2f;
        curTheta = theta;

        curDamage = damage * 10f;
        cloneScale = Vector2.one * 5f;
        isFrozen = false;
        isMaxPower = true;
    }


}
