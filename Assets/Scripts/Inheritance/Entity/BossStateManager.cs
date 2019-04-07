using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class BossStateManager : Entity
    {
        [SerializeField] private BossGlobalState actualState = BossGlobalState.First;
        [Header("Bosses")]
        [SerializeField] private Boss firstPhaseBoss;
        [SerializeField] private Boss secondPhaseBoss;
        [SerializeField] private Boss thirdPhaseBoss;

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
            //if(actualStateIndex == 3) // endlevel;
            Debug.Log(actualStateIndex);
            actualState = (BossGlobalState)actualStateIndex;
            ActivateActualBoss();
        }

        [ContextMenu("Activate actual boss")]
        private void ActivateActualBoss()
        {
            switch(actualState)
            {
                case BossGlobalState.First:
                    firstPhaseBoss.gameObject.SetActive(true);
                    break;
                case BossGlobalState.Second:
                    secondPhaseBoss.gameObject.SetActive(true);
                    break;
                case BossGlobalState.Third:
                    thirdPhaseBoss.gameObject.SetActive(true);
                    break;
            }
        }


    }

    public enum BossGlobalState
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
