using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FSM_System;

public class NipperLaserEnemyAttackState : FSM_State<ENormalPatrolEnemyState>
{
    private NipperLaserEnemyStateController _controller;

    public NipperLaserEnemyAttackState(NipperLaserEnemyStateController controller) : base(controller)
    {
        _controller = controller;
    }

    protected override void EnterState()
    {
        StartCoroutine(Laser(0.2f));
    }

    protected override void ExitState()
    {

    }

    protected override void UpdateState()
    {

    }

    private IEnumerator Laser(float laserFireTime)
    {
        Vector3 origin = _controller.transform.position;

        Vector2 dir = GameManager.Instance.player.position - _controller.transform.position;
        float z = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, z - 90);

        _controller.animator.SetTrigger(_controller.dehisce);
        MakeLaser(_controller.lineRenderer, origin, WallChecker(origin, dir), 0.2f, _controller.laserWarningMat, new Color(1, 0, 0, 0.2f));

        yield return new WaitForSeconds(0.5f);
        _controller.lineRenderer.enabled = false;

        float curTime = 0;
        MakeLaser(_controller.lineRenderer, origin, WallChecker(origin, dir), 0.1f, _controller.laserMat, Color.white);
        PlayerChecker(origin, dir);

        while (curTime < laserFireTime)
        {
            curTime += Time.deltaTime;
            yield return null;
        }

        _controller.animator.SetTrigger(_controller.shut);
        _controller.lineRenderer.enabled = false;

        yield return new WaitForSeconds(0.5f);
        _controller.animator.SetTrigger(_controller.idle);
        _controller.EnemyDataSO.SetCoolDown();
        _controller.ChangeState(ENormalPatrolEnemyState.Idle);
    }

    private void MakeLaser(LineRenderer line, Vector3 startVec, Vector3 endVec, float width, Material mat = null, Color color = default(Color))
    {
        if (color != default(Color))
        {
            line.startColor = color;
            line.endColor = color;
        }
        line.material = mat;
        line.enabled = true;
        line.SetPosition(0, startVec);
        line.startWidth = width;
        line.SetPosition(1, endVec);
        line.startWidth = width;
    }

    private Vector2 WallChecker(Vector3 originVec, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(originVec, dir, Mathf.Infinity, LayerMask.GetMask("Wall"));
        if(hit.collider)
        {
            return hit.point;
        }
        return Vector2.zero;
    }

    private void PlayerChecker(Vector3 originVec, Vector2 dir)
    {
        RaycastHit2D hit = Physics2D.Raycast(originVec, dir, Mathf.Infinity, LayerMask.GetMask("Player", "Wall"));
        if (hit.collider)
        {
            if (hit.collider.TryGetComponent<PlayerHP>(out var player))
            {
                player.Hit(_controller.EnemyDataSO.AttackPower);
            }
            else
            {
                return;
            }
        }
    }
}
