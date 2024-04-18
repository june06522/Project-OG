using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStageObject
{
    public Stage Stage { get; set; }
    public abstract bool Get_IsNeedRemove();

    public void LinkStage(Stage stage)
    {
        Stage = stage;
    }
}
