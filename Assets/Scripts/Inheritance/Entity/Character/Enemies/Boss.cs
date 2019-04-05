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

        [Header("Intro pattern")]
        [SerializeField] private BossPattern introPattern;
        [Header("Pattern list")]
        [SerializeField] private BossPattern[] firstPhaseAttacks;
        [SerializeField] private BossPattern[] secondPhaseAttacks;
        [SerializeField] private BossPattern[] thirdPhaseAttacks;

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
        }

        #endregion

        #region Entity CallBacks

        public override void OnIntroLaunched()
        {
            base.OnIntroLaunched();
            canAttack = true;
        }

        #endregion

        #region State management

        public void Hit(float dmgs)
        {
            currentHealthToNextState -= dmgs;
            GameManager.state.CallOnBossHurt();
            Debug.Log(gameObject.name + " has " + currentHealthToNextState + " hp. Damaged " + dmgs);
            if (currentHealthToNextState < 0) SwitchState();
        }

        private void SwitchState()
        {
            bossStateIndex++;
            if (bossStateIndex > 3) Death();
            else
            {
                bossState = (Team17.BallDash.BossState) bossStateIndex;
                Debug.Log("Switched to " + bossState);
                GameManager.state.CallOnBossChangeState(bossState);
                SetHealth();
            }
        }

        private void Death()
        {
            GameManager.state.CallOnBossDeath();
        }

        public void EndLevel()
        {
            GameManager.state.CallOnLevelEnd();
        }

        #endregion

        #region Attacks management

        private void ChooseAttack()
        {
            if(canAttack)
            {
                int index = -1;
                int lastPriority = -1;
                switch (bossState)
                {
                    case BossState.Intro:
                        Debug.Log("Launched intro pattern");
                        Attack(-1);
                        return;

                    case BossState.First:
                        Debug.Log("Launched first phase attack");
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

        private void Attack(int index)
        {
            canAttack = false;
            switch (bossState)
            {
                case BossState.Intro:
                    introPattern.LaunchAttack(IntroEnd);
                    break;
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

        public void IntroEnd()
        {
            canAttack = true;
            GameManager.state.CallOnBossBeginsPattern();
            SwitchState();
        }

        private void AttackEnd()
        {
            canAttack = true;
        }

        private void SetMoveListsTimers()
        {
            introPattern.Timers = timers;
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
    public class BossPattern
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
