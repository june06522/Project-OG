using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEvent : MonoBehaviour
{
    Item _curItem;

    [SerializeField]
    ParticleSystem _particleSystem;
    [SerializeField]
    BrickRotateSO[] _brickSO;

    private void Awake()
    {
        if (_particleSystem != null)
            _particleSystem.Play();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Brick"))
        {
            if (_curItem == null)
            {
                _curItem = collision.GetComponent<Item>();
                if(_curItem == null )
                {
                    Debug.LogError($"{transform} : Item Component is null");
                    return;
                }
                SpendMoney();
                _curItem.transform.position = transform.position;

                
            }
            else
            {
                Vector2 pos = collision.transform.position + new Vector3(0,-5,0);
                collision.transform.position = pos;
            }
        }
    }

    public void LeftRotate()
    {
        if (_curItem == null)
            return;

        SwapItem(FindBlock(-1));
    }

    public void RightRotate()
    {
        if (_curItem == null)
            return;

        SwapItem(FindBlock(1));
    }

    private void SwapItem(Item item)
    {
        Vector2 pos = _curItem.transform.position;
        Destroy(_curItem.gameObject);
        _curItem = null;
        Instantiate(item, pos, Quaternion.identity);
    }

    private Item FindBlock(int dir = 0)
    {
        foreach (var b in _brickSO)
        {
            if(b.brickType == _curItem.brickType)
            {
                for(int i = 0; i < b.items.Length; i++)
                {
                    if (b.items[i].Brick == _curItem.Brick)
                    {
                        int index = i + dir;
                        if (index < 0)
                            index = b.items.Length - 1;
                        else if (index == b.items.Length)
                            index = 0;
                        return b.items[index];
                    }
                }
            }
        }
        

        Debug.LogError("BlockSO not found");
        return null;
    }

    private void SpendMoney()
    {

    }
}