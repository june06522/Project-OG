using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPatternTest : MonoBehaviour
{
    // 미리 보스 패턴들 만들어 두고 나중에 필요한 것들만 들고가기 위한 스크립트
    // pooling은 걍 나중에 실제로 적용시킬 때 사용하자
    //https://blog.naver.com/kkodug9/220915991690 패턴 참고용 자료

    public LayerMask _mask;

    private Rigidbody2D _rigid;

    [SerializeField]
    private GameObject _bulletPrefab;
    [SerializeField]
    private GameObject _bouncingBulletPrefab;
    [SerializeField]
    private GameObject _wormBulletPrefab;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _warningCirclePrefab;
    [SerializeField]
    private GameObject _warningLinePrefab;
    [SerializeField]
    private GameObject _player;

    private bool _seven = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        if(_seven)
            _seven = false;
    }

    void Start()
    {
        //StartCoroutine(One(30, 2, 1, 2)); // 총알 개수, 총알 속도, 대기 시간, 발사 횟수
        //StartCoroutine(Two(6, 2, 0.2f, 100)); // 총알 개수, 총알 속도, 대기 시간, 회전 횟수
        //StartCoroutine(Three(4, 100, this.transform, 3)); // 총알 개수, 회전 속도, 부모 위치, 반지름
        //StartCoroutine(Four(4, 2, 3, 2)); // 총알 개수, 총알 속도, 대기 시간, 발사 횟수
        //StartCoroutine(Five(30, 5, 0.1f, 3)); // 총알 개수, 총알 속도, 대기 시간, 회전 횟수
        //StartCoroutine(Six(5, 2, 2, 3)); // 총알 개수, 총알 속도, 대기 시간, 발사 횟수
        //StartCoroutine(Seven(5, 5)); // 돌진 속도, 돌진 시간
        //StartCoroutine(Eight(6, 3, 1f, 5)); // 총알 개수, 총알 속도, 대기 시간, 반지름
        //StartCoroutine(Nine(5, 1, 5, -5, 2, -2)); // 폭탄 개수, 대기 시간, x축 최대 위치, x축 최소 위치, y축 최대 위치, y축 최소 위치
        //StartCoroutine(Ten(6, 1, 3, 2)); // 충격파 개수, 대기 시간, 총 충격파 횟수, 처음 반지름
        //StartCoroutine(Eleven(3, 1)); // 경고 선이 따라 다니는 시간, 레이저를 쏘기 위해 경고를 멈추고 모으는 시간
        //StartCoroutine(Twelve(7, 2, 1, 4)); // 총알 개수, 총알 속도, 방향이 바뀌는 시간, 방향 수 
        //StartCoroutine(Thirteen(100, 1, 0.2f)); // 총알 개수, 총알 속도, 총알 나오는 시간
        //StartCoroutine(Fourteen(3, 2, 1, 2)); // 총알 개수, 총알 속도, 총알 대기 시간, 점사 수
        //StartCoroutine(Fifteen(8, 3, 1.5f, 3)); // 총알 개수, 총알 속도, 대기 시간, 분산 횟수
        //StartCoroutine(Sixteen(10, 2, 0.5f, 3)); // 총알 개수, 총알 속도, 대기 시간, 바라보는 횟수
        //StartCoroutine(Seventeen(8, 3, 1, 5)); // 총알 개수, 총알 속도, 대기 시간, 점사 횟수
        //StartCoroutine(Eighteen(52, 2, 2, 1)); // 총알 개수, 총알 속도, 대기 시간, 점사 횟수
        //StartCoroutine(Nineteen(30, 2, 0.1f, 3)); // 총알 개수, 총알 속도, 대기 시간, 회전 횟수
        //StartCoroutine(Twenty(5, 3, 1f, 5)); // 총알 개수, 총알 속도, 대기 시간, 점사 횟수 (미완성)
    }

    private IEnumerator One(int bulletCount, float speed, float time, int burstCount) // 전방향으로 한 번에 총알 발사
    {
        for(int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;

                // 원기둥의 경우 총알이 발사 방향으로 회전
                //Vector3 rotation = Vector3.forward * 360 * i / bulletCount + Vector3.forward * 90;
                //bullet.transform.Rotate(rotation);
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Two(int bulletCount, float speed, float time, int turnCount) // 조금 씩 발사 방향이 바뀌면서 발사
    {
        for(int i = 0; i < turnCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / 5 + i * 2), Mathf.Sin(Mathf.PI * 2 * j / 5 + i * 2));
                rigid.velocity = dir.normalized * speed;

                // 원기둥의 경우 총알이 발사 방향으로 회전 <- 이거 회전이 이상함 (일단은 이 패턴은 원형 총알로만 하기)
                //Vector3 rotation = Vector3.forward * 360 * j / 5 + Vector3.forward * 90 - Vector3.forward * 10 * i;
                //bullet.transform.Rotate(rotation);
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Three(int bulletCount, float speed, Transform trans, float r) // 리메이크 전 아우솔 패시브 (주위를 도는 유성)
    {
        float deg = 0; // 각도
        GameObject[] bullets = new GameObject[bulletCount];

        for(int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(_bulletPrefab);
        }
        

        while (true)
        {
            deg += Time.deltaTime * speed;

            if (deg < 360)
            {
                for (int i = 0; i < bullets.Length; i++)
                {
                    var rad = Mathf.Deg2Rad * (deg + i * 360 / bullets.Length);
                    var x = r * Mathf.Cos(rad);
                    var y = r * Mathf.Sin(rad);
                    bullets[i].transform.position = trans.position + new Vector3(x, y);
                    bullets[i].transform.rotation = Quaternion.Euler(0, 0, (deg + i * 360 / bullets.Length) * -1);
                }
            }
            else
                deg = 0;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private IEnumerator Four(int bulletCount, float speed, float time, int burstCount) // 세 번째 패턴을 가진 총알을 여러 방향으로 날린다
    {
        for(int i = 0; i < burstCount; i++)
        {
            for(int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;

                StartCoroutine(Three(2, Random.Range(100, 500), bullet.transform, 2));
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Five(int bulletCount, float speed, float time, int end) // 회오리 치듯 원을 그리며 총알 발사
    {
        for(int i = 0; i < end; i++)
        {
            for(int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;

                // 원기둥의 경우 총알이 발사 방향으로 회전
                //Vector3 rotation = Vector3.forward * 360 * counting / bulletCount + Vector3.forward * 90;
                //bullet.transform.Rotate(rotation);

                yield return new WaitForSeconds(time);
            }
        }
        
    }

    private IEnumerator Six(int bulletCount, float speed, float time, int burstCount) // 총알이 여러 방향으로 날아가고 총알이 지나가면서 또 총알을 좌우로 발사
    {
        for(int i = 0; i < burstCount; i++)
        {
            for (int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid.velocity = dir.normalized * speed;

                StartCoroutine(SixCo(bullet.transform, speed / 2, dir));
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator SixCo(Transform trans, float speed, Vector2 vec)
    {
        GameObject[] bullets = new GameObject[2];

        Rigidbody2D[] rigids = new Rigidbody2D[2];

        while(true)
        {
            for (int i = 0; i < bullets.Length; i++)
            {
                bullets[i] = Instantiate(_bulletPrefab);
                bullets[i].transform.position = trans.position;
                bullets[i].transform.rotation = Quaternion.identity;
                rigids[i] = bullets[i].GetComponent<Rigidbody2D>();
            }

            if(vec == Vector2.right || vec == Vector2.left)
            {
                rigids[0].velocity = Vector2.up * speed;
                rigids[1].velocity = Vector2.down * speed;
            }
            else
            {
                rigids[0].velocity = Vector2.right * speed;
                rigids[1].velocity = Vector2.left * speed;
            }
            
            yield return new WaitForSeconds(1f);
        }
    } // 총알을 좌우로 발사하게 해주는 함수

    private IEnumerator Seven(float speed, float time) // 플레이어 방향으로 돌진
    {
        _seven = true;
        Vector2 dir = _player.transform.position - transform.position;
        _rigid.velocity = dir.normalized * speed;

        yield return new WaitForSeconds(time);

        _rigid.velocity = Vector2.zero;
    }

    private IEnumerator Eight(int bulletCount, float speed, float time, int r) // 전방향으로 총알을 만든 뒤 잠시 후 튕기는 총알 발사 
    {
        GameObject[] bullets = new GameObject[bulletCount];

        for(int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(_bouncingBulletPrefab);
            var rad = Mathf.Deg2Rad * i * 360 / bulletCount;
            var x = Mathf.Cos(rad);
            var y = Mathf.Sin(rad);
            bullets[i].transform.position = transform.position + new Vector3(x, y);
            bullets[i].transform.rotation = Quaternion.identity;
        }

        yield return new WaitForSeconds(time);

        for(int i = 0; i < bullets.Length; i++)
        {
            Rigidbody2D rigid = bullets[i].GetComponent<Rigidbody2D>();
            Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / bullets.Length), Mathf.Sin(Mathf.PI * 2 * i / bullets.Length));
            rigid.velocity = dir.normalized * speed;
        }
    }

    private IEnumerator Nine(int bombCount, float time, float maxX, float minX, float maxY, float minY) // 랜덤한 위치에 폭발
    {
        Vector2[] bombVecs = new Vector2[bombCount];
        GameObject[] warnings = new GameObject[bombCount];

        for(int i = 0; i < bombCount; i++)
        {
            GameObject warning = Instantiate(_warningCirclePrefab);
            warnings[i] = warning;
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            warning.transform.position = new Vector2(x, y);
            warning.transform.rotation = Quaternion.identity;
            bombVecs[i] = new Vector2(x, y);
        }

        yield return new WaitForSeconds(time);

        for(int i = 0; i < bombCount; i++)
        {
            Destroy(warnings[i]);
        }
        for(int i = 0; i < bombCount; i++)
        {
            GameObject bomb = Instantiate(_bulletPrefab);
            bomb.transform.position = bombVecs[i];
            bomb.transform.rotation = Quaternion.identity;
        }
    }

    private IEnumerator Ten(int shockCount, float time, int waveCount, float r) // 소울 나이트 충격파
    {
        GameObject[] shocks = new GameObject[shockCount];

        for(int i = 0; i < shockCount; i++)
        {
            shocks[i] = Instantiate(_bulletPrefab);
        }
        for(int i = 1; i <= waveCount; i++)
        {
            for (int j = 0; j < shockCount; j++)
            {
                var rad = Mathf.Deg2Rad * j * 360 / shockCount;
                var x = i * r * Mathf.Cos(rad);
                var y = i * r * Mathf.Sin(rad);
                shocks[j].transform.position = transform.position + new Vector3(x, y);
                shocks[j].transform.rotation = Quaternion.identity;
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Eleven(float followTime, float chargingTime) // 소울 나이트 레이저 - 플레이어를 따라다니는 경고 선이 있다가 잠시 후 레이저를 발사
    {
        float t = 0;
        float angle = 0;
        GameObject warning = Instantiate(_warningLinePrefab);
        warning.transform.position = transform.position;
        while(t < followTime)
        {
            angle = Mathf.Atan2(_player.transform.position.y - transform.position.y, _player.transform.position.x - transform.position.x) * Mathf.Rad2Deg; // 한 점까지의 각도를 구함(플레이어까지의 각도를 구함)
            warning.transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward); // angle 기준으로 각도를 회전

            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Quaternion rot = warning.transform.rotation;
        Destroy(warning);

        yield return new WaitForSeconds(chargingTime);

        GameObject laser = Instantiate(_laserPrefab);
        laser.transform.position = transform.position;
        laser.transform.rotation = rot;
    }

    private IEnumerator Twelve(int bulletCount, float speed, float time, int directionCount) // 여러 방향으로 총알을 가로로 일렬 여러 개씩 날린다
    {
        int bc = 0;
        if (bulletCount % 2 != 0)
            bc = bulletCount / 2 + 1;
        else
            bc = bulletCount / 2;

        for(int i = 0; i < directionCount; i++)
        {
            for(int j = -(bulletCount / 2); j < bc ; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / directionCount +  0.5f * j), Mathf.Sin(Mathf.PI * 2 * i / directionCount + 0.5f * j));
                rigid.velocity = dir.normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Thirteen(int bulletCount, float speed, float time) // 네 방향으로 지렁이 처럼 공격하는 패턴
    {
        Vector2 spawnVec = transform.position;

        for (int i = 0; i < bulletCount; i++)
        {
            for(int j = 0; j < 4; j++)
            {
                GameObject bullet = Instantiate(_wormBulletPrefab);
                bullet.transform.position = spawnVec;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / 4), Mathf.Sin(Mathf.PI * 2 * j / 4));
                rigid.velocity = dir.normalized * speed;
            }
            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Fourteen(int bulletCount, float speed, float time, int burstCount) // 플레이어 방향으로 점사 
    {
        int bc = 0;
        Vector2 dir = _player.transform.position - transform.position;

        if (bulletCount % 2 == 0)
            bc = bulletCount / 2;
        else
            bc = bulletCount / 2 + 1;

        for(int i = 0; i < burstCount; i++)
        {
            for(int j = -(bulletCount / 2); j < bc; j++)
            {
                Vector2 temp = dir;
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                temp += new Vector2(j, j);
                rigid.velocity = temp.normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Fifteen(int bulletCount, float speed, float time, int splitCount) // 엔터 더 건전 분산
    {
        List<GameObject> bulletList = new List<GameObject>();
        List<Transform> transList = new List<Transform>();
        GameObject beforeBullet;

        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bulletList.Add(bullet);
        beforeBullet = bullet;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.up * speed;

        yield return new WaitForSeconds(time);

        transList.Add(bulletList[0].transform);
        Destroy(bulletList[0]);
        bulletList.Clear();

        for(int i = 1; i < splitCount; i++)
        {
            for(int j = 0; j < transList.Count; j++)
            {
                for(int k = 0; k < bulletCount; k++)
                {
                    GameObject bulleT = Instantiate(_bulletPrefab);
                    bulleT.transform.position = transList[j].position;
                    bulleT.transform.rotation = Quaternion.identity;
                    bulleT.transform.localScale = new Vector3(beforeBullet.transform.localScale.x - beforeBullet.transform.localScale.x / (splitCount + 1),
                        beforeBullet.transform.localScale.y - beforeBullet.transform.localScale.y / (splitCount + 1));
                    bulletList.Add(bulleT);

                    Rigidbody2D rigid2d = bulleT.GetComponent<Rigidbody2D>();
                    Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * k / bulletCount), Mathf.Sin(Mathf.PI * 2 * k / bulletCount));
                    rigid2d.velocity = dir.normalized * speed;
                }
            }

            transList.Clear();
            beforeBullet = bulletList[0];

            yield return new WaitForSeconds(time);

            for(int j = 0; j < bulletList.Count; j++)
            {
                transList.Add(bulletList[j].transform);
                Destroy(bulletList[j]);
            }

            bulletList.Clear();
        }
    }

    private IEnumerator Sixteen(int bulletCount, float speed, float time, int watchCount) // 엔터 더 건전 개틀링 건 난사
    {
        for(int i = 0; i < watchCount; i++)
        {
            Vector2 vec = _player.transform.position - transform.position;
            for(int j = 0; j < bulletCount; j++)
            {
                float x = Random.Range(-1, 2);
                float y = Random.Range(-1, 2);

                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(vec.x + x, vec.y + y);
                rigid.velocity = dir.normalized * speed;

                yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Seventeen(int bulletCount, float speed, float time, int burstCount) // 엔터 더 건전 구불거리는 총알 여러 발 발사
    {
        for(int i = 0; i < burstCount; i++)
        {
            Vector2 vec = _player.transform.position - transform.position;
            float x = Random.Range(-3, 4);
            float y = Random.Range(-3, 4);
            for(int j = 0; j < bulletCount; j++)
            {
                GameObject bullet = Instantiate(_wormBulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(vec.x + x, vec.y + y);
                rigid.velocity = dir.normalized * speed;

                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Eighteen(int bulletCount, float speed, float time, int burstCount) // 엔터 더 건전 기어모양으로 전방향에 총알 발사
    {
        bool plus = true;
        float r = 1;
        for (int i = 0; i < burstCount; i++)
        {
            for(int j = 0; j < bulletCount; j++)
            {
                if (r > 1.1f)
                    plus = false;
                else if (r == 1)
                    plus = true;
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                Vector2 dir = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount)) * r;
                rigid.velocity = dir * speed;

                if (plus)
                    r += 0.1f;
                else
                    r -= 0.1f;
            }

            yield return new WaitForSeconds(time);
        }
    }

    private IEnumerator Nineteen(int bulletCount, float speed, float time, int turnCount) // 엔터 더 건전 회오리 샷 근데 양쪽에서 총알이 나옴
    {
        for(int i = 0; i < turnCount; i++)
        {
            for(int j = 0; j < bulletCount; j++)
            {
                GameObject bullet1 = Instantiate(_bulletPrefab);
                GameObject bullet2 = Instantiate(_bulletPrefab);
                bullet1.transform.position = transform.position;
                bullet1.transform.rotation = Quaternion.identity;
                bullet2.transform.position = transform.position;
                bullet2.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
                Vector2 dir1 = new Vector2(Mathf.Cos(Mathf.PI * 2 * j / bulletCount), Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                Rigidbody2D rigid2 = bullet2.GetComponent<Rigidbody2D>();
                Vector2 dir2 = new Vector2(-Mathf.Cos(Mathf.PI * 2 * j / bulletCount), -Mathf.Sin(Mathf.PI * 2 * j / bulletCount));
                rigid1.velocity = dir1.normalized * speed;
                rigid2.velocity = dir2.normalized * speed;

                yield return new WaitForSeconds(time);
            }
        }
    }

    private IEnumerator Twenty(int bulletCount, float speed, float time, int burstCount) // 엔터 더 건전 플레이어 방향으로 총알 여러 발 발사
    {
        int bc = 0;

        if (bulletCount % 2 == 0)
            bc = bulletCount / 2;
        else
            bc = bulletCount / 2 + 1;

        for (int i = 0; i < burstCount; i++)
        {
            Vector2 vec = _player.transform.position - transform.position;

            for (int j = -(bulletCount / 2); j < bc; j++)
            {
                GameObject bullet = Instantiate(_bulletPrefab);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.velocity = new Vector2(vec.x, vec.y).normalized * speed;
            }

            yield return new WaitForSeconds(time);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(_seven)
        {
            Vector2 dir = _rigid.velocity;

            RaycastHit2D rHit = Physics2D.Raycast(transform.position, Vector2.right, 0.55f, _mask);
            RaycastHit2D lHit = Physics2D.Raycast(transform.position, Vector2.left, 0.55f, _mask);
            RaycastHit2D uHit = Physics2D.Raycast(transform.position, Vector2.up, 0.55f, _mask);
            RaycastHit2D dHit = Physics2D.Raycast(transform.position, Vector2.down, 0.55f, _mask);

            if ((rHit.collider != null || lHit.collider != null) && (uHit.collider != null || dHit.collider != null))
                _rigid.velocity = new Vector2(-dir.x, -dir.y);
            else if (rHit.collider != null || lHit.collider != null)
                _rigid.velocity = new Vector2(-dir.x, dir.y);
            else if (uHit.collider != null || dHit.collider != null)
                _rigid.velocity = new Vector2(dir.x, -dir.y);
        }
    }
}
