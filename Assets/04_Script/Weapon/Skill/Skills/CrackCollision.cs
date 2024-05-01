using UnityEngine;
using UnityEngine.VFX;

public class CrackCollision : MonoBehaviour
{
    private float damage;
    [SerializeField] VisualEffect vfx;

    public void SetDamage(float damage)
    {
        
        this.damage = damage;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player")) return;

        if (collision.TryGetComponent<IHitAble>(out var h))
        {
            
            h.Hit(damage);
            h.Hit(damage * 1.2f);
            h.Hit(damage * 1.5f);

        }

    }

}
    