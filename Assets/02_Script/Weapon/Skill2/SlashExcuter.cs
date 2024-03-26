using DG.Tweening;
using NavMeshPlus.Editors.Components;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SlashExcuter : MonoBehaviour
{
    public LayerMask hitableLayer;

    private void OnEnable()
    {
        StartCoroutine(Play());
    }

    IEnumerator Play()
    {

        var obj1 = transform.GetChild(0).gameObject;
        obj1.SetActive(true);

        StartCoroutine(Attack(obj1));
        yield return new WaitForSeconds(0.15f);


        var obj2 = transform.GetChild(1).gameObject;
        obj2.SetActive(true);

        StartCoroutine(Attack(obj2));
        yield return new WaitForSeconds(0.2f);

        var obj3 = transform.GetChild(2).gameObject;
        obj3.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);

        StartCoroutine(Attack(obj3));
        yield return new WaitForSeconds(0.2f);

        var obj4 = transform.GetChild(4).gameObject;
        obj4.SetActive(true);
        transform.GetChild(5).gameObject.SetActive(true);

        StartCoroutine(Attack(obj4));
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);

    }

    IEnumerator Attack(GameObject obj)
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
