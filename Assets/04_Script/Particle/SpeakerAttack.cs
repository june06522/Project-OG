using UnityEngine;

public class SpeakerAttack : MonoBehaviour
{
    float damage = 1;

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void EndOfAnimation()
    {

        Destroy(gameObject, 0.05f);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && collision.TryGetComponent<IHitAble>(out var h))
        {
            h.Hit(damage);
        }
    }
}
