using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticleFeedback : Feedback
{
    [SerializeField]
    private ParticleObject _spawnParticle;

    public override void Play(float damage)
    {
        if (_spawnParticle != null)
            Instantiate(_spawnParticle, transform.position, Quaternion.identity).PlayOnceParticle();
        else
            Debug.LogWarning("SpawnParticleFeedback's spawnParticle is null");
    }
}
