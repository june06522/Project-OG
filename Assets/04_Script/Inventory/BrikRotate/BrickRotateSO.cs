using UnityEngine;

[CreateAssetMenu(menuName = "SO/BrickSO/SO")]
public class BrickRotateSO : ScriptableObject
{
    public BrickType brickType;
    public Item[] items;
}