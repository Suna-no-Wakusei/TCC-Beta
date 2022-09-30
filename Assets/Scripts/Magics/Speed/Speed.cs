using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed : MonoBehaviour
{
    public ParticleSystem speedEffect;

    public void PlayEffect()
    {
        speedEffect.Play();
    }

}
