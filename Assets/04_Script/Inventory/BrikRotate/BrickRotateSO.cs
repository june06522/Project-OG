using UnityEngine;

[CreateAssetMenu(menuName = "SO/BrickSO")]
public class BrickRotateSO : ScriptableObject
{
    public BrickType brickType;
    public Item[] items;
}