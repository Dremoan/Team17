using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.BallDash
{
    /*
    [System.Serializable]
    public class MyCustomCoolEvent : UnityEvent<Transform, Rigidbody, Animator>
    {

    }
    /* */

    [RequireComponent(typeof(TimersCalculator))]
    public class Boss : Character, IBallHitable
    {
        [Header("Components")]
        [SerializeField] private TimersCalculator timers;
        [Header("Health and state")]
        [SerializeField] private BossState bossState = BossState.First;
        [SerializeField] private float firstPhaseHealth = 50f;
        [SerializeField] private float secondPhaseHealth = 75f;
        [SerializeField] private float thirdPhaseHealth = 100f;

        [Header("Rooms zeros")]
        [SerializeField] private Transform phaseOneZero;
        [SerializeField] private Transform phaseTwoZero;
        [SerializeField] private Transform phaseThreeZero;

        [Header("Intro move")]
        [SerializeField] private BossAttack introMove;
        [Header("Move list")]
        [SerializeField] private BossAttack[] firstPhaseAttacks;
        [SerializeField] private BossAttack[] secondPhaseAttacks;
        [SerializeField] private BossAttack[] thirdPhaseAttacks;

        protected float currentHealthToNextState = 0f;
        protected int bossStateIndex = 0;
        protected bool canAttack = false;

        #region Monobehaviour callbacks

        protected override void Start()
        {
            base.Start();
            SetMoveListsTimers();
            SetHealth();
            bossStateIndex = (int)bossState;
        }

        protected override void Update()
        {
            base.Update();
            ChooseAttack();
            if(Input.GetKeyDown(KeyCode.A))
            {
                canAttack = true;
            }
        }

        #endregion

        #region State management

        public void Hit(float dmgs)
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
                SetHealth();
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
            if(canAttack)
            {
                int index = -1;
                int lastPriority = -1;
                switch (bossState)
                {
                    case BossState.Intro:

                        break;

                    case BossState.First:
                        for (int i = 0; i < firstPhaseAttacks.Length; i++)
                        {
                            if (firstPhaseAttacks[i].IsUsableAndUseful(phaseOneZero, GameManager.state.PlayerGameObject.GetComponent<PlayerProjectile>().FuturPositionInArena()))
                            {
                                if (firstPhaseAttacks[i].Priority > lastPriority)
                                {
                                    index = i;
                                    lastPriority = firstPhaseAttacks[i].Priority;
                                }
                            }
                        }
                        break;

                    case BossState.Second:
                        for (int i = 0; i < secondPhaseAttacks.Length; i++)
                        {
                            if (secondPhaseAttacks[i].IsUsableAndUseful(phaseTwoZero, GameManager.state.PlayerGameObject.GetComponent<PlayerProjectile>().FuturPositionInArena()))
                            {
                                if (secondPhaseAttacks[i].Priority > lastPriority)
                                {
                                    index = i;
                                    lastPriority = secondPhaseAttacks[i].Priority;
                                }
                            }
                        }
                        break;

                    case BossState.Third:
                        for (int i = 0; i < thirdPhaseAttacks.Length; i++)
                        {
                            if (thirdPhaseAttacks[i].IsUsableAndUseful(phaseThreeZero, GameManager.state.PlayerGameObject.GetComponent<PlayerProjectile>().FuturPositionInArena()))
                            {
                                if (thirdPhaseAttacks[i].Priority > lastPriority)
                                {
                                    index = i;
                                    lastPriority = thirdPhaseAttacks[i].Priority;
                                }
                            }
                        }
                        break;
                }
                if(index == -1)
                {
                    return;
                }
                else
                {
                    Attack(index);
                }
            }
        }

        protected virtual void Attack(int index)
        {
            switch (bossState)
            {
                case BossState.First:
                    firstPhaseAttacks[index].LaunchAttack(AttackEnd);
                    break;
                case BossState.Second:
                    secondPhaseAttacks[index].LaunchAttack(AttackEnd);
                    break;
                case BossState.Third:
                    thirdPhaseAttacks[index].LaunchAttack(AttackEnd);
                    break;
            }
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

        private void SetHealth()
        {
            switch(bossState)
            {
                case BossState.First:
                    currentHealthToNextState = firstPhaseHealth;
                    break;
                case BossState.Second:
                    currentHealthToNextState = secondPhaseHealth;
                    break;
                case BossState.Third:
                    currentHealthToNextState = thirdPhaseHealth;
                    break;
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
        [SerializeField] private UnityEngine.Events.UnityEvent pattern;

        private TimersCalculator timers;
        private bool canBeUsed = true;
        private System.Action endAction;

        public void LaunchAttack(System.Action endAct)
        {
            Debug.Log(name);
            pattern.Invoke();
            endAction = endAct;
            canBeUsed = false;
            timers.LaunchNewTimer(timeToEnd, EndAttack);
        }

        private void EndAttack()
        {
            endAction.Invoke();
            timers.LaunchNewTimer(coolDown, ResetAttack);
        }

        private void ResetAttack()
        {
            canBeUsed = true;
        }

        public bool IsUsableAndUseful(Transform zero, Vector3 targetPos)
        {
            if (!canBeUsed) return false;
            if (zone.Contains(zero, targetPos)) return true;
            return false;
        }

        public bool CanBeUsed { get => canBeUsed; }
        public TimersCalculator Timers { get => timers; set => timers = value; }
        public int Priority { get => priority; }
    }


    public enum BossState
    {
        Intro = 0,
        First = 1,
        Second = 2,
        Third = 3
    };
}
