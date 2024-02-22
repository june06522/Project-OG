using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//root°â patrolState
public class MummyGunRootState : BaseFSM_State<EMummyGunState>
{
    protected new MummyGunStateController controller;
    protected EnemyDataSO _data => controller.EnemyData;

    PatrolAction<EMummyGunState> patrolAct;

    public MummyGunRootState(MummyGunStateController controller) : base(controller)
    {
        this.controller = controller;
        patrolAct = new PatrolAction<EMummyGunState>(controller, controller.Target);
    }

    protected override void EnterState()
    {
        patrolAct.OnEnter();
    }

    protected override void ExitState()
    {
        patrolAct.OnExit();
    }

    protected override void UpdateState()
    {
        patrolAct.OnUpdate();
    }
}
