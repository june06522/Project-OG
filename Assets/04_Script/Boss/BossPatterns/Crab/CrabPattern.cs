using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// Laser랑 Bubble은 연계기로 사용하자
public class CrabPattern : BossPatternBase
{
    [SerializeField] private CrabBoss _boss;

    private event Action _laserShotEnded;
    private event Action _swingEnded;
    private event Action _bubbleShotEnded;

    private bool _laserShooting;
    private bool _realLaserShoot;
    private bool _bubbleShooting;
    private bool _realBubbleShoot;

    private float _laserWidth;

    GameObject[] linePrefabs = new GameObject[2];
    LineRenderer[] lines = new LineRenderer[2];

    private void Awake()
    {
        _laserShotEnded += LaserShootingEnded;
        _swingEnded += SwingEnded;
        _bubbleShotEnded += BubbleShootingEnded;

        _laserShooting = false;
        _realLaserShoot = false;
        _bubbleShooting = false;
        _realBubbleShoot = false;

        _laserWidth = 1f;

        linePrefabs[0] = ObjectPool.Instance.GetObject(ObjectPoolType.CrabLaser, _boss.crabLaserCollector.transform);
        linePrefabs[1] = ObjectPool.Instance.GetObject(ObjectPoolType.CrabLaser, _boss.crabLaserCollector.transform);

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = linePrefabs[i].GetComponent<LineRenderer>();
        }
    }

    public IEnumerator NipperLaserAttack()
    {
        _laserShooting = true;
        _boss.isAttacking = true;
        _boss.animator.SetBool(_boss.laserShooting, _laserShooting);

        while(_laserShooting)
        {
            if(_realLaserShoot)
            {
                Vector3 leftDir = _boss.leftGuidePos.position - _boss.leftFirePos.position;
                Vector3 rightDir = _boss.rightGuidePos.position - _boss.rightFirePos.position;

                MakeLine(lines[0], _boss.leftFirePos.position, leftDir, _laserWidth);
                MakeLine(lines[1], _boss.rightFirePos.position, rightDir, _laserWidth);
            }

            yield return null;
        }

        _boss.isAttacking = false;
        _realLaserShoot = false;
    }

    public void LaserShot()
    {
        _realLaserShoot = true;
        MakeLine(lines[0], _boss.leftFirePos.position, Vector3.down, _laserWidth);
        MakeLine(lines[1], _boss.rightFirePos.position, Vector3.down, _laserWidth);
    }

    public void CallLaserShootingIsEnded()
    {
        _laserShotEnded?.Invoke();
    }

    public void CallSwingShootEnded()
    {
        _swingEnded?.Invoke();
    }

    private void LaserShootingEnded()
    {
        _laserShooting = false;
        lines[0].enabled = false;
        lines[1].enabled = false;
        _boss.animator.SetBool(_boss.laserShooting, _laserShooting);
    }

    private void MakeLine(LineRenderer line, Vector3 startPos, Vector3 dir, float width)
    {
        line.enabled = true;
        line.SetPosition(0, startPos);
        line.SetPosition(1, WallCheckRay(startPos, dir));
        SetLineWidth(line, width);
    }

    private void SetLineWidth(LineRenderer line, float width)
    {
        line.startWidth = width;
        line.endWidth = width;
    }

    private Vector3 WallCheckRay(Vector3 startPos, Vector3 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPos, dir, Mathf.Infinity, LayerMask.GetMask("Wall"));

        if (hit.point != null)
        {
            return hit.point;
        }

        return Vector3.zero;
    }

    public void SwingAttack()
    {
        _boss.isAttacking = true;
        _boss.animator.SetBool(_boss.swing, true);
    }

    public void LeftSwingShot()
    {
        MakeSwingBullet(_boss.leftFirePos.position, 10, 5);
    }

    public void RightSwingShot()
    {
        MakeSwingBullet(_boss.rightFirePos.position, 10, 5);
    }

    private void SwingEnded()
    {
        _boss.animator.SetBool(_boss.swing, false);
        _boss.isAttacking = false;
    }

    private void MakeSwingBullet(Vector3 firePos, float speed, float dirCount)
    {
        for(int i = 0; i < dirCount; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.CrabNormalBullet, _boss.bulletCollector.transform);
            bullet.transform.position = firePos;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * UnityEngine.Random.Range(0, 180) / 180), -Mathf.Sin(Mathf.PI * UnityEngine.Random.Range(0, 180) / 180));

            rigid.velocity = dir.normalized * speed;
        }
    }

    public IEnumerator BubbleAttack()
    {
        _bubbleShooting = true;
        _boss.isAttacking = true;
        _boss.animator.SetBool(_boss.bubbleShooting, _bubbleShooting);

        while(_bubbleShooting)
        {
            if(_realBubbleShoot)
            {
                MakeBubbleBullet(200, 2, 10, 3);
            }

            yield return new WaitForSeconds(0.1f);
        }

        _realBubbleShoot = false;
        _boss.isAttacking = false;
    }

    public void BubbleShot()
    {
        _realBubbleShoot = true;
    }

    private void BubbleShootingEnded()
    {
        _bubbleShooting = false;
        _boss.animator.SetBool(_boss.bubbleShooting, _bubbleShooting);
    }

    private void MakeBubbleBullet(float angle, float minSpeed, float maxSpeed, int burstCount)
    {
        for(int i = 0; i < burstCount; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.CrabBubbleBullet, _boss.bulletCollector.transform);
            bullet.transform.position = _boss.mouthFirePos.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-angle / 2, angle / 2)) * Vector3.down;
            rigid.velocity = dir.normalized * UnityEngine.Random.Range(minSpeed, maxSpeed);
        }
    }

    public IEnumerator NipperLeftPunch()
    {
        _boss.isAttacking = true;
        _boss.animator.enabled = false;

        int count = _boss.leftJoints.transform.childCount;
        List<Vector3> originSize = new ();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = _boss.leftJoints.transform.GetChild(i).gameObject;
            if(obj.activeSelf != false)
            {
                originSize.Add(obj.transform.localScale);
                obj.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine);

                yield return new WaitForSeconds(0.2f);
            }
        }
        originSize.Add(_boss.crabLeftNipper.transform.localScale);

        float curTime = 0;
        float warningTime = 1f;
        GameObject warningLine = ObjectPool.Instance.GetObject(ObjectPoolType.CrabWarningLine, _boss.leftFirePos);
        warningLine.transform.localPosition = Vector3.zero;
        Vector2 dir = Vector2.zero;
        while (curTime < warningTime)
        {
            curTime += Time.deltaTime;
            dir = GameManager.Instance.player.position - _boss.crabLeftNipper.transform.position;
            float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _boss.crabLeftNipper.transform.DORotate(new Vector3(0, 0, z + 30), 0.2f).SetEase(Ease.InOutSine);

            yield return null;
        }

        curTime = 0;
        float speed = 60;
        float animTime = 30 / speed;
        Vector3 originPos = _boss.crabLeftNipper.transform.position;

        while (curTime < animTime)
        {
            curTime += Time.deltaTime;
            _boss.crabLeftNipper.transform.position = Vector3.MoveTowards(_boss.crabLeftNipper.transform.position, _boss.crabLeftNipper.transform.position + (Vector3)dir, Time.deltaTime * speed);
            yield return null;
        }

        _boss.crabLeftNipper.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _boss.crabLeftNipper.SetActive(false);
            });

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < originSize.Count; i++)
        {
            GameObject obj = _boss.leftJoints.transform.GetChild(i).gameObject;
            obj.transform.DOScale(originSize[i], 0.5f).SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(0.2f);
        }

        _boss.crabLeftNipper.transform.position = originPos;
        _boss.crabLeftNipper.transform.rotation = Quaternion.identity;
        _boss.crabLeftNipper.SetActive(true);

        _boss.crabLeftNipper.transform.DOScale(originSize[originSize.Count - 1], 0.5f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _boss.isAttacking = false;
                _boss.animator.enabled = true;
            });
    }
}
