using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LastBossPattern : MonoBehaviour
{

    Transform _playerTrm;

    #region BossAwakeEffect
    [Header("Boss Awake Effect")]       // 보스 등장할 때 효과
    [SerializeField]
    private Volume _bossVolume;
    private float _awakeEffectTime = 1f;

    private float _startVolumeValue = 1f;
    private float _endVolumeValue = 0.3f;
    #endregion

    #region Boss Laser Loop
    [Header("Boss Laser Loop")]         // 2초마다 발생하는 레이저
    [SerializeField]
    private Transform _laserTrm;
    private SpriteRenderer _laserSprite;
    private TouchDamageObject _laserDamage;

    WaitForSeconds _wfsLaserTime = new WaitForSeconds(3f);
    WaitForSeconds _wfsLaserDangerDelayTime = new WaitForSeconds(0.2f);
    #endregion

    #region Boss Pattern
    [Header("BossPattern")]
    RandomPattern _currentPattern;

    [SerializeField]
    RandomPattern _firstPattern;
    [SerializeField]
    RandomPattern _secondPattern;

    WaitUntil _wuBossPatternEnd;
    Coroutine _currentBossPatternCo;

    #endregion

    private void Awake()
    {

        // Value Setting
        _playerTrm = GameManager.Instance.player;

        _laserSprite = _laserTrm.GetComponent<SpriteRenderer>();
        _laserDamage = _laserTrm.GetComponent<TouchDamageObject>();
        _laserDamage.SetOnOff(false);

        _currentPattern = _firstPattern;
        _wuBossPatternEnd = new WaitUntil(() => { return _currentPattern.IsEnd; });

        StartCoroutine(AppearBossCo());

    }

    IEnumerator AppearBossCo()
    {
        _bossVolume.weight = _startVolumeValue;

        yield return new WaitForSeconds(1f);

        PatternStart();

        float curTime = 0f;
        while(curTime < _awakeEffectTime)
        {
            curTime += Time.deltaTime;
            _bossVolume.weight = Mathf.Lerp(_startVolumeValue, _endVolumeValue, curTime / _awakeEffectTime);
            yield return null;
        }

        _bossVolume.weight = _endVolumeValue;

    }

    private void PatternStart()
    {

        StartCoroutine(Judgement_LaserLoopCo());
        _currentBossPatternCo = StartCoroutine(BossPatternLoopCo());

    }

    IEnumerator Judgement_LaserLoopCo()
    {
        while(true)
        {

            yield return _wfsLaserTime;
            _laserTrm.localScale = new Vector3(0.1f, 30f, 1f);

            _laserTrm.position = new Vector3(_playerTrm.position.x, _laserTrm.position.y);
            _laserDamage.SetOnOff(false);
            _laserSprite.color = new Color(1f, 1f, 1f, 0.25f);

            yield return _wfsLaserDangerDelayTime;

            _laserDamage.SetOnOff(true);
            _laserSprite.color = new Color(1f, 1f, 1f, 1f);

            // Tweening
            Sequence seq = DOTween.Sequence();
            seq.Append(_laserTrm.DOScaleX(4f, 0.5f).SetEase(Ease.OutElastic).SetDelay(1f));
            seq.Append(_laserTrm.DOScaleX(0f, 1f));

            _laserTrm.localScale = new Vector3(0f, 30f, 1f);

        }
    }

    IEnumerator BossPatternLoopCo()
    {

        while(true)
        {

            _currentPattern.Play();

            yield return _wuBossPatternEnd;

        }

    }



}
