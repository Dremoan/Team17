using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.StreetHunt
{
    public class FightStateManager : Entity
    {
        [SerializeField] private FightGlobalState actualState = FightGlobalState.First;

        [Header("Bosses")]
        [SerializeField] private Boss firstPhaseBoss;
        [SerializeField] private Boss secondPhaseBoss;
        [SerializeField] private Boss thirdPhaseBoss;

        [Header("Rooms spawn points")]
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private Transform firstSpawnPoint;
        [SerializeField] private Transform secondSpawnPoint;
        [SerializeField] private Transform thirdSpawnPoint;

        [Header("Cutscenes")]
        [SerializeField] private PlayableDirector introCutScene;
        [SerializeField] private PlayableDirector phaseTwoCutScene;
        [SerializeField] private PlayableDirector phaseThreeCutScene;
        [SerializeField] private PlayableDirector endCutScene;

        private int actualStateIndex = 0;

        protected override void OnEnable()
        {
            base.OnEnable();
            LinkCutScenesEvents();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnlinkCutScenesEvents();
        }

        protected override void Start()
        {
            base.Start();
            actualStateIndex = (int)actualState;
            ActivateCurrentStateBoss();
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
                    //character.TeleportToRoom(firstSpawnPoint);
                    break;
                case FightGlobalState.Second:
                    secondPhaseBoss.gameObject.SetActive(true);
                    //character.TeleportToRoom(secondSpawnPoint);
                    break;
                case FightGlobalState.Third:
                    thirdPhaseBoss.gameObject.SetActive(true);
                    //character.TeleportToRoom(thirdSpawnPoint);
                    break;
            }
        }

        private void LinkCutScenesEvents()
        {
            firstPhaseBoss.EntryBeginsEvent += introCutScene.Play;
            firstPhaseBoss.EntryBeginsEvent += GameManager.state.CallOnIntroCutScene;
            firstPhaseBoss.EntryEndsEvent += introCutScene.Stop;
            firstPhaseBoss.EntryEndsEvent += GameManager.state.CallOnIntroCutSceneEnds;

            // phase one fight

            firstPhaseBoss.ExitBeginsEvent += phaseTwoCutScene.Play;
            firstPhaseBoss.ExitBeginsEvent += GameManager.state.CallOnPhaseTwoCutScene;
            firstPhaseBoss.ExitEndsEvent += ActivateCurrentStateBoss; // second boss active
            firstPhaseBoss.ExitEndsEvent += GameManager.state.CallOnPhaseTwoCutSceneEnds;


            // phase two fight

            secondPhaseBoss.ExitBeginsEvent += phaseThreeCutScene.Play;
            secondPhaseBoss.ExitBeginsEvent += GameManager.state.CallOnPhaseThreeCutScene;
            secondPhaseBoss.ExitEndsEvent += ActivateCurrentStateBoss; // third boss active
            secondPhaseBoss.ExitEndsEvent += GameManager.state.CallOnPhaseThreeCutSceneEnds;

            // phase three fight

            thirdPhaseBoss.ExitBeginsEvent += endCutScene.Play;
            thirdPhaseBoss.ExitBeginsEvent += GameManager.state.CallOnEndCutScene;
            thirdPhaseBoss.ExitEndsEvent += GameManager.state.CallOnEndCutSceneEnds;

            // fight end

        }

        private void UnlinkCutScenesEvents()
        {
            firstPhaseBoss.EntryBeginsEvent -= introCutScene.Play;
            firstPhaseBoss.EntryBeginsEvent -= GameManager.state.CallOnIntroCutScene;
            firstPhaseBoss.EntryEndsEvent -= introCutScene.Stop;
            firstPhaseBoss.EntryEndsEvent -= GameManager.state.CallOnIntroCutSceneEnds;

            // phase one fight

            firstPhaseBoss.ExitBeginsEvent -= phaseTwoCutScene.Play;
            firstPhaseBoss.ExitBeginsEvent -= GameManager.state.CallOnPhaseTwoCutScene;
            firstPhaseBoss.ExitEndsEvent -= ActivateCurrentStateBoss; // second boss active
            firstPhaseBoss.ExitEndsEvent -= GameManager.state.CallOnPhaseTwoCutSceneEnds;


            // phase two fight

            secondPhaseBoss.ExitBeginsEvent -= phaseThreeCutScene.Play;
            secondPhaseBoss.ExitBeginsEvent -= GameManager.state.CallOnPhaseThreeCutScene;
            secondPhaseBoss.ExitEndsEvent -= ActivateCurrentStateBoss; // third boss active
            secondPhaseBoss.ExitEndsEvent -= GameManager.state.CallOnPhaseThreeCutSceneEnds;

            // phase three fight

            thirdPhaseBoss.ExitBeginsEvent -= endCutScene.Play;
            thirdPhaseBoss.ExitBeginsEvent -= GameManager.state.CallOnEndCutScene;
            thirdPhaseBoss.ExitEndsEvent -= GameManager.state.CallOnEndCutSceneEnds;

            // fight end
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

    public delegate void CutSceneEvent();
}
