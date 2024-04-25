using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleObject : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void PlayOnceParticle()
    {
        StartCoroutine(PlayParticleCoroutine());
    }

    IEnumerator PlayParticleCoroutine()
    {
        _particleSystem.Play();

        yield return new WaitUntil(() => !_particleSystem.isPlaying);

        Destroy(gameObject);
    }

}
