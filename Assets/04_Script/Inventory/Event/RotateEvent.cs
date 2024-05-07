using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEvent : MonoBehaviour
{
    Item _curItem;

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
                Vector2 pos = collision.transform.position;
                collision.transform.position = pos;
            }
        }
    }

    public void LeftRotate()
    {
        if (_curItem == null)
            return;
    }

    public void RightRotate()
    {
        if (_curItem == null)
            return;
    }

    public void SpendMoney()
    {

    }
}