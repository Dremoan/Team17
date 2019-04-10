using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.BallDash
{
    public class FightStateManager : Entity
    {
        [SerializeField] private FightGlobalState actualState = FightGlobalState.First;

        [Header("Bosses")]
        [SerializeField] private Boss firstPhaseBoss;
        [SerializeField] private Boss secondPhaseBoss;
        [SerializeField] private Boss thirdPhaseBoss;

        [Header("Rooms spawn points")]
        [SerializeField] private Transform firstSpawnPoint;
        [SerializeField] private Transform secondSpawnPoint;
        [SerializeField] private Transform thirdSpawnPoint;

        [Header("Cutscenes")]
        [SerializeField] private PlayableDirector introCutScene;
        [SerializeField] private PlayableDirector phaseTwoCutScene;
        [SerializeField] private PlayableDirector phaseThreeCutScene;
        [SerializeField] private PlayableDirector endCutScene;

        private int actualStateIndex = 0;

        protected override void Start()
        {
            base.Start();
            actualStateIndex = (int)actualState;
        }

        public override void OnBossChangeState()
        {
            base.OnBossChangeState();
            actualStateIndex++;
            if(actualStateIndex == 3)
            {
                // end level;
                return;
            }
            actualState = (FightGlobalState)actualStateIndex;
        }
        //modif
        [ContextMenu("Activate actual boss")]
        private void ActivateCurrentStateBoss()
        {
            switch (actualState)
            {
                case FightGlobalState.First:
                    firstPhaseBoss.gameObject.SetActive(true);
                    break;
                case FightGlobalState.Second:
                    secondPhaseBoss.gameObject.SetActive(true);
                    break;
                case FightGlobalState.Third:
                    thirdPhaseBoss.gameObject.SetActive(true);
                    break;
            }
        }
    }

    public enum FightGlobalState
    {
        First = 0,
        Second = 1,
        Third = 2
    };

    public enum BossPhaseState
    {
        Entry = 0,
        Attacking = 1,
        Exit = 2
    };
}
