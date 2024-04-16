using UnityEngine;

public class SpeakerAttack : MonoBehaviour
{
    float damage;

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
        Debug.Log("½Ã²ô·¯¿ò");
        if (collision.TryGetComponent<IHitAble>(out var h))
        {
            h.Hit(damage);
        }
    }
}
