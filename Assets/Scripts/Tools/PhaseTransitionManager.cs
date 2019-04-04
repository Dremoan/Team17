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

        public override void OnBossChangeState(BossState targetState)
        {
            base.OnBossChangeState(targetState);
            TriggerPhase(targetState);
        }

        public void TriggerPhase(BossState actualState)
        {
            transitionPhases[(int)actualState].Play();
        }
    }
}

