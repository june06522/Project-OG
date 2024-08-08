using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GeneratorEnforceInfo
{
    public string LightSaborExplain;
    public string DroneExplain;
    public string EmpExplain;
    public string SpeakerExplain;
    public string LaserExplain;
}


[CreateAssetMenu(menuName = "SO/ExplainSO")]
public class GeneratorEnforceInfoSO : ScriptableObject
{
    public GeneratorID GeneratorID;
    public GeneratorEnforceInfo Info;
}
