using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.BallDash
{
    public class PhaseTransitionManager : Entity
    {
        [SerializeField] private PlayableDirector[] transitionPhases;

        protected override void Start()
        {
            base.Start();
        }

        public void TriggerPhase(BossGlobalState actualState)
        {
            if(transitionPhases[(int)actualState] != null) transitionPhases[(int)actualState].Play();
        }
    }
}

