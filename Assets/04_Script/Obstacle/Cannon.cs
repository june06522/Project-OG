using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class Cannon : MonoBehaviour
{
    [SerializeField] EnemyBullet bullet;
    [SerializeField] Transform shootPoint;
    [SerializeField] float speedRatio;
    [SerializeField] float scaleRatio;
    [SerializeField] int vibrato = 10;
    [SerializeField] float elasticity = 1;
    [SerializeField] Ease ease;
    
    [SerializeField]
    AnimationCurve curve;

    Material mat;
    Vector3 originScale;
    

    private void Awake()
    {
        Renderer mr = GetComponent<Renderer>();
        mat = Instantiate(mr.material);
        mr.material = mat;
        originScale = transform.localScale;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Shoot();
    }

    public void Shoot() => StartCoroutine(ShootCoroutine());

    public IEnumerator ShootCoroutine()
    {
        transform.DOKill();
        transform.localScale = originScale;

        Vector2 dir = transform.right;
        float start = 0f;
        float end = 1f;

        mat.SetFloat("_Panner", -0.2f);
        while (start < end)
        {
            Debug.Log(Mathf.Lerp(-0.2f, 1.2f, curve.Evaluate(start / end)));
            mat.SetFloat("_Panner", Mathf.Lerp(-0.2f, 1.2f, curve.Evaluate(start/end)));
            start += Time.deltaTime * speedRatio;
            yield return null;
        }

        transform.DOPunchScale(originScale * scaleRatio, 0.5f, vibrato, elasticity).SetEase(ease);
        EnemyBullet clone = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        clone.Shoot(-transform.up);
    }
}
