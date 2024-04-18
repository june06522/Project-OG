using Cinemachine;
using DG.Tweening;
using FD.Dev;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

public enum ESpikeWormState
{
    None,
    Cool,
    SnakeMove,
    SpikeBullet,
    EnergeCharge,
    TightSnakeBody,
    BodyBullet,
    Die

}

public class SW_Pattern : MonoBehaviour
{
    [SerializeField]
    private SpikeWorm _boss;

    [SerializeField]
    private GameObject _rootObject;
    [SerializeField]
    private CinemachineVirtualCamera _vStageCam;

    [Header("Boss")]
    private Transform _player;
    [SerializeField]
    ESpikeWormState _state = ESpikeWormState.None;
    private bool _isOutside = true;

    [Header("Body")]
    [SerializeField]
    private Transform _head;
    [SerializeField]
    private SpriteRenderer _headSpriteRenderer;
    private readonly int HASH_BLINK = Shader.PropertyToID("_StrongTintFade");

    private bool _patternCheckOnce = false;

    [Header("Sound")]
    [SerializeField]
    private AudioClip _shotBodyBulletClip;
    [SerializeField]
    private AudioClip _shakeClip;
    [SerializeField]
    private AudioClip _hitClip;
    [SerializeField]
    private AudioClip _chargeClip;

    [Header("Info")]
    [SerializeField]
    private SnakeMove _snakeMove;
    [SerializeField]
    private LayerMask _obstacleLayer;
    [SerializeField]
    private int _bodyCount;

    [Header("Body Bullet")]
    [SerializeField]
    private GameObject _dangerObject;

    [SerializeField]
    private int _bodyBulletShotCount = 10;
    private float _shotDelay = 0.25f;

    [Header("SnakeMove Pattern")]
    [SerializeField]
    private float _detectRadius;
    private float _accel;

    [Header("EnergeCharge Pattern")]
    [SerializeField]
    private Charge_SWPart _chargeObject;
    [SerializeField]
    private float _chargeObjectSpeed;
    [SerializeField]
    private float _chargeObjectDamage;
    [SerializeField]
    private LayerMask _collectedLayer;

    private int _currentBodyCount = 0;

    [Header("Spike Bullet")]
    [SerializeField]
    private EnemyBullet _spikeBullet;
    [SerializeField]
    private int _shotCount = 5;
    [SerializeField]
    private float _turnSpeed = 5f;

    [SerializeField]
    private GameObject _dangerCrossObject;
    [SerializeField]
    private GameObject _dangerXCrossObject;

    private bool _isShotCross = false;
    private int _currentShotCount = 0;
    private float _currentTime = 0;

    [Header("Stage Pos")]
    [SerializeField]
    private Transform _underStartPos;
    [SerializeField]
    private Transform _worldCenterPos;

    // move Value
    private Vector3 _movePos = Vector3.zero;
    private Vector3 _dir = Vector3.up;
    private float _speed = 10.0f;

    // state Value
    private float _cool = 0f;

    // tightSnakeMove
    private float _angle = 0f;
    private float _moveAngle = 0f;
    private float _tightSnakeMoveSpeed = 30.0f;
    private float _rotateAngleSpeed = 180.0f;
    Color _saveColor;

    private void Awake()
    {
        _player = GameManager.Instance.player;
        _saveColor = _headSpriteRenderer.color;

        _boss.DieEvt += HandleDie;
    }

    private void Update()
    {
        BossStatePlay();
        RandomState();
    }

    public void RandomState()
    {
        if (_state != ESpikeWormState.None)
            return;

        if(_isOutside)
        {
            int rand = Random.Range(0, 3);
            if(rand == 0)
            {
                SetBossState(ESpikeWormState.TightSnakeBody);
            }
            else if(rand == 1)
            {
                SetBossState(ESpikeWormState.SnakeMove);
            }
            else if (rand == 2)
            {
                SetBossState(ESpikeWormState.BodyBullet);
            }
        }
        else
        {
            int rand = Random.Range(0, 2);
            if(rand == 0)
            {
                SetBossState(ESpikeWormState.EnergeCharge);
            }
            else if(rand == 1)
            {
                SetBossState(ESpikeWormState.SpikeBullet);
            }
            
        }
    }

