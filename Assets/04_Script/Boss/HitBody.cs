using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBody : MonoBehaviour, IHitAble
{
    [SerializeField] private Boss _boss;

    [field: SerializeField] public FeedbackPlayer feedbackPlayer { get; set; }

    public bool Hit(float damage)
    {
        float critical = UnityEngine.Random.Range(0.25f, 1.75f);
        damage += critical;

        if (_boss.IsDie)
            return false;

        _boss.MinusCurrentHp(damage);

        feedbackPlayer?.Play(damage);

        if (_boss.GetCurrentHp() < 0)
        {
            _boss.OnDie();

            return false;
        }

        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerHP player;

        if(collision.gameObject.TryGetComponent<PlayerHP>(out player))
        {
            player.Hit(_boss.so.Damage);
        }
    }
}
