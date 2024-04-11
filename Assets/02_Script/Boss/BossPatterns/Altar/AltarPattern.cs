using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AltarPattern : BossPatternBase
{
    public IEnumerator OmnidirAttack(AltarBoss boss, int bulletCount, float speed, float time, int burstCount)
    {
        SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
        boss.bigestBody.transform.DORotate(new Vector3(0, 0, 180), 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                boss.bigestBody.transform.DORotate(Vector3.zero, 0.5f)
                .SetEase(Ease.OutQuad);
            });

        for (int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
                bullet.transform.position = boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(time);
        boss.isAttacking = false;
    }

    // 전방향으로 탄막을 날리고 잠시 뒤 탄막들이 플레이어 방향으로 날아간다 - 플레이어가 근접하기 좋은 패턴
    public IEnumerator OmniGuidPlayerAttack(AltarBoss boss, int bulletCount, float speed, float time, int burstCount)
    {
        SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
        GameObject[,] bullets = new GameObject[burstCount, bulletCount];

        boss.bigestBody.transform.DORotate(new Vector3(0, 0, -180), 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                boss.bigestBody.transform.DORotate(Vector3.zero, 0.5f)
                .SetEase(Ease.OutQuad);
            });

        for (int i = 0; i < burstCount; i++)
        {
            //boss.ChangeMaterial(boss.glitchMat);

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

            yield return new WaitForSeconds(time / 2);

            //boss.ChangeMaterial(boss.basicMat);

            yield return new WaitForSeconds(time / 2);

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

    // 플레이어 방향으로 에너지 볼을 던진다 - 플레이어가 근접하기 좋은 패턴
    public IEnumerator ThrowEnergyBall(AltarBoss boss, int burstCount, float speed, float waitTime, float returnTime)
    {
        Vector3 originSize = boss.transform.localScale;

        for (int i = 0; i < burstCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
            boss.smallestBody.transform.DOScale(originSize * 2, 0.2f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                boss.smallestBody.transform.DOScale(originSize, 0.2f)
                .SetEase(Ease.OutQuad);
            });

            GameObject energyBall = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
            energyBall.GetComponent<BossBullet>().Attack(boss.so.Damage);
            energyBall.transform.localScale *= 2;
            energyBall.transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y + 0.5f, boss.transform.position.z);
            energyBall.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = energyBall.GetComponent<Rigidbody2D>();
            Vector2 dir = GameManager.Instance.player.transform.position - energyBall.transform.position;
            rigid.velocity = dir.normalized * speed;

            yield return new WaitForSeconds(waitTime);
        }

        boss.isAttacking = false;
    }

    private bool rotate = true;
    // 총알을 전 방향으로 난사한다 - 플레이어가 멀어져야 좋은 패턴
    public IEnumerator OmnidirShooting(AltarBoss boss, int bulletCount, float speed, float time, int turnCount)
    {
        //boss.ChangeMaterial(boss.enchantedMat);

        rotate = true;
        StartCoroutine(RotateBigAndSmall(boss, 100));
        for (int i = 0; i < turnCount; i++)
        {
            SoundManager.Instance.SFXPlay("Fire", boss.fireClip, 1);
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, boss.bulletCollector.transform);
                bullet.GetComponent<BossBullet>().Attack(boss.so.Damage);
                bullet.transform.position = boss.transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount + i * 2), Mathf.Sin(Mathf.PI * 2 * j / bulletCount + i * 2));
                rigid.velocity = dir.normalized * speed;
            }
            yield return new WaitForSeconds(time);
        }

        rotate = false;
        //boss.ChangeMaterial(boss.basicMat);

        boss.bigestBody.transform.DORotate(Vector3.zero, 0.5f)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    boss.smallestBody.transform.DORotate(Vector3.zero, 0.5f)
                        .SetEase(Ease.OutQuad)
                        .OnComplete(() =>
                        {
                            boss.isStop = false;
                            boss.isAttacking = false;
                        });
                });
    }

    private IEnumerator RotateBigAndSmall(AltarBoss boss, float speed)
    {
        while(rotate)
        {
            boss.bigestBody.transform.Rotate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
            boss.smallestBody.transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * speed);
            yield return null;
        }
    }
}
