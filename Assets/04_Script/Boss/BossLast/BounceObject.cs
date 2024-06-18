using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{

    [SerializeField]
    private Vector3 _bounceValue;

    [SerializeField]
    private float _damageValue;

    private PlayerController _playerController;
    private Transform _playerTrm;
    private PlayerHP _playerHP;

    private void Start()
    {
        _bounceValue.z = 0;

        _playerTrm = GameManager.Instance.player;
        _playerController = GameManager.Instance.PlayerController;
        _playerHP = _playerTrm.GetComponent<PlayerHP>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player") == false)
        {
            return;
        }

        if (_playerController == null)
        {
            Debug.Log("A");
            return;
        }
        if (_playerTrm == null)
        {
            Debug.Log("B");
            return;
        }
        if (_playerHP == null)
        {
            Debug.Log("C");
            return;
        }


        Debug.Log("AB");

        _playerController.ChangeState(EnumPlayerState.Idle);
        _playerTrm.DOMove(_playerTrm.position + _bounceValue, 0.02f, false);


        _playerHP.Hit(_damageValue);



    }

}
