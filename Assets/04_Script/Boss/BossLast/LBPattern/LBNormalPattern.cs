using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBNormalPattern : LBRandomPattern
{

    [Header("Laser")]
    [SerializeField]
    private Transform _laserX;
    [SerializeField]
    private Transform _laserY;

    private float _laserAwakeTime = 0.5f;
    private float _laserDisappearTime = 0.25f;

    WaitForSeconds _wfsLaserAwake;
    WaitForSeconds _wfsLaserDisappear;

    private void Awake()
    {

        _wfsLaserAwake          = new WaitForSeconds(_laserAwakeTime);
        _wfsLaserDisappear      = new WaitForSeconds(_laserDisappearTime);

        RegisterPattern(TeleportAndLaser);
        RegisterPattern(RainbowBullet);
        //RegisterPattern(RainBullet);

    }

    // 텔레포트 후 십자, 대각 십자 레이저 발사
    private void TeleportAndLaser()
    {

    }

    IEnumerator TeleportAndLaserCo()
    {
        float randomX = Random.Range(-12f, 12f);
        _boss.transform.localPosition = new Vector3(randomX, 7.1f);



        yield return _wfsLaserAwake;
        _isEnd = true;

    }

    // 포물선을 그리는 총알을 발사
    private void RainbowBullet()
    {

    }

    // 총알이 비처럼 내린다.
    private void RainBullet()
    {

    }
    

}
