using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WaterEffect : MonoBehaviour
{

    [SerializeField] private BulletDataSO data;
    Transform target;

    public BulletData Data { get; protected set; }

    SpriteRenderer _renderer;

    private void Init(float value)
    {

        Data = data.CreateBulletData();

        _renderer = GetComponent<SpriteRenderer>();
        _renderer.DOFade(1, 0.3f);

        Destroy(gameObject, value + 0.1f);

        DOTween.Sequence().
            AppendInterval(value - 0.2f).
            Append(_renderer.DOFade(0, 0.3f));

    }

    public void Shoot(float damage, float lifeTime, Transform parent)
    {
        target = parent;
        Init(lifeTime);

        GameManager.Instance.player.GetComponent<PlayerController>().playerData.AddModify(PlayerStatsType.MoveSpeed, damage, lifeTime);

    }

    private void Update()
    {

        transform.position = target.position;

    }

}
