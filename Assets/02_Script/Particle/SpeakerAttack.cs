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
        if (collision.CompareTag("HitAble"))
        {
            collision.GetComponent<IHitAble>().Hit(damage);
            Debug.Log(collision.gameObject.name);
        }
        //Debug.Log(collision.gameObject.name);
        //if (collision.TryGetComponent<IHitAble>(out var h) && collision.tag != "Player")
        //{
        //    h.Hit(damage);
        //    Debug.Log(collision.gameObject.name);
        //}
    }
}
