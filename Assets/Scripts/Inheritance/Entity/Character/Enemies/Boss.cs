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
        [Header("Move list")]
        [SerializeField] protected BossAttack[] firstPhaseAttacks;
        [SerializeField] protected BossAttack[] secondPhaseAttacks;
        [SerializeField] protected BossAttack[] thirdPhaseAttacks;

        protected float currentHealthToNextState = 0f;
        protected BossState bossState = BossState.First;
        protected int bossStateIndex = 0;
        protected int nextAttackIndex = 0;
        protected bool canAttack = true;



        #region Monobehaviour callbacks

        protected override void Start()
        {
            base.Start();
            SetMoveListsTimers();
        }

        #endregion

        #region State management

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

        #endregion

        #region Attacks management

        protected virtual void ChooseAttack()
        {
            int index = 0;
            int lastPriority = -1;
            switch (bossState) 
            {
                case BossState.First:
                    for (int i = 0; i < firstPhaseAttacks.Length; i++)
                    {
                        if(firstPhaseAttacks[i].CanBeUsed)
                        {
                            
                        }
                    }
                    break;

                case BossState.Second:

                    break;

                case BossState.Third:

                    break;
            }
        }

        private bool CheckForAttackAvailability(BossAttack attack)
        {

            return false;
        }

        protected virtual void AttackEnd()
        {
            canAttack = true;
        }

        private void SetMoveListsTimers()
        {
            for (int i = 0; i < firstPhaseAttacks.Length; i++)
            {
                firstPhaseAttacks[i].Timers = timers;
            }

            for (int i = 0; i < secondPhaseAttacks.Length; i++)
            {
                secondPhaseAttacks[i].Timers = timers;
            }

            for (int i = 0; i < thirdPhaseAttacks.Length; i++)
            {
                thirdPhaseAttacks[i].Timers = timers;
            }
        }

        #endregion 

        #region Properties

        public float CurrentHealthToNextState { get => currentHealthToNextState;}
        public BossState BossState { get => bossState; }
        public int BossStateIndex { get => bossStateIndex;}

        #endregion

    }

    [System.Serializable]
    public class BossAttack
    {
        [Header ("Base parameter")]
        [SerializeField] private string name;
        [SerializeField] private BossAimZone zone;
        [SerializeField] private int priority = 0;
        [Header ("Attack parameter")]
        [Tooltip("Time it takes for the attack to be considered finished. After that time, the boss considers that he can choose and launch another attack.")]
        [SerializeField] private float timeToEnd = 3f;
        [Tooltip("Time it takes for the attack to be considered usable again after the boss used it once. During this time, the boss will ignore this attack.")]
        [SerializeField] private float coolDown = 4f;
        [SerializeField] private UnityEngine.Events.UnityEvent attack;

        private TimersCalculator timers;
        private bool canBeUsed = true;

        public void LaunchAttack(System.Action endAction)
        {

        }

        private void ResetAttack()
        {

        }

        public bool CanBeUsed { get => canBeUsed; }
        public TimersCalculator Timers { get => timers; set => timers = value; }
        public int Priority { get => priority; }
    }


    public enum BossState
    {
        First = 0,
        Second = 1,
        Third = 2
    };
}
