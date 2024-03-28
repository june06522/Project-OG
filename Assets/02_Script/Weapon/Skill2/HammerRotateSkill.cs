using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerRotateSkill : Skill
{
    [SerializeField] float rotateTime = 5f;
    [SerializeField] float dissolveTime = 0.5f;
    [SerializeField] float rotateSpeed = 20f;
    [SerializeField] float radius = 15f;

    [Header("Eclipse")]
    [SerializeField] float width;
    [SerializeField] float height;
    [SerializeField] float theta;

    [SerializeField] HammerClone hammerClone;
    public int hammerCount = 6;

    private List<HammerClone> clones;
    private float rotateTimer;

    private bool running;

    private void Awake()
    {
        clones = new List<HammerClone>(hammerCount);
    }

    private void OnEnable()
    {
        rotateTimer = 0;
        running = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Excute(null, null, 10);
        }

        if(running) 
        {
            bool endRotate = rotateTimer > rotateTime;

            float addRotateValue = rotateSpeed * Time.deltaTime;
            for(int i = 0; i < clones.Count; i++) 
            {
                if(endRotate)
                {
                    float reRotateValue = Mathf.Clamp(addRotateValue * 30, 0, 60);
                    clones[i].CurAngle -= reRotateValue;
                    Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clones[i].CurAngle * Mathf.Deg2Rad,
                                  width, height, this.theta);
                    clones[i].Move(pos, true);
                    clones.Remove(clones[i]);
                    i -= 1;
                    continue;
                }
                else
                {
                    clones[i].CurAngle += addRotateValue;
                    Vector3 pos = Eclipse.GetElipsePos(Vector2.zero, clones[i].CurAngle * Mathf.Deg2Rad,
                                  width, height, this.theta);
                    clones[i].Move(pos, false);
                }
            }

            if(endRotate)
            {
                running = false;
                clones.Clear();
            }

            rotateTimer += Time.deltaTime;
        }
    }

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        if(clones.Count > 0)
        {
            rotateTimer = 0;
            return;
        }

        rotateTimer = 0;
        // 스킬이 사용되고 있지 않는 상태면
        for(int i = 0; i < hammerCount; i++) 
        {
            float angle = 360 / hammerCount * i;

            Vector2 pos = Eclipse.GetElipsePos(Vector2.zero, angle * Mathf.Deg2Rad, this.width, this.height, this.theta);
            HammerClone clone = Instantiate(hammerClone, GameManager.Instance.player.transform);
            clone.transform.localPosition = pos;
            clone.transform.up = pos.normalized;

            clone.Init(rotateSpeed, dissolveTime, power, angle);
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
}
