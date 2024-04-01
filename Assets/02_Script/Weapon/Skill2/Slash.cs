using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

public class Slash : MonoBehaviour
{
    [SerializeField] VisualEffect effect;
    [SerializeField] Transform swordParent;
    [SerializeField] AudioClip clip;
    private void OnEnable()
    {
        Play();        
    }

    [ContextMenu("Play")]
    void Play()
    {

        effect.Play();

        float originAngle = swordParent.rotation.eulerAngles.z;
        float originY = swordParent.eulerAngles.y;

        SoundManager.Instance.SFXPlay("slash", clip);
        Sequence seq = DOTween.Sequence();
        seq.Append(swordParent.DORotate(new Vector3(0, originY, originAngle + 90), 0.3f * 0.2f).SetEase(Ease.Linear)).
            Append(swordParent.DORotate(new Vector3(0, originY, originAngle + 180), 0.3f * 0.2f).SetEase(Ease.Linear)).
            Append(swordParent.DORotate(new Vector3(0, originY, originAngle + 270), 0.3f * 0.2f).SetEase(Ease.Linear)).
            Append(swordParent.DORotate(new Vector3(0, originY, originAngle), 0.3f * 0.2f).SetEase(Ease.Linear)).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        
    }

}
