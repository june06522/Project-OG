using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserSkillObj : MonoBehaviour
{
    [SerializeField] LaserSkillLine gunLine;
    bool isMoving = false;
    Coroutine iCo = null;
    LaserSkillLine obj;
    LaserSkillLine _obj;

    Transform _weaponTrm, _target;
    int _power;

    public void Execute(Transform weaponTrm, Transform target, int power)
    {
        _weaponTrm = weaponTrm;
        _target = target;
        _power = power;
        isMoving = true;
        if (iCo == null)
        {
            if (obj != null)
                Destroy(obj);

            obj = Instantiate(gunLine, Vector3.zero, Quaternion.identity);
            iCo = StartCoroutine(ILaser());
        }
    }

    private IEnumerator ILaser()
    {
        if (obj == null)
            obj = Instantiate(gunLine, Vector3.zero, Quaternion.identity);
        while (isMoving)
        {
            isMoving = false;
            obj.LineRenderer.positionCount = 2;
            RaycastHit2D hit = Physics2D.Raycast(_weaponTrm.position, _target.position - _weaponTrm.position, int.MaxValue, LayerMask.GetMask("Enemy"));

            if (hit.collider != null)
            {
                obj.SetLine(_weaponTrm.position, hit.point, _power, 0.01f * _power);
                obj.LineRenderer.enabled = true;
                obj.EdgeCollider.SetPoints(new List<Vector2>
            {
                _weaponTrm.position,
                hit.point
            });

            }
            yield return null;
        }


        if (_obj != null)
            Destroy(_obj);
        _obj = obj;
        DOTween.To(() => _obj.LineRenderer.widthMultiplier, x => _obj.LineRenderer.widthMultiplier = x, 0f, 0.5f);

        Destroy(_obj.gameObject, 0.5f);
        iCo = null;

    }
}
