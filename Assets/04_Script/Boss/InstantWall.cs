using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHP player;

        if (collision.gameObject.TryGetComponent<PlayerHP>(out player))
        {
            player.Hit(20);
        }
    }
}
