using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AnchorSpeaker : MonoBehaviour
{
    [SerializeField] SpeakerAttack attack;

    [ContextMenu("T")]
    public IEnumerator Attack(int cnt)
    {

        YieldInstruction one = new WaitForSeconds(1f);
        for (int i = 0; i < cnt; i++)
        {

            yield return one;

            var obj = Instantiate(attack, transform.position, Quaternion.identity);
            obj.SetDamage(cnt);

        }

        yield return one;

        Destroy(gameObject);
    }


}
