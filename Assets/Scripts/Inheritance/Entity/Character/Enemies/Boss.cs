using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(TimersCalculator))]
    public class Boss : Character
    {
        [Header("Components")]
        [SerializeField] protected Rigidbody body;
        [SerializeField] protected TimersCalculator timers;

        protected float currentHealthToNextState = 0f;
        protected BossState bossState = BossState.First;
        protected int bossStateIndex = 0;

        protected virtual void Hit(float dmgs)
        {
            currentHealthToNextState -= dmgs;
            GameManager.state.CallOnBossHurt();
            if (currentHealthToNextState < 0) SwitchState();
        }

        protected virtual void SwitchState()
        {
            bossStateIndex++;
            if (bossStateIndex > 2) Death();
            else
            {
                GameManager.state.CallOnBossChangeState();
                bossState = (Team17.BallDash.BossState) bossStateIndex;
            }
        }

        protected virtual void Death()
        {
            GameManager.state.CallOnBossDeath();
        }

        #region Properties

        public float CurrentHealthToNextState { get => currentHealthToNextState;}
        public BossState BossState { get => bossState; }
        public int BossStateIndex { get => bossStateIndex;}

        #endregion
    }

    public enum BossState
    {
        First = 0,
        Second = 1,
        Third = 2
    };
}
