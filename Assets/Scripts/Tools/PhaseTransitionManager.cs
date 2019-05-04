using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.StreetHunt
{
    public class PhaseTransitionManager : Entity
    {
        [SerializeField] private PlayableDirector[] transitionPhases;

        protected override void Start()
        {
            base.Start();
        }

        public void TriggerPhase(FightGlobalState actualState)
        {
            if(transitionPhases[(int)actualState] != null) transitionPhases[(int)actualState].Play();
        }
    }
}

