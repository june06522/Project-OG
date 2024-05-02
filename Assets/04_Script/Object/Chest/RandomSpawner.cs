using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class RandomSpawner : MonoBehaviour, IInteractable
{

    [SerializeField] List<GameObject> weapons = new();
    [SerializeField] Collider2D _collider;
    bool active = true;

    public void OnInteract()
    {

        Spawn();

    }

    private void Spawn()
    {

        if (active == false) return;

        _collider.enabled = false;
        active = false;
        var idx = Random.Range(0, weapons.Count - 1);
        var obj = Instantiate(weapons[idx], transform.position, Quaternion.identity);
        obj.transform.DOJump(transform.position + Vector3.up, 1.5f, 1, 0.7f);

    }

}
