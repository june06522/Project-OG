using UnityEngine;

public class BreakableBox : BreakableObject
{
    //[SerializeField, Range(0f, 1f)]
    //private float _goldDropPercent = 0f;

    [SerializeField]
    private float _minDropGold;
    [SerializeField]
    private float _maxDropGold;

    protected override void BrakingObject()
    {
        
    }
}
