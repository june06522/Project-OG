using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public enum BurstState
{
    Normal,
    Penetrate,
    Laser
}
public struct BurstMember
{
    public int _curBurstCount;
    public int _penetrateCnt;
    public float _curBurstSpeed;
    public float _curDamage;
    public BurstState _curState;
}

public class BurstSkil : Skill
{

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

    private Dictionary<Transform, bool> _playingDictionary;
    private Dictionary<Transform, BurstMember> _powerDictionary;

    private void OnEnable()
    {
        if (_playingDictionary != null)
            _playingDictionary.Clear();
        else
            _playingDictionary = new Dictionary<Transform, bool>();


        if (_powerDictionary != null)
            _powerDictionary.Clear();
        else
            _powerDictionary = new Dictionary<Transform, BurstMember>();
    }

    public override void Excute(Transform weaponTrm, Transform target, int power, SendData trigger = null)
    {
        _powerDictionary[weaponTrm] = CurPowerInit(power);

        bool isPlaying = true;
        if (_playingDictionary.TryGetValue(weaponTrm, out isPlaying))
        {
            if (isPlaying == true) return;
        }


        if (_powerDictionary[weaponTrm]._curState == BurstState.Laser)
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
        BurstMember bM = _powerDictionary[weaponTrm];

        for (int i = 0; i < bM._curBurstCount; i++)
        {
            weaponTrm.DOShakePosition(0.1f, 0.25f);
            if(bM._curState == BurstState.Penetrate)
            {
                PenetrateBullet bullet = 
                    Instantiate(_penetrateBulletPrefab, attackTrm.position, weaponTrm.rotation);
                bullet.Init(bM._penetrateCnt);
                bullet.Shoot(bM._curDamage);
            }
            else
            {
                Instantiate(_bulletPrefab, attackTrm.position, weaponTrm.rotation).Shoot(bM._curDamage);
            }
            yield return new WaitForSeconds(bM._curBurstSpeed);
        }

        _playingDictionary[weaponTrm] = false;
    }

    private IEnumerator LaserAttack(Transform weaponTrm)
    {
        weaponTrm.DOKill();
        weaponTrm.DOShakePosition(0.5f, 0.5f);

        BurstMember bM = _powerDictionary[weaponTrm];
        Transform attackTrm = weaponTrm.GetChild(0);
      
        Instantiate(_laserPrefab, attackTrm.position, weaponTrm.rotation)
            .Shoot(attackTrm.position, weaponTrm.right * 30, bM._curDamage, true);

        yield return new WaitForSeconds(1f);
        _playingDictionary[weaponTrm] = false;
    }

    protected new BurstMember CurPowerInit(int power)
    {
        power = Mathf.Clamp(power, 1, 5);
        BurstMember bM = new();
        switch (power)
        {
            case 1: Power1(ref bM); break;
            case 2: Power2(ref bM); break;
            case 3: Power3(ref bM); break;
            case 4: Power4(ref bM); break;
            case 5: Power5(ref bM); break;
        }

        return bM;
    }

    private void Power1(ref BurstMember bM)
    {
        bM._curBurstSpeed = _minBurstSpeed;
        bM._curBurstCount = _minBurstCount;
        bM._curDamage = _weaponDamage;
        
        bM._curState = BurstState.Normal;
    } 


    private void Power2(ref BurstMember bM)
    {
        bM._curBurstCount = (int)Mathf.Lerp(_minBurstCount, _maxBurstCount, 0.3f);
        bM._curBurstSpeed = Mathf.Lerp(_minBurstSpeed, _maxBurstSpeed, 0.5f);
        bM._curDamage = _weaponDamage * 1.25f;
        
        bM._curState = BurstState.Normal;
    }

    private void Power3(ref BurstMember bM)
    {
        bM._curBurstCount = (int)Mathf.Lerp(_minBurstCount, _maxBurstCount, 0.6f);
        bM._curBurstSpeed = Mathf.Lerp(_minBurstSpeed, _maxBurstSpeed, 0.25f);
        bM._curDamage = _weaponDamage * 2f;
        bM._penetrateCnt = 2;
        bM._curState = BurstState.Penetrate;
    }

    private void Power4(ref BurstMember bM)
    {
        bM._curBurstCount = _maxBurstCount;
        bM._curBurstSpeed = _minBurstSpeed;
        bM._curDamage = _weaponDamage * 2.5f;
        bM._penetrateCnt = 4;
        bM._curState = BurstState.Penetrate;
    }

    private void Power5(ref BurstMember bM)
    {
        bM._curDamage = _weaponDamage * 10;
        bM._curState = BurstState.Laser;
    }
}
