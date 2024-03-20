using UnityEngine;

public class CrackCollision : MonoBehaviour
{
    private float damage;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHitAble>(out var h))
        {
            h.Hit(damage);
        }
    }

}
