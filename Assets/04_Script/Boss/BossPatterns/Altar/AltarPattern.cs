using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AltarPattern : BossPatternBase
{
    private IEnumerator OutTriangleAnim(AltarBoss boss, float time, float a)
    {
        StartCoroutine(boss.Blinking(boss.smallestBody, time, a, 1, Color.white));
        yield return new WaitForSeconds(time);
        StartCoroutine(boss.Blinking(boss.mediumSizeBody, time, a, 1, Color.white));
        yield return new WaitForSeconds(time);
        StartCoroutine(boss.Blinking(boss.bigestBody, time, a, 1, Color.white));
    }

    private IEnumerator InTriangleAnim(AltarBoss boss, float time, float a)
    {
        StartCoroutine(boss.Blinking(boss.bigestBody, time, a, 1, Color.white));
        yield return new WaitForSeconds(time);
        StartCoroutine(boss.Blinking(boss.mediumSizeBody, time, a, 1, Color.white));
        yield return new WaitForSeconds(time);
        StartCoroutine(boss.Blinking(boss.smallestBody, time, a, 1, Color.white));
    }

    public IEnumerator OmniGuidPlayerAttack(AltarBoss boss, int bulletCount, float speed, float time, int burstCount)
    {
        float animTime = 0.5f;
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        StartCoroutine(OutTriangleAnim(boss, animTime / 3, 1));
        boss.bigestBody.transform.DORotate(new Vector3(0, 0, -180), animTime)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                boss.bigestBody.transform.DORotate(Vector3.zero, animTime)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    StartCoroutine(boss.Poping(boss.bigestBody, animTime, 1.5f));
                    StartCoroutine(boss.Blinking(boss.gameObject, animTime, 0.5f, 1, Color.white, boss.bigTriangleSprite, true));
                });
            });

        yield return new WaitForSeconds(animTime * 2);

        for (int i = 0; i < burstCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
            for (int j = 0; j < bulletCount; j++)
            {
                bullets[i, j] = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullets[i, j].GetComponent<BossBullet>().Attack(boss.so.Damage);
                bullets[i, j].transform.position = boss.transform.position;
                bullets[i, j].transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                rigid.velocity = Vector2.zero;
            }

            yield return new WaitForSeconds(Time.deltaTime);

            Vector3 nextDir = GameManager.Instance.player.transform.position;

            for (int j = 0; j < bulletCount; j++)
            {
                Rigidbody2D rigid = bullets[i, j].GetComponent<Rigidbody2D>();
                Vector2 dir = nextDir - bullets[i, j].transform.position;
                rigid.velocity = dir.normalized * speed * 2;
            }
        }

        yield return new WaitForSeconds(time);
        boss.isAttacking = false;
    }

    public IEnumerator Dash(AltarBoss boss, float maxDistance, float speed, float rotateSpeed, float shakeTime, float waitTime)
    {
        boss.isBlocked = false;
        boss.isDashing = true;
        Vector3 dir = (GameManager.Instance.player.transform.position - boss.transform.position).normalized;
        float deg = 0;
        GameObject lineObj = ObjectPool.Instance.GetObject(ObjectPoolType.Laser, boss.altarCollector.transform);
        LineRenderer line = lineObj.GetComponent<LineRenderer>();

        line.SetPosition(0, transform.position);
        line.startWidth = 3f;
        ShowLineRenderer(transform.position, line, dir, 3);

        yield return new WaitForSeconds(0.3f);

        line.enabled = false;

        SoundManager.Instance.SFXPlay("Dash", boss.dashClip);

        while (!boss.isBlocked)
        {
            deg += Time.deltaTime * rotateSpeed;

            if(deg < 360)
            {
                boss.transform.rotation = Quaternion.Euler(0, 0, deg);
            }
            else
            {
                deg = 0;
            }

            Vector3 target = boss.transform.position + dir * maxDistance;
            boss.transform.position = Vector2.MoveTowards(boss.transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
        CameraManager.Instance.CameraShake(10, shakeTime);
        float curTime = 0;
        float knockBackPower = 1.5f;

        boss.SetBody(boss.gameObject, Vector3.one, Vector3.zero, Color.white, 0.5f, false);
        while(curTime < 1)
        {
            curTime += Time.deltaTime;

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, boss.transform.position + (-dir), Time.deltaTime * knockBackPower);
            yield return null;
        }

        boss.isStop = false;
        boss.isDashing = false;
        yield return new WaitForSeconds(waitTime);
        boss.isAttacking = false;
    }

    public void MakeIntantWall(AltarBoss boss, float moveTime, float time, float warningTime)
    {
        int rand = Random.Range(1, 4);

        switch(rand)
        {
            case 1:
                StartCoroutine(InstantWallOne(boss, moveTime, time, warningTime));
                break;
            case 2:
                StartCoroutine(InstantWallTwo(boss, moveTime, time, warningTime));
                break;
            case 3:
                StartCoroutine(InstantWallThree(boss, moveTime, time, warningTime));
                break;
        }
    }
    
    private IEnumerator InstantWallOne(AltarBoss boss, float moveTime, float time, float warningTime)
    {
        Teleport(boss, Vector3.zero, warningTime);

        Vector3 originPos1 = boss.IW_1_1.transform.localPosition;
        Vector3 originPos2 = boss.IW_1_2.transform.localPosition;

        boss?.IWW_1_1?.gameObject.SetActive(true);
        boss?.IWW_1_2?.gameObject.SetActive(true);

        yield return new WaitForSeconds(warningTime);

        boss?.IWW_1_1?.gameObject.SetActive(false);
        boss?.IWW_1_2?.gameObject.SetActive(false);

        CameraManager.Instance.CameraShake(5, 0.5f);
        boss?.IW_1_1?.transform.DOLocalMove(new Vector3(-29, 18), moveTime).SetEase(Ease.InOutSine);
        boss?.IW_1_2?.transform.DOLocalMove(new Vector3(22, -27), moveTime).SetEase(Ease.InOutSine);

        boss.isAttacking = false;

        yield return new WaitForSeconds(time);

        boss?.IW_1_1?.transform.DOLocalMove(originPos1, moveTime).SetEase(Ease.InOutSine);
        boss?.IW_1_2?.transform.DOLocalMove(originPos2, moveTime).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(moveTime);

        boss.isIW = false;
    }

    private IEnumerator InstantWallTwo(AltarBoss boss, float moveTime, float time, float warningTime)
    {
        Teleport(boss, Vector3.zero, warningTime);


        Vector3 originPos1 = boss.IW_2_1.transform.localPosition;
        Vector3 originPos2 = boss.IW_2_2.transform.localPosition;

        boss?.IWW_2_1?.gameObject.SetActive(true);
        boss?.IWW_2_2?.gameObject.SetActive(true);

        yield return new WaitForSeconds(warningTime);

        boss?.IWW_2_1?.gameObject.SetActive(false);
        boss?.IWW_2_2?.gameObject.SetActive(false);

        CameraManager.Instance.CameraShake(5, 0.5f);
        boss?.IW_2_1?.transform.DOLocalMove(new Vector3(-29, 18), moveTime).SetEase(Ease.InOutSine);
        boss?.IW_2_2?.transform.DOLocalMove(new Vector3(0, 26), moveTime).SetEase(Ease.InOutSine);

        boss.isAttacking = false;

        yield return new WaitForSeconds(time);

        boss?.IW_2_1?.transform.DOLocalMove(originPos1, moveTime).SetEase(Ease.InOutSine);
        boss?.IW_2_2?.transform.DOLocalMove(originPos2, moveTime).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(moveTime);

        boss.isIW = false;
    }

    private IEnumerator InstantWallThree(AltarBoss boss, float moveTime, float time, float warningTime)
    {
        Teleport(boss, Vector3.zero, warningTime);


        Vector3 originPos1 = boss.IW_3_1.transform.localPosition;

        boss?.IWW_3_1?.gameObject.SetActive(true);

        yield return new WaitForSeconds(warningTime);

        boss?.IWW_3_1?.gameObject.SetActive(false);

        CameraManager.Instance.CameraShake(5, 0.5f);
        boss?.IW_3_1?.transform.DOLocalMove(new Vector3(-31, -13), moveTime).SetEase(Ease.InOutSine);

        boss.isAttacking = false;

        yield return new WaitForSeconds(time);

        boss?.IW_3_1?.transform.DOLocalMove(originPos1, moveTime).SetEase(Ease.InOutSine);

        yield return new WaitForSeconds(moveTime);

        boss.isIW = false;
    }

    private void Teleport(AltarBoss boss, Vector3 pos, float teleportAnimTime)
    {
        boss?.transform.DOScale(Vector3.zero, teleportAnimTime / 2).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                boss.transform.localPosition = pos;
                boss?.transform.DOScale(Vector3.one, teleportAnimTime / 2).SetEase(Ease.InOutSine);
            });
    }

    //public IEnumerator OmnidirAttack(AltarBoss boss, int bulletCount, float speed, float ShotCoolTime, int burstCount)
    //{
    //    float animTime = 0.5f;

    //    StartCoroutine(OutTriangleAnim(boss, animTime / 3, 1));
    //    boss.bigestBody.transform.DORotate(new Vector3(0, 0, 180), animTime)
    //        .SetEase(Ease.OutQuad)
    //        .OnComplete(() =>
    //        {
    //            boss.bigestBody.transform.DORotate(Vector3.zero, animTime)
    //            .SetEase(Ease.OutQuad)
    //            .OnComplete(() =>
    //            {
    //                StartCoroutine(boss.Poping(boss.bigestBody, animTime, 1.5f));
    //                StartCoroutine(boss.Blinking(boss.gameObject, animTime, 0.5f, 1, Color.white, boss.bigTriangleSprite, true));
    //            });
    //        });

    //    yield return new WaitForSeconds(animTime * 2);

    //    for (int i = 0; i < burstCount; i++)
    //    {
    //        SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
    //        for (int j = 0; j < bulletCount; j++)
    //        {
    //            GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
    //            bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
    //            bullet.transform.position = boss.transform.position;
    //            bullet.transform.rotation = Quaternion.identity;

    //            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
    //            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
    //            rigid.velocity = dir.normalized * speed;
    //        }

    //        yield return new WaitForSeconds(ShotCoolTime);
    //    }

    //    yield return new WaitForSeconds(ShotCoolTime);
    //    boss.isAttacking = false;
    //}



    //// 플레이어 방향으로 에너지 볼을 던진다 - 플레이어가 근접하기 좋은 패턴
    //public IEnumerator ThrowEnergyBall(AltarBoss boss, int burstCount, float speed, float waitTime, float returnTime)
    //{
    //    float animTime = 0.5f;
    //    Vector3 originSize = boss.transform.localScale;

    //    for (int i = 0; i < burstCount; i++)
    //    {

    //        StartCoroutine(InTriangleAnim(boss, animTime / 3, 1));

    //        boss.smallestBody.transform.DOScale(originSize * 2, animTime)
    //        .SetEase(Ease.OutQuad)
    //        .OnComplete(() =>
    //        {
    //            boss.smallestBody.transform.DOScale(originSize, animTime)
    //            .SetEase(Ease.OutQuad);
    //        });

    //        yield return new WaitForSeconds(animTime / 1.5f);
    //        SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);

    //        GameObject energyBall = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
    //        energyBall.GetComponent<BossBullet>().Attack(boss.so.Damage);
    //        energyBall.transform.localScale *= 2;
    //        energyBall.transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y + 0.5f, boss.transform.position.z);
    //        energyBall.transform.rotation = Quaternion.identity;

    //        Rigidbody2D rigid = energyBall.GetComponent<Rigidbody2D>();
    //        Vector2 dir = GameManager.Instance.player.transform.position - energyBall.transform.position;
    //        rigid.velocity = dir.normalized * speed;

    //        yield return new WaitForSeconds(waitTime);
    //    }

    //    boss.isAttacking = false;
    //}

    //private bool rotate = true;
    //// 총알을 전 방향으로 난사한다 - 플레이어가 멀어져야 좋은 패턴
    //public IEnumerator OmnidirShooting(AltarBoss boss, int bulletCount, float speed, float time, int turnCount)
    //{
    //    rotate = true;
    //    StartCoroutine(RotateBigAndSmall(boss, 50, 0.5f));
    //    for (int i = 0; i < turnCount * 2; i += 2)
    //    {
    //        SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
    //        StartCoroutine(OutTriangleAnim(boss, 0.5f, 1));
    //        for (int j = 0; j < bulletCount * 2; j++)
    //        {
    //            if(j % 2 != 0)
    //            {
    //                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
    //                bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
    //                bullet.transform.position = boss.transform.position;
    //                bullet.transform.rotation = Quaternion.identity;

    //                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
    //                Vector2 dir = new Vector2(Mathf.Sin(Mathf.PI * 2 * j / (bulletCount * 2) + i * 2), Mathf.Cos(Mathf.PI * 2 * j / (bulletCount * 2) + i * 2));
    //                rigid.velocity = dir.normalized * speed;
    //            }
    //        }
    //        yield return new WaitForSeconds(time);
    //    }

    //    rotate = false;

    //    boss.bigestBody.transform.DORotate(Vector3.zero, 0.5f)
    //            .SetEase(Ease.OutQuad)
    //            .OnComplete(() =>
    //            {
    //                boss.smallestBody.transform.DORotate(Vector3.zero, 0.5f)
    //                    .SetEase(Ease.OutQuad)
    //                    .OnComplete(() =>
    //                    {
    //                        boss.isStop = false;
    //                        boss.isAttacking = false;
    //                    });
    //            });
    //}

    //private IEnumerator RotateBigAndSmall(AltarBoss boss, float speed, float time = 0)
    //{
    //    yield return new WaitForSeconds(time);

    //    while(rotate)
    //    {
    //        boss.bigestBody.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
    //        boss.smallestBody.transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * speed);
    //        yield return null;
    //    }
    //}
}
