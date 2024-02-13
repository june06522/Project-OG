using FSM_System;
using System;

public class BaseFSM_State<T> : FSM_System.FSM_State<T> where T : Enum
{
    protected new BaseFSM_Controller<T> controller;
    public BaseFSM_State(BaseFSM_Controller<T> controller) : base(controller)
    {
        this.controller = controller;
    }
}
