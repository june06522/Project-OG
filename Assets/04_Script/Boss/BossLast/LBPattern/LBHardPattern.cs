using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBHardPattern : LBRandomPattern
{

    private Transform _target;

    Sequence _seq;
    Sequence _seq2;

    [Header("Sound")]
    [SerializeField] AudioClip _dangerClip;
    [SerializeField] AudioClip _shotLaserClip;
    [SerializeField] AudioClip _shotBulletClip;

    [Header("World")]
    [SerializeField] private Transform _worldCollider;

    #region Laser
    [Header("Laser")]
    [SerializeField]
    private Transform _rootLaserTrm;

    [SerializeField]
    private Transform _laserX;
    [SerializeField]
    private Transform _laserY;

    [SerializeField]
    private EnemyBullet _enemyBullet;

    private SpriteRenderer _laserXSprite;
    private SpriteRenderer _laserYSprite;

    [Header("Danger Space")]
    [SerializeField] private SpriteRenderer _bulletDangerSpaceLeftUp;
    [SerializeField] private SpriteRenderer _bulletDangerSpaceLeftDown;
    [SerializeField] private SpriteRenderer _bulletDangerSpaceRightUp;
    [SerializeField] private SpriteRenderer _bulletDangerSpaceRightDown;

    private TouchDamageObject _laserXDamage;
    private TouchDamageObject _laserYDamage;

    private float _dashDelayTime = 1.2f;
    private float _laserAwakeTime = 0.51f;
    private float _laserDelayTime = 1.0f;
    private float _laserDisappearTime = 0.25f;
    private float _laserPatternEndDelay = 0.5f;

    WaitForSeconds _wfsDashDelay;
    WaitForSeconds _wfsLaserAwake;
    WaitForSeconds _wfsLaserDelay;
    WaitForSeconds _wfsBulletDanger = new WaitForSeconds(0.5f);

    WaitForSeconds _wfsBulletDelay = new WaitForSeconds(0.05f);

    #endregion

    #region ManyLaser
    [Header("Many Laser")]
    [SerializeField] private GameObject _laserObject;
    List<GameObject> _spawnLaserObjects = new List<GameObject>();
    Vector2[] _dirVec = new Vector2[4] { Vector2.left, Vector2.up, Vector2.right, Vector2.down };

    [Header("Left, Up, Right, Down")]
    [SerializeField]
    List<GameObject> _dirLaserDangerObject = new List<GameObject>();


    WaitForSeconds _wfs04Delay = new WaitForSeconds(0.4f);
    WaitForSeconds _wfs2Delay = new WaitForSeconds(2f);
    #endregion

    private void Awake()
    {

        _target = GameManager.Instance.player;
        // Laser
        _laserXSprite = _laserX.GetComponent<SpriteRenderer>();
        _laserYSprite = _laserY.GetComponent<SpriteRenderer>();

        _laserXDamage = _laserX.GetComponent<TouchDamageObject>();
        _laserYDamage = _laserY.GetComponent<TouchDamageObject>();

        _wfsDashDelay = new WaitForSeconds(_dashDelayTime);
        _wfsLaserAwake = new WaitForSeconds(_laserAwakeTime);
        _wfsLaserDelay = new WaitForSeconds(_laserDelayTime);

        RegisterPattern(DashLaser);
        RegisterPattern(ManyLaserShot);

    }

    private void DashLaser()
    {

        StartCoroutine(DashLaserCo());

    }

    IEnumerator DashLaserCo()
    {
        _seq = DOTween.Sequence();

        // Tweening 
        _seq.Append(_boss.transform.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.InBounce));
        _seq.Append(_boss.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBounce));
        _seq.Append(_boss.transform.DOScale(Vector3.one * 1f, 0.1f).SetEase(Ease.OutCubic));
        _seq.Append(_boss.transform.DOMove(_target.transform.position, 0.5f).SetEase(Ease.OutCubic));

        yield return _wfsDashDelay;

        bool isDiagonal = Random.Range(0, 2) == 0;
        float angle = isDiagonal ? 45f : 0f;
        _rootLaserTrm.eulerAngles = new Vector3(0, 0, angle);
         
        _seq.Append(_laserX.DOScaleY(0.1f, 0.1f).SetEase(Ease.OutBack))
             .Join(_laserY.DOScaleX(0.1f, 0.1f).SetEase(Ease.OutBack));

        SoundManager.Instance.SFXPlay("Danger", _dangerClip);

        yield return _wfsLaserAwake;

        _laserXDamage.SetOnOff(true);
        _laserYDamage.SetOnOff(true);

        // Tweening
        _seq.Append(_laserX.DOScaleY(1, 0.5f).SetEase(Ease.OutElastic))
            .Join(_laserY.DOScaleX(1, 0.5f).SetEase(Ease.OutElastic));

        SoundManager.Instance.SFXPlay("ShotLaser", _shotLaserClip);


        // 총알 난사
        bool shotLeftUpAndRightDown = Random.Range(0, 2) == 0;
        if (shotLeftUpAndRightDown == false)
        {
            angle += 90f;
        }

        for(int i = 0; i < 3; ++i)
        {
            SoundManager.Instance.SFXPlay("Danger", _dangerClip);

            if (shotLeftUpAndRightDown)
            {

                _bulletDangerSpaceLeftDown.gameObject.SetActive(true);
                _bulletDangerSpaceRightUp.gameObject.SetActive(true); 

            }
            else
            {

                _bulletDangerSpaceLeftUp.gameObject.SetActive(true);
                _bulletDangerSpaceRightDown.gameObject.SetActive(true);

            }
            yield return _wfsBulletDelay;

            if (shotLeftUpAndRightDown)
            {
                
                _bulletDangerSpaceLeftDown.gameObject.SetActive(false);
                _bulletDangerSpaceRightUp.gameObject.SetActive(false);

            }
            else
            {

                _bulletDangerSpaceLeftUp.gameObject.SetActive(false);
                _bulletDangerSpaceRightDown.gameObject.SetActive(false);

            }
            yield return _wfsBulletDelay;

        }

        yield return _wfsBulletDanger;

        float angle2 = angle + 180f;

        for(int i = 0; i < 20; ++i)
        {
            
            SoundManager.Instance.SFXPlay("ShotBullet", _shotBulletClip);

            float randomAngle = Random.Range(angle, angle + 90f) % 360;
            ShotBullet(randomAngle);

            randomAngle = Random.Range(angle2, angle2 + 90f) % 360;
            ShotBullet(randomAngle);

            yield return _wfsBulletDelay;
        }

        yield return _wfsLaserDelay;

        _seq2 = DOTween.Sequence();
        _seq2.Append(_laserX.DOScaleY(0, _laserDisappearTime).SetEase(Ease.OutElastic))
              .Join(_laserY.DOScaleX(0, _laserDisappearTime).SetEase(Ease.OutElastic));

        _seq2.OnComplete(() =>
        {

            _laserXDamage.SetOnOff(false);
            _laserYDamage.SetOnOff(false);

            FAED.InvokeDelay(() => { _isEnd = true; }, _laserPatternEndDelay);

        });


    }

    private void ShotBullet(float degree_angle)
    {

        float radAngle = degree_angle * Mathf.Deg2Rad;

        EnemyBullet bullet = Instantiate(_enemyBullet, _boss.transform.position, Quaternion.identity);
        bullet.Shoot(new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)));

    }

    private void ManyLaserShot()
    {

        // 방향 계산
        StartCoroutine(ManyLaserShotCo());


    }

    IEnumerator ManyLaserShotCo()
    {

        for(int i = 0; i < 6; ++i)
        {

            yield return _wfs04Delay;
            int dirType = Random.Range(0, 4); // 0 ~ 3
            Vector3 dir = _dirVec[dirType];
            GameObject dirDangerObj = _dirLaserDangerObject[dirType];

            // Danger
            dirDangerObj.SetActive(true);
            SoundManager.Instance.SFXPlay("Danger", _dangerClip);


            yield return _wfs04Delay;
            // Danger Off
            dirDangerObj.SetActive(false);


            // Shot Laser
            GameObject laserObj = Instantiate(_laserObject);
            SoundManager.Instance.SFXPlay("ShotLaser", _shotLaserClip);


            _seq = DOTween.Sequence();
            if(dirType == 1 || dirType == 3)
            {

                laserObj.transform.eulerAngles = new Vector3(0, 0, 90);
                laserObj.transform.position = _boss.transform.position + dir * 15f;
                _seq.SetEase(Ease.Linear);
                _seq.Append(laserObj.transform.DOMove(_boss.transform.position + dir * -15f, 1.5f).SetEase(Ease.Linear)); //30

            }
            else
            {

                laserObj.transform.position = _boss.transform.position + dir * 25f;
                _seq.Append(laserObj.transform.DOMove(_boss.transform.position + dir * -25f, 2.5f).SetEase(Ease.Linear)); //50


            }

            _spawnLaserObjects.Add(laserObj);
            _seq.AppendCallback(() =>
            {
                _spawnLaserObjects.Remove(laserObj);
                Destroy(laserObj);
            });

        }

        yield return _wfs2Delay;

        _isEnd = true;
    }

    public override void OnPattern()
    {
        //
        _isEnd = true;
        _worldCollider.localPosition = new Vector3(0, 15);

    }

    public override void OffPattern()
    {
        SetDie(true);

        //
        StopAllCoroutines();
        foreach(var obj in _spawnLaserObjects)
        {
            Destroy(obj);
        }
        _spawnLaserObjects.Clear();

        _bulletDangerSpaceLeftDown.gameObject.SetActive(false);
        _bulletDangerSpaceRightUp.gameObject.SetActive(false);

        _bulletDangerSpaceLeftUp.gameObject.SetActive(false);
        _bulletDangerSpaceRightDown.gameObject.SetActive(false);

        _laserX.gameObject.SetActive(false);
        _laserY.gameObject.SetActive(false);

        _laserXDamage.SetOnOff(false);
        _laserYDamage.SetOnOff(false);

    }


}
