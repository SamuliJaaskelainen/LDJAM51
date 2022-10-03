using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSelfdestruct : MonoBehaviour
{
    public ParticleSystem partice;

    void Update()
    {
        if (!partice.isPlaying)
        {
            Destroy(gameObject, partice.totalTime);
        }
    }
}

