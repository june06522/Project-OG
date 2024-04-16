using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnchorSpeaker : MonoBehaviour
{
    [SerializeField] SpeakerAttack attack;
    [SerializeField] GameObject target;

    [ContextMenu("T")]
    private void Move()
    {
        DOTween.Sequence(transform.DOJump(target.transform.position, 2f, 1, 0.8f).SetEase(Ease.Linear))
        .AppendCallback(() =>
        {
            StartCoroutine(Attack(3));
        });

    }

    public IEnumerator Attack(int cnt)
    {
        YieldInstruction one = new WaitForSeconds(1f);
        for (int i = 0; i < cnt; i++)
        {

            yield return one;

            Instantiate(attack, transform.position, Quaternion.identity);

        }

        yield return one;

        Destroy(gameObject);
    }


}
