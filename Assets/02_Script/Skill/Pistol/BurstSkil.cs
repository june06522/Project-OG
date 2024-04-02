using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class BurstSkil : Skill
{
    enum BurstState
    {
        Normal,
        Penetrate,
        Laser
    }
    private BurstState _curState;


    [Header("Prefab")]
    [SerializeField]
    private Bullet _bulletPrefab;
    [SerializeField]
    private PenetrateBullet _penetrateBulletPrefab;
    [SerializeField]
    private LaserBullet _laserPrefab;

    [Header("Burst Setting")]
    [SerializeField]
    private int _minBurstCount;
    [SerializeField]
    private int _maxBurstCount;

    [SerializeField]
    private float _minBurstSpeed;
    [SerializeField]
    private float _maxBurstSpeed;

    [SerializeField] 
    private float _weaponDamage;

    private int _curBurstCount;
    private int _penetrateCnt;
    private float _curBurstSpeed;
    private float _curDamage;

    private Dictionary<Transform, bool> _playingDictionary;

    private void OnEnable()
    {
        if (_playingDictionary != null)
            _playingDictionary.Clear();
        else
            _playingDictionary = new Dictionary<Transform, bool>();
    }

    public override void Excute(Transform weaponTrm, Transform target, int power)
    {
        CurPowerInit(power);


        Debug.Log("gang");
        bool isPlaying = true;
        if (_playingDictionary.TryGetValue(weaponTrm, out isPlaying))
        {
            if (isPlaying == true) return;
        }


        if (_curState == BurstState.Laser)
        {
            StartCoroutine("LaserAttack", weaponTrm);
        }
        else
        {
            StartCoroutine("BurstAttack", weaponTrm);
        }

        _playingDictionary[weaponTrm] = true;
    }

    private IEnumerator BurstAttack(Transform weaponTrm)
    {
        Transform attackTrm = weaponTrm.GetChild(0);
        for (int i = 0; i < _curBurstCount; i++)
        {
            weaponTrm.DOShakePosition(0.1f, 0.25f);
            if(_curState == BurstState.Penetrate)
            {
                PenetrateBullet bullet = 
                    Instantiate(_penetrateBulletPrefab, attackTrm.position, weaponTrm.rotation);
                bullet.Init(_penetrateCnt);
                bullet.Shoot(_curDamage);
            }
            else
            {
                Instantiate(_bulletPrefab, attackTrm.position, weaponTrm.rotation).Shoot(_curDamage);
            }
            yield return new WaitForSeconds(_curBurstSpeed);
        }

        _playingDictionary[weaponTrm] = false;
    }

    private IEnumerator LaserAttack(Transform weaponTrm)
    {
        weaponTrm.DOKill();
        weaponTrm.DOShakePosition(0.5f, 0.5f);

        Transform attackTrm = weaponTrm.GetChild(0);
      
        Instantiate(_laserPrefab, attackTrm.position, weaponTrm.rotation)
            .Shoot(attackTrm.position, weaponTrm.right * 30, _curDamage, true);

        yield return new WaitForSeconds(1f);
        _playingDictionary[weaponTrm] = false;
    }



    public override void Power1()
    {
        _curBurstCount = _minBurstCount;
        _curBurstSpeed = _maxBurstSpeed;
        _curDamage = _weaponDamage;

        _curState = BurstState.Normal;
    } 


    public override void Power2()
    {
        _curBurstCount = (int)Mathf.Lerp(_minBurstCount, _maxBurstCount, 0.3f);
        _curBurstSpeed = Mathf.Lerp(_minBurstSpeed, _maxBurstSpeed, 0.5f);
        _curDamage = _weaponDamage * 1.25f;

        _curState = BurstState.Normal;
    }

    public override void Power3()
    {
        _curBurstCount = (int)Mathf.Lerp(_minBurstCount, _maxBurstCount, 0.6f);
        _curBurstSpeed = Mathf.Lerp(_minBurstSpeed, _maxBurstSpeed, 0.25f);
        _curDamage = _weaponDamage * 2f;
        _penetrateCnt = 2;
        _curState = BurstState.Penetrate;
    }

    public override void Power4()
    {
        _curBurstCount = _maxBurstCount;
        _curBurstSpeed = _minBurstSpeed;
        _curDamage = _weaponDamage * 2.5f;
        _penetrateCnt = 4;
        _curState = BurstState.Penetrate;
    }

    public override void Power5()
    {
        _curDamage = _weaponDamage * 10;
        _curState = BurstState.Laser;
    }
}
