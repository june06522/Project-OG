
//root°â patrolState
public class MummyRootState : BaseFSM_State<EMummyState>
{
    protected new MummyStateController controller;
    protected EnemyDataSO _data => controller.EnemyDataSO;

    PatrolAction<EMummyState> patrolAct;

    public MummyRootState(MummyStateController controller) : base(controller)
    {
        this.controller = controller;
        patrolAct = new PatrolAction<EMummyState>(controller, controller.target);
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
    