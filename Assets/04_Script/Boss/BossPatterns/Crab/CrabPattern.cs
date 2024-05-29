using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CrabPattern : BossPatternBase
{
    [SerializeField] private GameObject _instantWallWarningLB;
    [SerializeField] private GameObject _instantWallWarningRB;
    [SerializeField] private GameObject _instantWallWarningLM;

    [SerializeField] private GameObject _instantWallLB;
    [SerializeField] private GameObject _instantWallRB;
    [SerializeField] private GameObject _instantWallLM;

    [SerializeField] private GameObject _swingWarning;

    [SerializeField] private GameObject _leftWarningLine;
    [SerializeField] private GameObject _rightWarningLine;

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

    private void Update()
    {
        if(_boss.IsDie)
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator NipperLaserAttack()
    {
        _laserShooting = true;
        _boss.isAttacking = true;
        float animTime = 0.5f;
        float waitTime = 1f;

        StartCoroutine(MakeLaserInstantWall(waitTime, animTime));
        yield return new WaitForSeconds(waitTime);

        _boss.animator.SetBool(_boss.laserShooting, _laserShooting);

        while(_laserShooting)
        {
            if(_realLaserShoot)
            {
                Vector3 leftDir = _boss.leftGuidePos.position - _boss.leftFirePos.position;
                Vector3 rightDir = _boss.rightGuidePos.position - _boss.rightFirePos.position;

                MakeLine(lines[0], _boss.leftFirePos.position, leftDir, _laserWidth);
                PlayerCheckRay(_boss.leftFirePos.position, leftDir);
                MakeLine(lines[1], _boss.rightFirePos.position, rightDir, _laserWidth);
                PlayerCheckRay(_boss.rightFirePos.position, rightDir);
            }

            yield return null;
        }

        _instantWallLM.transform.DOScale(new Vector3(0, 30, 0), animTime).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                _instantWallLM.SetActive(false);
                _boss.isAttacking = false;
                _realLaserShoot = false;
            });
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

    private IEnumerator MakeLaserInstantWall(float waitTime, float makeTime)
    {
        _instantWallWarningLM.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        _instantWallWarningLM.SetActive(false);

        _instantWallLM.SetActive(true);
        CameraManager.Instance.CameraShake(5, makeTime);
        _instantWallLM.transform.DOScale(_instantWallWarningLM.transform.localScale, makeTime).SetEase(Ease.InOutSine);
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

    private void PlayerCheckRay(Vector3 startPos, Vector3 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(startPos, dir, int.MaxValue, LayerMask.GetMask("Player"));
        PlayerHP player;

        if (hit.collider != null)
        {
            if(hit.collider.gameObject.TryGetComponent<PlayerHP>(out player))
            {
                player.Hit(1);
            }
            else
            {
                return;
            }
        }
    }

    public void JointsLaserAnimation()
    {
        StartCoroutine(LeftJointsAnimation());
        StartCoroutine(RightJointsAnimation());
    }

    private IEnumerator LeftJointsAnimation(bool animationStop = false, float waitTime = 0.1f)
    {
        int leftjointCount = _boss.leftJoints.transform.childCount;

        for (int i = 0; i < leftjointCount; i++)
        {
            if (_boss.leftJoints.transform.GetChild(i).gameObject.activeSelf != false)
            {
                StartCoroutine(Blinking(_boss.leftJoints.transform.GetChild(i).gameObject, Color.white, 0.2f));

                yield return new WaitForSeconds(waitTime);
            }
        }

        if (animationStop)
        {
            if (!_boss.IsDie)
            {
                _swingWarning.SetActive(false);
                _boss.animator.speed = 1;
            }
        }
    }

    private IEnumerator RightJointsAnimation(bool animationStop = false, float waitTime = 0.1f)
    {
        int rightjointCount = _boss.rightJoints.transform.childCount;

        for (int i = 0; i < rightjointCount; i++)
        {
            if (_boss.rightJoints.transform.GetChild(i).gameObject.activeSelf != false)
            {
                StartCoroutine(Blinking(_boss.rightJoints.transform.GetChild(i).gameObject, Color.white, 0.2f));

                yield return new WaitForSeconds(waitTime);
            }
        }

        if (animationStop)
        {
            if (!_boss.IsDie)
            {
                _swingWarning.SetActive(false);
                _boss.animator.speed = 1;
            }
        }
    }

    public void CallRightJointsAnimation()
    {
        _boss.animator.speed = 0;
        _swingWarning.SetActive(true);
        StartCoroutine(RightJointsAnimation(true, 0.3f));
    }

    public void CallLeftJointsAnimation()
    {
        _boss.animator.speed = 0;
        _swingWarning.SetActive(true);
        StartCoroutine(LeftJointsAnimation(true, 0.3f));
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

    public void Shake()
    {
        CameraManager.Instance.CameraShake(5, 0.1f);
    }

    public IEnumerator BubbleAttack()
    {
        _bubbleShooting = true;
        _boss.isAttacking = true;
        float animTime = 0.5f;
        float waitTime = 1f;

        StartCoroutine(MakeBubbleInstantWall(waitTime, animTime));

        yield return new WaitForSeconds(waitTime);

        _boss.animator.SetBool(_boss.bubbleShooting, _bubbleShooting);

        while(_bubbleShooting)
        {
            if(_realBubbleShoot)
            {
                CameraManager.Instance.CameraShake(10, 0.25f);
                MakeBubbleBullet(300, 2, 10);
            }

            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(waitTime);

        _instantWallLB.transform.DOLocalMove(new Vector3(-28, 94, 0), animTime).SetEase(Ease.InOutSine);
        _instantWallRB.transform.DOLocalMove(new Vector3(28, 94, 0), animTime).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(animTime);

        _realBubbleShoot = false;
        _boss.isAttacking = false;
    }

    private IEnumerator MakeBubbleInstantWall(float waitTime, float makeTime)
    {
        _instantWallWarningLB.SetActive(true);
        _instantWallWarningRB.SetActive(true);

        yield return new WaitForSeconds(waitTime);

        _instantWallWarningLB.SetActive(false);
        _instantWallWarningRB.SetActive(false);

        _instantWallLB.transform.DOLocalMove(_instantWallWarningLB.transform.localPosition, makeTime).SetEase(Ease.InOutSine);
        _instantWallRB.transform.DOLocalMove(_instantWallWarningRB.transform.localPosition, makeTime).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(makeTime);

        CameraManager.Instance.CameraShake(10, 0.1f);
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

    private void MakeBubbleBullet(float angle, float minSpeed, float maxSpeed)
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.CrabBubbleBullet, _boss.bulletCollector.transform);
            bullet.transform.position = _boss.mouthFirePos.GetChild(i).position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            Vector3 dir = Quaternion.Euler(0, 0, UnityEngine.Random.Range(-angle, angle)) * Vector3.down;
            rigid.velocity = dir.normalized * UnityEngine.Random.Range(minSpeed, maxSpeed);
        }
    }

    public IEnumerator NipperPunch(GameObject nipper, GameObject joints, bool left = true)
    {
        if(left)
        {
            _boss.crabLeftNipper.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        else
        {
            _boss.crabRightNipper.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        _boss.isAttacking = true;
        _boss.animator.enabled = false;

        int count = joints.transform.childCount;
        List<Vector3> originSize = new ();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = joints.transform.GetChild(i).gameObject;
            if(obj.activeSelf != false)
            {
                originSize.Add(obj.transform.localScale);
                obj.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine);

                yield return new WaitForSeconds(0.2f);
            }
        }
        originSize.Add(nipper.transform.localScale);

        float curTime = 0;
        float warningTime = 2f;
        if (left)
        {
            _leftWarningLine.SetActive(true);
        }
        else
        {
            _rightWarningLine.SetActive(true);
        }
        
        Vector2 dir = Vector2.zero;
        while (curTime < warningTime)
        {
            curTime += Time.deltaTime;
            dir = GameManager.Instance.player.position - nipper.transform.position;
            float z;
            if(left)
            {
                z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                nipper.transform.DORotate(new Vector3(0, 0, z + 65), 0.2f).SetEase(Ease.InOutSine);
            }
            else
            {
                // 요고 한 번 정도 360도를 돌아버림 해결해라
                z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                nipper.transform.DORotate(new Vector3(0, 0, z - 235), 0.2f).SetEase(Ease.InOutSine);
            }

            yield return null;
        }


        StartCoroutine(Blinking(_boss.leftEye, _boss.eyesColor, 0.3f));
        StartCoroutine(Blinking(_boss.rightEye, _boss.eyesColor, 0.3f));
        _leftWarningLine.SetActive(false);
        _rightWarningLine.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        float speed = 120;
        Vector3 originPos = nipper.transform.position;
        
        while (true)
        {
            Collider2D hit = Physics2D.OverlapCircle(nipper.transform.position, 5, LayerMask.GetMask("Wall"));
            if (hit)
            {
                if(hit.gameObject.name == "CheckWall")
                {
                    CameraManager.Instance.CameraShake(10, 0.5f);
                    break;
                }
            }

            nipper.transform.position = Vector3.MoveTowards(nipper.transform.position, nipper.transform.position + (Vector3)dir, Time.deltaTime * speed);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        nipper.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                nipper.transform.position = originPos;
                if(left)
                {
                    nipper.transform.rotation = Quaternion.Euler(0, 0, 270);
                }
                else
                {
                    nipper.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
                nipper.SetActive(false);
            });

        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < originSize.Count; i++)
        {
            GameObject obj = joints.transform.GetChild(i).gameObject;
            obj.transform.DOScale(originSize[i], 0.5f).SetEase(Ease.InOutSine);

            yield return new WaitForSeconds(0.2f);
        }

        nipper.SetActive(true);

        nipper.transform.DOScale(originSize[originSize.Count - 1], 0.5f).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                if (left)
                {
                    _boss.crabLeftNipper.gameObject.layer = LayerMask.NameToLayer("Boss");
                }
                else
                {
                    _boss.crabRightNipper.gameObject.layer = LayerMask.NameToLayer("Boss");
                }
                _boss.isAttacking = false;
                _boss.animator.enabled = true;
            });
    }

    private IEnumerator Blinking(GameObject obj, Color changeColor, float blinkTime)
    {
        Color originColor = obj.GetComponent<SpriteRenderer>().color;

        obj.GetComponent<SpriteRenderer>().color = changeColor;

        yield return new WaitForSeconds(blinkTime);

        obj.GetComponent<SpriteRenderer>().color = originColor;
    }
}
