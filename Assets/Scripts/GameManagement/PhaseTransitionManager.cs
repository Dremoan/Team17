﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PhaseTransitionManager : MonoBehaviour
{

    [SerializeField] private PlayableDirector[] transitionPhases;
    [SerializeField] private int indexPhase = 0;


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            TriggerPhase();
        }
    }
    public void TriggerPhase()
    {
        transitionPhases[indexPhase].Play();
        indexPhase++;
    }
}

