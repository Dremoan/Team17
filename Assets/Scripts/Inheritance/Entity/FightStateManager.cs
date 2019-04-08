using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class FightStateManager : Entity
    {
        [SerializeField] private FightGlobalState actualState = FightGlobalState.First;

        [Header("Components")]
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private TimersCalculator timer;

        [Header("Bosses")]
        [SerializeField] private Boss firstPhaseBoss;
        [SerializeField] private Boss secondPhaseBoss;
        [SerializeField] private Boss thirdPhaseBoss;

        [Header("Rooms spawn points")]
        [SerializeField] private Transform firstSpawnPoint;
        [SerializeField] private Transform secondSpawnPoint;
        [SerializeField] private Transform thirdSpawnPoint;

        [Header("Virtual cameras")]
        [SerializeField] private Cinemachine.CinemachineVirtualCamera firstCamera;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera secondCamera;
        [SerializeField] private Cinemachine.CinemachineVirtualCamera thirdCamera;
        [SerializeField] private float travelTime;

        private int actualStateIndex = 0;

        protected override void Start()
        {
            base.Start();
            actualStateIndex = (int)actualState;
            SetStateCameraPriorities();
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
            SetStateCameraPriorities();
            timer.LaunchNewTimer(travelTime, ActivateCurrentStateBoss);
        }

        [ContextMenu ("Set cameras priorities")]
        private void SetStateCameraPriorities()
        {
            switch(actualState)
            {
                case FightGlobalState.First:
                    firstCamera.Priority = 3;
                    secondCamera.Priority = 1;
                    thirdCamera.Priority = 1;
                    break;
                case FightGlobalState.Second:
                    firstCamera.Priority = 1;
                    secondCamera.Priority = 3;
                    thirdCamera.Priority = 1;
                    break;
                case FightGlobalState.Third:
                    firstCamera.Priority = 1;
                    secondCamera.Priority = 1;
                    thirdCamera.Priority = 3;
                    break;
            }
            GameManager.state.CallOnStateIntroBegins(actualState);
        }

        [ContextMenu("Activate actual boss")]
        private void ActivateCurrentStateBoss()
        {
            switch (actualState)
            {
                case FightGlobalState.First:
                    firstPhaseBoss.gameObject.SetActive(true);
                    character.TeleportToRoom(firstSpawnPoint);
                    break;
                case FightGlobalState.Second:
                    secondPhaseBoss.gameObject.SetActive(true);
                    character.TeleportToRoom(secondSpawnPoint);
                    break;
                case FightGlobalState.Third:
                    thirdPhaseBoss.gameObject.SetActive(true);
                    character.TeleportToRoom(thirdSpawnPoint);
                    break;
            }
            GameManager.state.CallOnStateIntroEnds(actualState);
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
