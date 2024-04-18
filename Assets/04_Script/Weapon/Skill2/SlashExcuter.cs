using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashExcuter : MonoBehaviour
{
    public LayerMask hitableLayer;
    [SerializeField] Slash slash;


    public List<Color> colors = new List<Color>();
    private void OnEnable()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {
        var obj1 = Instantiate(slash, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - 90));
        StartCoroutine(Attack(obj1));
        obj1.effect.SetVector4("EffectColor", colors[0] * 1.4f);

        yield return new WaitForSeconds(0.15f);

        var obj2 = Instantiate(slash, transform.position + transform.up * 1.5f, Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
        StartCoroutine(Attack(obj2));
        obj2.effect.SetVector4("EffectColor", colors[1] * 1.4f);

        yield return new WaitForSeconds(0.2f);

        var obj3 = Instantiate(slash, transform.position + transform.up * 3, Quaternion.Euler(0, 0, transform.eulerAngles.z - 90));
        StartCoroutine(Attack(obj3));
        obj3.effect.SetVector4("EffectColor", colors[2] * 1.4f);

        var obj4 = Instantiate(slash, transform.position + transform.up * 3, Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
        obj4.effect.SetVector4("EffectColor", colors[3] * 1.4f);

        yield return new WaitForSeconds(0.2f);

        var obj5 = Instantiate(slash, transform.position + transform.up * 5, Quaternion.Euler(0, 0, transform.eulerAngles.z - 90));
        obj5.effect.SetVector4("EffectColor", colors[4] * 1.4f);
        StartCoroutine(Attack(obj5));

        var obj6 = Instantiate(slash, transform.position + transform.up * 5, Quaternion.Euler(0, 0, transform.eulerAngles.z + 90));
        obj6.effect.SetVector4("EffectColor", colors[5] * 1.4f);

        yield return new WaitForSeconds(0.3f);
        Destroy(obj1);
        Destroy(obj2);
        Destroy(obj3);
        Destroy(obj4);
        Destroy(obj5);
        Destroy(obj6);
        Destroy(gameObject);

    }

    IEnumerator Attack(Slash obj)
    {

        foreach (var item in Physics2D.OverlapCircleAll(obj.transform.position, 2.5f, hitableLayer))
        {

            if (item != null)
                if (item.TryGetComponent<IHitAble>(out var h))
                {

                    for (int i = 0; i < 3; i++)
                    {

                        yield return new WaitForSeconds(0.1f);
                        h.Hit(10);

                    }

                }

        }

    }

}