    private void BossStatePlay()
    {
        switch (_state)
        {
            case ESpikeWormState.None:
                //
                break;
            case ESpikeWormState.Cool:
                CoolDown();
                break;
            case ESpikeWormState.SnakeMove:
                SnakeMovePattern();
                break;
            case ESpikeWormState.SpikeBullet:
                SpikeBulletPattern();
                break;
            case ESpikeWormState.TightSnakeBody:
                TightSnakePattern();
                break;
            case ESpikeWormState.EnergeCharge:
                EnergeChargePattern();
                break;
            case ESpikeWormState.BodyBullet:
                BodyBullet();
                break;
        }
    }

    private void EnergeChargePattern()
    {
        // Move
        if(_currentBodyCount < _bodyCount)
        {
            if(Vector3.Distance(_head.position, _movePos) < 0.2f)
            {
                _angle += 5f * Mathf.Deg2Rad;
                _movePos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * (10f + _currentBodyCount * 0.1f);
            }

            _dir = (_movePos - _head.position).normalized;
            _head.position += _dir * _speed * Time.deltaTime;

            Collider2D col = Physics2D.OverlapCircle(_head.position, _detectRadius, _collectedLayer);
            if (col)
            {
                Destroy(col.gameObject);
                ChargeCount();
            }

        }
        else // OutWorld
        {
            if (_patternCheckOnce == false)
            {
                _patternCheckOnce = true;
                SoundManager.Instance.SFXPlay("Shake", _shakeClip, 0.7f);

                _vStageCam.transform.DOShakePosition(0.2f, 5, 20);
            }

            if (Vector3.Distance(_head.position, _worldCenterPos.position) > 15f)
            {
                _moveAngle = _angle + (360f * Mathf.Deg2Rad);

                _isOutside = true;
                _currentBodyCount = 0;
                _state = ESpikeWormState.TightSnakeBody;
                Vector3 pos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * 14.9f;
                _dir = (pos - _head.position).normalized;
            }

            _head.position += _dir * _speed * 2 * Time.deltaTime;
        }

    }
    private void SnakeMovePattern()
    {
        _head.localPosition += (_speed + _accel - 8) * _dir * Time.deltaTime;
        _accel += 1f * Time.deltaTime;

        if(_dir == Vector3.up || _dir == Vector3.down)
        {
            // bossY == playerY
            
            if ((_head.position.y - _player.position.y) *_dir.y > 0)
            {

                if (_head.position.x < _player.position.x)
                {

                    _dir = Vector3.right;

                }
                else
                {

                    _dir = Vector3.left;

                }

            }
        }
        else
        {
            if ((_head.position.x - _player.position.x) * _dir.x > 0f)
            {

                if (_head.position.y < _player.position.y)
                {

                    _dir = Vector3.up;

                }
                else
                {

                    _dir = Vector3.down;

                }

            }

        }
        
        if(Physics2D.OverlapCircle(_head.position, _detectRadius, _obstacleLayer))
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(BlinkCo(0.5f));
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(Vector3.one * 1.5f, 0.2f).SetEase(Ease.OutElastic));
            seq.Append(transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBounce));

            _isOutside = false;
            _snakeMove.DestroyBody();

            SoundManager.Instance.SFXPlay("Shake", _shakeClip, 0.7f);
            SoundManager.Instance.SFXPlay("Hit", _hitClip, 0.5f);

            _vStageCam.transform.DOShakePosition(0.3f);

            SetCoolTime(5f);
        }


    }
    private void SpikeBulletPattern()
    {
        // 
        if (_shotCount <= _currentShotCount)
        {
            _currentShotCount = 0;
            SetCoolTime(1f);
        }

        if (Vector3.Distance(_head.position, _movePos) < 0.2f)
        {
            _angle += -75f * Mathf.Deg2Rad;
            _movePos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * 5f;
            _dir = (_movePos - _head.position).normalized;

            //Shot Bullet
            (_isShotCross ? _dangerCrossObject : _dangerXCrossObject).SetActive(true);

            //Anim
            FAED.InvokeDelay(() =>
            {
                (_isShotCross ? _dangerCrossObject : _dangerXCrossObject).SetActive(false);

                float offsetAngle = (_isShotCross) ? 0f : 45f;
                for (int i = 0; i < 4; ++i)
                {
                    float shotAngle = (i * 90 + offsetAngle) * Mathf.Deg2Rad;
                    Vector2 shotDir = new Vector2(Mathf.Cos(shotAngle), Mathf.Sin(shotAngle));

                    Instantiate(_spikeBullet, _head.position, Quaternion.identity).Shoot(shotDir);
                }

                if (coroutine != null)
                    StopCoroutine(coroutine);
                coroutine = StartCoroutine(BlinkCo(0.2f));
                Sequence seq = DOTween.Sequence();
                seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutElastic));
                seq.Append(transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InBounce));

                SoundManager.Instance.SFXPlay("Shake", _shakeClip, 0.7f);
                SoundManager.Instance.SFXPlay("ShotBodyBullet",  _shotBodyBulletClip, 0.5f);
                _vStageCam.transform.DOShakePosition(0.1f, 3);

                _isShotCross = !_isShotCross;
            }, 1f);

            _currentShotCount++;
        }

        
        _head.position += _dir * _turnSpeed * Time.deltaTime;
    }
    private void TightSnakePattern()
    {
        if (_patternCheckOnce == false)
        {
            SoundManager.Instance.SFXPlay("Shake", _shakeClip, 0.7f);


            _patternCheckOnce = true;
            _vStageCam.transform.DOShakePosition(0.2f, 3, 10);
        }

        // step1
        if (Vector3.Distance(_head.position, _worldCenterPos.position) > 15.0f && _angle < _moveAngle)
        {
            _head.position += _dir * _tightSnakeMoveSpeed * Time.deltaTime;
        }
        // step2
        else if (_angle < _moveAngle)
        {
            _angle += Mathf.Deg2Rad * _rotateAngleSpeed * Time.deltaTime;
            Vector3 pos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * 14.9f;
            _dir = (pos - _head.position).normalized;
            _head.position = pos; 
        }
        // step3
        else if (Vector3.Distance(_head.position, _worldCenterPos.position) < 135.0f)
        {
            _head.position += _dir * _tightSnakeMoveSpeed * Time.deltaTime;
        }
        else
        {
            _isOutside = true;
            _snakeMove.ForceDestroyBody();
            SetCoolTime(0.5f);
        }
        

    }
    private void BodyBullet()
    {
        if(_head.localPosition.y < 30f)
        {
            _head.position += _dir * _speed * 2 * Time.deltaTime;
        }
        else if(_bodyBulletShotCount > _currentShotCount)
        {
            if(_patternCheckOnce == false)
            {
                SoundManager.Instance.SFXPlay("Shake", _shakeClip, 0.7f);


                _patternCheckOnce = true;
                _vStageCam.transform.DOShakePosition(0.2f, 5, 20);
            }


            _currentTime += Time.deltaTime;
            if(_shotDelay <= _currentTime)
            {
                _currentTime -= _shotDelay;
                _currentShotCount++;

                int randomIdx = Random.Range(9, 21);
                GameObject body = _snakeMove.BodyList[randomIdx].gameObject;

                // Danger
                GameObject danger = Instantiate(_dangerObject, body.transform.position, Quaternion.identity);

                // Anim
                _snakeMove.BlinkBody(body, 0.1f);

                Sequence seq = DOTween.Sequence();
                seq.Append(body.transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.OutElastic))
                    .Join(danger.transform.DOScaleX(40f, 0.1f).SetEase(Ease.OutCirc))
                    .Append(body.transform.DOScale(Vector3.one * 0.8f, 0.1f).SetEase(Ease.OutBounce));


                // Shot
                FAED.InvokeDelay(() =>
                {
                    SoundManager.Instance.SFXPlay("Shot_BodyBullet", _shotBodyBulletClip, 0.6f);

                    Destroy(danger);
                    Instantiate(_spikeBullet, body.transform.position, Quaternion.identity).Shoot(Vector2.left);
                    Instantiate(_spikeBullet, body.transform.position, Quaternion.identity).Shoot(Vector2.right);
                }, 0.101f);



            }
        }
        else if(_head.localPosition.y < 80f)
        {
            _head.position += _dir * _speed * 2 * Time.deltaTime;
        }
        else
        {
            _snakeMove.ForceDestroyBody();
            SetCoolTime(0.5f);
        }
    }

    private void SetCoolTime(float time)
    {
        _cool = time;
        SetBossState(ESpikeWormState.Cool);
    }
    private void CoolDown()
    {
        _cool -= Time.deltaTime;
        if (_cool <= 0)
            _state = ESpikeWormState.None;
    }
    private IEnumerator BlinkCo(float time)
    {
        

        _headSpriteRenderer.color = Color.white;
        _headSpriteRenderer.material.SetFloat(HASH_BLINK, 1f);
        yield return new WaitForSeconds(time);
        _headSpriteRenderer.color = _saveColor;
        _headSpriteRenderer.material.SetFloat(HASH_BLINK, 0f);


    }

    public void HandleDie()
    {
        _vStageCam.transform.DOShakePosition(0.8f);
        SetBossState(ESpikeWormState.Die);
        FAED.InvokeDelay(() =>
        {
            Destroy(_rootObject);
            _vStageCam.transform.DOShakePosition(0.1f, 5f);
        }, 1f);
    }

    Coroutine coroutine;
    public void ChargeCount()
    {
        SoundManager.Instance.SFXPlay("Charge", _chargeClip, 0.4f);

        _snakeMove.AddBody(true);
        _currentBodyCount++;

        if(coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(BlinkCo(0.1f));
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * 1.2f, 0.1f).SetEase(Ease.OutElastic));
        seq.Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.InBounce));
    }
    public void SetBossState(ESpikeWormState state)
    {
        if (this._state == ESpikeWormState.Cool)
            return;

        _patternCheckOnce = false;
        switch (state)
        {
            case ESpikeWormState.SnakeMove:
                _head.position = _underStartPos.position;
                _accel = 0f;
                _snakeMove.AddBody(Vector3.down, _bodyCount - 10);
                break;
            case ESpikeWormState.SpikeBullet:
                _currentShotCount = 0;
                _movePos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * 5f;
                _dir = (_movePos - _head.position).normalized;
                break;
            case ESpikeWormState.TightSnakeBody:
                {
                    _angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                    _moveAngle = _angle + (390f * Mathf.Deg2Rad);
                    Vector3 pos = _worldCenterPos.localPosition + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * 30;
                    _head.localPosition = pos;
                    _dir = (_worldCenterPos.localPosition - _head.localPosition).normalized;
                    _snakeMove.AddBody(-_dir, _bodyCount + 10);
                }
                break;
            case ESpikeWormState.EnergeCharge:
                {
                    for(int i = 0; i < _bodyCount; ++i)
                    {
                        _angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
                        Vector3 pos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * (30+i*5);

                        Charge_SWPart spawnChargeObject = Instantiate(_chargeObject, pos, Quaternion.identity);
                        spawnChargeObject.transform.SetParent(_rootObject.transform);
                        //spawnChargeObject.transform.SetParent(transform);
                        spawnChargeObject.SetPartInfo(_head, _chargeObjectDamage, _chargeObjectSpeed);
                    }

                    _movePos = _worldCenterPos.position + new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0) * 10f;
                }
                break;
            case ESpikeWormState.BodyBullet:
                _currentShotCount = 0;
                _dir = Vector3.up;
                _head.position = _underStartPos.position;
                _snakeMove.AddBody(Vector3.down, _bodyCount - 20);
                break;
        }
        this._state = state;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_head.position, _detectRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_head.position, _movePos);
        Gizmos.DrawWireCube(_movePos, Vector3.one);

    }
#endif
}
