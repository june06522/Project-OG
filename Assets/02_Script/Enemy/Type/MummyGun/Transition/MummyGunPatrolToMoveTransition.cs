using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyGunPatrolToMoveTransition : BaseFSM_Transition<EMummyGunState>
{
    Transform playerTrm;
    public MummyGunPatrolToMoveTransition(BaseFSM_Controller<EMummyGunState> controller, EMummyGunState nextState) : base(controller, nextState)
    {
        playerTrm = GameManager.Instance.player;
    }

    protected override bool CheckTransition()
    {
        // 거리 안에 있고 공격 할수 있는 상태일때.
        return Transitions.CheckDistance(playerTrm, this.transform, _data.Range) 
            && !_data.IsAttackCoolDown;
    }
}
