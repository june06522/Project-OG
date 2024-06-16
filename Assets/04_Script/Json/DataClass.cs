#region 클래스 선언
using UnityEngine;

public class SoundData
{
    public float MasterSoundVal = .5f;
    public float BGMSoundVal = .5f;
    public float SFXSoundVal = .5f;
}

public class KeyData
{
    public KeyCode up = KeyCode.W;
    public KeyCode down = KeyCode.S;
    public KeyCode left = KeyCode.A;
    public KeyCode right = KeyCode.D;
    public KeyCode dash = KeyCode.Space;
    public KeyCode inven = KeyCode.Tab;
    public KeyCode action = KeyCode.F;
    public KeyCode map = KeyCode.M;
}

public class TutorialData
{
    public bool isClear = false;
}
#endregion