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

    #region Laser
    [Header("Laser")]
    [SerializeField]
    private Transform _rootLaserTrm;

    [SerializeField]
    private Transform _laserX;
    [SerializeField]
    private Transform _laserY;

    private SpriteRenderer _laserXSprite;
    private SpriteRenderer _laserYSprite;

    private TouchDamageObject _laserXDamage;
    private TouchDamageObject _laserYDamage;

    private float _dashDelayTime = 1.2f;
    private float _laserAwakeTime = 0.51f;
    private float _laserDelayTime = 1.0f;
    private float _laserDisappearTime = 0.25f;
    private float _laserPatternEndDelay = 1.5f;

    WaitForSeconds _wfsDashDelay;
    WaitForSeconds _wfsLaserAwake;
    WaitForSeconds _wfsLaserDelay;
    WaitForSeconds _wfsBulletDelay = new WaitForSeconds(0.05f);
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

        yield return _wfsLaserAwake;

        _laserXDamage.SetOnOff(true);
        _laserYDamage.SetOnOff(true);

        // Tweening
        _seq.Append(_laserX.DOScaleY(1, 0.5f).SetEase(Ease.OutElastic))
            .Join(_laserY.DOScaleX(1, 0.5f).SetEase(Ease.OutElastic));

        // ÃÑ¾Ë ³­»ç
        bool shotLeftUpAndRightDown = Random.Range(0, 2) == 0;
        if(shotLeftUpAndRightDown == false)
        {
            angle += 90f;
        }

        float angle2 = angle + 180f;

        for(int i = 0; i < 60; ++i)
        {

            float randomAngle = Random.Range(angle, angle + 90f) % 360;
            ShotBullet(randomAngle);

            randomAngle = Random.Range(angle2, angle2 + 90f) % 360;
            ShotBullet(randomAngle);

            yield return _wfsBulletDelay;
        }

        yield return _wfsLaserDelay;

        _seq.Append(_laserX.DOScaleY(0, _laserDisappearTime).SetEase(Ease.OutElastic))
              .Join(_laserY.DOScaleX(0, _laserDisappearTime).SetEase(Ease.OutElastic));

        _seq.OnComplete(() =>
        {

            _laserXDamage.SetOnOff(false);
            _laserYDamage.SetOnOff(false);

            FAED.InvokeDelay(() => { _isEnd = true; }, _laserPatternEndDelay);

        });


    }

    private void ShotBullet(float degree_angle)
    {

    }

    public override void OnPattern()
    {
        //
        _isEnd = true;

    }

    public override void OffPattern()
    {
        //


    }


}
