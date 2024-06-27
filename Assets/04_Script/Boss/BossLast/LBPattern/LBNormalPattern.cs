using DG.Tweening;
using FD.Dev;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBNormalPattern : LBRandomPattern
{

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

    private float _teleportDelayTime = 1f;
    private float _laserAwakeTime = 0.51f;
    private float _laserDelayTime = 0.25f;
    private float _laserDisappearTime = 0.25f;
    private float _laserPatternEndDelay = 1.5f;

    WaitForSeconds _wfsTeleportDelay;
    WaitForSeconds _wfsLaserAwake;
    WaitForSeconds _wfsLaserDelay;
    #endregion

    #region Fall Gravity
    [Header("Fall Gravity")]
    [SerializeField] private WorldPlayerDown _worldMover;
    [SerializeField] private ParticleSystem _stageParticle;

    private float _defaultSpeed = 0.2f;
    private float _changeSpeed = 5f;

    private float _changeGravityTime = 5f;

    private WaitForSeconds _wfsChangeGravityTime;

    #endregion

    private void Awake()
    {

        // Laser
        _laserXSprite = _laserX.GetComponent<SpriteRenderer>();
        _laserYSprite = _laserY.GetComponent<SpriteRenderer>();

        _laserXDamage = _laserX.GetComponent<TouchDamageObject>();
        _laserYDamage = _laserY.GetComponent<TouchDamageObject>();

        _wfsTeleportDelay = new WaitForSeconds(_teleportDelayTime);
        _wfsLaserAwake = new WaitForSeconds(_laserAwakeTime);
        _wfsLaserDelay = new WaitForSeconds(_laserDelayTime);

        // Fall Gravity
        _wfsChangeGravityTime = new WaitForSeconds(_changeGravityTime + 0.1f);

        RegisterPattern(TeleportAndLaser);
        //RegisterPattern(RainbowBullet);
        RegisterPattern(FallGravity);

    }

    // 텔레포트 후 십자, 대각 십자 레이저 발사
    private void TeleportAndLaser()
    {

        _rootLaserTrm.rotation = Quaternion.identity;
        _laserX.localScale = new Vector3(150f, 0f, 1f);
        _laserY.localScale = new Vector3(0f, 150f, 1f);

        _laserXDamage.SetOnOff(false);
        _laserYDamage.SetOnOff(false);

        StartCoroutine(TeleportAndLaserCo());

    }

    IEnumerator TeleportAndLaserCo()
    {
        _seq = DOTween.Sequence();

        // Tweening 
        _seq.Append(_boss.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InElastic));

        yield return _wfsTeleportDelay;

        float randomX = Random.Range(-12f, 12f);
        _boss.transform.localPosition = new Vector3(randomX, 7.1f);
        _seq.Append(_boss.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic));

        yield return _wfsLaserDelay;

        _seq.Append(_laserX.DOScaleY(0.1f, 0.1f).SetEase(Ease.OutBack))
             .Join(_laserY.DOScaleX(0.1f, 0.1f).SetEase(Ease.OutBack));

        yield return _wfsLaserAwake;

        _laserXDamage.SetOnOff(true);
        _laserYDamage.SetOnOff(true);

        // Tweening
        _seq.Append(_laserX.DOScaleY(1, 0.5f).SetEase(Ease.OutElastic))
            .Join(_laserY.DOScaleX(1, 0.5f).SetEase(Ease.OutElastic));

        SetPower(true);

        yield return _wfsLaserAwake;

        _seq2 = DOTween.Sequence();

        bool isLeft = Random.Range(0, 2) == 1;
        int rotateDir = isLeft ? 1 : -1;

        // Tweening
        _seq2.Append(_rootLaserTrm.DORotate(new Vector3(0f, 0f, 45f * rotateDir),
            3f, RotateMode.LocalAxisAdd))
            .Append(_rootLaserTrm.DORotate(new Vector3(0f, 0f, 180f * rotateDir),
            2f, RotateMode.LocalAxisAdd));

        _seq2.AppendCallback(() =>
        {
            SetPower(false);
        });

        _seq2.Append(_laserX.DOScaleY(0, _laserDisappearTime).SetEase(Ease.OutElastic))
              .Join(_laserY.DOScaleX(0, _laserDisappearTime).SetEase(Ease.OutElastic));

        _seq2.OnComplete(() =>
        {

            _laserXDamage.SetOnOff(false);
            _laserYDamage.SetOnOff(false);

            FAED.InvokeDelay(() => { _isEnd = true; }, _laserPatternEndDelay);

        });


    }

    // 포물선을 그리는 총알을 발사
    private void RainbowBullet()
    {

    }

    // 중력을 강화한다?
    private void FallGravity()
    {

        StartCoroutine(FallGravityCo());

    }

    IEnumerator FallGravityCo()
    {

        var obj = _stageParticle.main;

        SetPower(true);
        float currentTime = 0f;
        while(currentTime < _changeGravityTime)
        {
            currentTime += Time.deltaTime;
            obj.simulationSpeed = Mathf.Lerp(1f, 50f, currentTime / _changeGravityTime);
            _worldMover.ChangeSpeed(Mathf.Lerp(_defaultSpeed, _changeSpeed, currentTime / _changeGravityTime));
            yield return null;

        }

        yield return _wfsChangeGravityTime;

        _isEnd = true;

        currentTime = 1f;
        while (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            obj.simulationSpeed = Mathf.Lerp(1f, 50f, currentTime / _changeGravityTime);
            _worldMover.ChangeSpeed(Mathf.Lerp(_defaultSpeed, _changeSpeed, currentTime / _changeGravityTime));
            yield return null;

        }

        yield return _wfsChangeGravityTime;
        SetPower(false);


    }

    public override void OnPattern()
    {
        //
    }

    public override void OffPattern()
    {
        //
        if (_seq != null)
            _seq.Kill();
        if (_seq2 != null)
            _seq2.Kill();

        _rootLaserTrm.DOKill();
        _laserX.DOKill();
        _laserY.DOKill();

        _boss.transform.DOKill();

        StopAllCoroutines();
        SetPower(false);

        _boss.transform.localScale = Vector3.one;

        var obj = _stageParticle.main;
        obj.simulationSpeed = 0.5f;
        _worldMover.ChangeSpeed(0);

        _laserX.localScale = new Vector3(150f, 0f, 1f);
        _laserY.localScale = new Vector3(0f, 150f, 1f);

        _laserXDamage.SetOnOff(false);
        _laserYDamage.SetOnOff(false);

        _boss.transform.localPosition = new Vector3(0, 0);
        TimeManager.instance.Stop(0.25f, 2f);


    }
}
