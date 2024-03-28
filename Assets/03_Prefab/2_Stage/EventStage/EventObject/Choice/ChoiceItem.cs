using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChoiceItem : MonoBehaviour
{
    [SerializeField]
    private ItemInfoListSO _itemListSO;
    [SerializeField]
    private ParticleSystem _particleSystem;

    [SerializeField]
    private List<Transform> _randomItemSpawnPos;
    private List<Item> _spawnItems = new List<Item>();

    void Start()
    {
        SpawnItem();
        
    }
    private void SpawnItem()
    {
        List<Item> items = new List<Item>();
        _itemListSO.ItemInfoList.ForEach(item => { items.Add(item.ItemObject); });

        // shuffle
        for(int i = 0; i < _randomItemSpawnPos.Count; i++)
        {
            int randomIdx = Random.Range(i, items.Count);

            Item item = Instantiate(items[randomIdx], _randomItemSpawnPos[i].position, Quaternion.identity);
            item.OnInteractItem += HandleDeleteItem;
            _spawnItems.Add(item);

            items[randomIdx] = items[i];
        }
    }

    private void HandleDeleteItem(Transform itemTrm)
    {
        ParticleSystem spawnParticle = Instantiate(_particleSystem, itemTrm.position, Quaternion.identity);
        spawnParticle.transform.SetParent(transform);
        spawnParticle.Play();

        foreach (Item item in _spawnItems)
            Destroy(item.gameObject);

        _spawnItems.Clear();
    }
}
