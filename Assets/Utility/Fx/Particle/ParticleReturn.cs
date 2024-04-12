using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleReturn : MonoBehaviour
{
    public Action OnReturn;

    void Start()
    {
        var main = GetComponent<ParticleSystem>().main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        OnReturn?.Invoke();
    }
}
