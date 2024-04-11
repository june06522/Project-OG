using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABossDeadState : BossBaseState
{
    private AltarBoss _altar;
    private Material _mat;
    private SpriteRenderer _spriteRenderer;
    private bool _make;

    public ABossDeadState(AltarBoss boss, AltarPattern pattern) : base(boss, pattern)
    {
        _altar = boss;
        _make = true;
    }

    public override void OnBossStateExit()
    {
        
    }

    public override void OnBossStateOn()
    {
        _altar.gameObject.layer = LayerMask.NameToLayer("Default");
        _spriteRenderer = _altar.bigestBody.GetComponent<SpriteRenderer>();
        _altar.mediumSizeBody.SetActive(false);
        _altar.smallestBody.SetActive(false);
        _spriteRenderer.sprite = _altar.dyingSprite;
        _altar.StopAllCoroutines();
        _altar.ReturnAll();
        _altar.ChainReturnAll();
        NowCoroutine(Dying(3, 2, 0.5f));
    }

    public override void OnBossStateUpdate()
    {
        
    }

    private IEnumerator Dying(float dyingTime, float disappearingTime, float disappearSpeed)
    {
        float curTime = 0;
        float a = 1;
        SoundManager.Instance.SFXPlay("Dead", _altar.deadClip, 1);
        //_altar.ChangeMaterial(_altar.dyingMat);
        _mat = _altar.dyingMat;
        _altar.StartCoroutine(MakeBullets());
        _mat.SetFloat("_VibrateFade", 1);
        yield return new WaitForSeconds(dyingTime);
        _make = false;
        GameObject effect = ObjectPool.Instance.GetObject(ObjectPoolType.AltarDeadEffect);
        effect.transform.position = _altar.transform.position;
        //_altar.ChangeMaterial(_altar.deadMat);
        _mat = _spriteRenderer.material;
        while (curTime < disappearingTime)
        {
            curTime += Time.deltaTime;
            if (a > 0)
            {
                _altar.gameObject.GetComponent<SpriteRenderer>().material.color = new Color(1, 1, 1, a -= Time.deltaTime * disappearSpeed);
                _mat.SetFloat("_FullDistortionFade", a);
            }
            yield return null;
        }
        _altar.gameObject.SetActive(false);
    }

    private IEnumerator MakeBullets()
    {
        PolygonCollider2D polygon = _altar.GetComponent<PolygonCollider2D>();
        List<GameObject> objList = new List<GameObject>();
        float maxX = 0;
        float minX = 0;
        float maxY = 0;
        float minY = 0;
        int listCount = 0;
        Vector2 a = Vector2.zero;
        Vector2 b = Vector2.zero;
        Vector2 c = Vector2.zero;

        if (polygon)
        {
            Vector2[] points = polygon.GetPath(0);
            if(points.Length >= 3)
            {
                Debug.Log("dd");
                a = points[0];
                b = points[1];
                c = points[2];

                maxX = a.x;
                minX = b.x;
                maxY = a.y;
                minY = c.y;
            }
        }
        while(_make)
        {
            // 랜덤 위치 문제?
            float randX = Random.Range(minX, maxX);
            float randY = Random.Range(minY, maxY);

            Vector3 randVec = new Vector2(randX, randY);

            if(Vector2.Dot(a, randVec) > 0 && Vector2.Dot(b, randVec) > 0 && Vector2.Dot(c, randVec) > 0)
            {
                objList.Add(ObjectPool.Instance.GetObject(ObjectPoolType.BossBulletType0, _altar.bulletCollector.transform));
                objList[listCount].transform.position = _altar.transform.position + randVec;
                objList[listCount].transform.rotation = Quaternion.identity;
                listCount++;
            }

            yield return null;
        }
        yield return null;
    }
}
