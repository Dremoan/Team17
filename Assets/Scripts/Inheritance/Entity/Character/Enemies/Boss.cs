using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.BallDash
{
    [RequireComponent(typeof(TimersCalculator))]
    public class Boss : Character, IBallHitable
    {
        [Header("Components")]
        [SerializeField] private TimersCalculator timers;
        [Header("Health and state")]
        [SerializeField] private BossPhaseState currentState = BossPhaseState.Entry;
        [SerializeField] private float health = 50f;

        [Header("Rooms zero")]
        [SerializeField] private Transform roomZero;

        [Header("Patterns")]
        [SerializeField] private BossPattern entryPattern;
        [SerializeField] private BossPattern[] patternList;
        [SerializeField] private BossPattern exitPattern;

        private CutSceneEvent entryBeginsEvent;
        private CutSceneEvent entryEndsEvent;
        private CutSceneEvent exitBeginsEvent;
        private CutSceneEvent exitEndsEvent;

        protected float currentHealth = 0f;
        protected bool canAttack = false;

        #region Monobehaviour callbacks

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SetHealth();
            SetMoveListsTimers();
            Attack(-1);
        }

        protected override void Update()
        {
            base.Update();
            ChooseAttack();
        }

        #endregion

        #region Entity CallBacks

        #endregion

        #region State management

        public void Hit(float dmgs)
        {
            currentHealth -= dmgs;
            GameManager.state.CallOnBossHurt();
            Debug.Log(gameObject.name + " has " + currentHealth + " hp. Damaged " + dmgs);
            if (currentHealth < 0) Death();
        }

        private void Death()
        {
            currentState = BossPhaseState.Exit;
        }

        #endregion

        #region Attacks management

        private void ChooseAttack()
        {
            if(canAttack)
            {
                int index = -1;
                int lastPriority = -1;

                // go through all patterns and choose;
                for (int i = 0; i < patternList.Length; i++)
                {
                    if(patternList[i].Priority > lastPriority)
                    {
                        if (patternList[i].IsUsableAndUseful(roomZero, GameManager.state.PlayerGameObject.GetComponent<PlayerProjectile>().FuturPositionInArena()))
                        {
                            index = i;
                            lastPriority = patternList[i].Priority;
                        }
                    }
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
            switch(currentState)
            {
                case BossPhaseState.Entry:
                    if(entryBeginsEvent != null) entryBeginsEvent.Invoke();
                    entryPattern.LaunchAttack(EntryEnd);
                    break;
                case BossPhaseState.Attacking:
                    patternList[index].LaunchAttack(AttackEnd);
                    break;
                case BossPhaseState.Exit:
                    //call exit begins
                    if(exitBeginsEvent != null)exitBeginsEvent.Invoke();
                    exitPattern.LaunchAttack(ExitEnd);
                    break;

            }
            canAttack = false;
        }

        private void EntryEnd()
        {
            canAttack = true;
            if(entryBeginsEvent != null)entryEndsEvent.Invoke();
            currentState = BossPhaseState.Attacking;
        }

        private void AttackEnd()
        {
            canAttack = true;
        }

        private void ExitEnd()
        {
            canAttack = false;
            //call exit end
            GameManager.state.CallOnBossChangeState();
            if(exitEndsEvent != null) exitEndsEvent.Invoke();
            gameObject.SetActive(false);
        }

        private void SetMoveListsTimers()
        {
            entryPattern.Timers = timers;
            for (int i = 0; i < patternList.Length; i++)
            {
                patternList[i].Timers = timers;
            }
            exitPattern.Timers = timers;
        }

        private void SetHealth()
        {
            currentHealth = health;
        }

        public void StopAllAttacks()
        {
            timers.DeleteAllTimers();
            canAttack = false;
        }

        public void ResumeAllAttacks()
        {
            canAttack = true;
        }

        #endregion

        #region Properties

        public float CurrentHealthToNextState { get => currentHealth;}
        public CutSceneEvent EntryBeginsEvent { get => entryBeginsEvent; set => entryBeginsEvent = value; }
        public CutSceneEvent EntryEndsEvent { get => entryEndsEvent; set => entryEndsEvent = value; }
        public CutSceneEvent ExitBeginsEvent { get => exitBeginsEvent; set => exitBeginsEvent = value; }
        public CutSceneEvent ExitEndsEvent { get => exitEndsEvent; set => exitEndsEvent = value; }

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
        [Tooltip("Time it takes for the attack to be considered finished. After that time, the boss can choose and launch another attack.")]
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
            if (timers == null) Debug.Log("null");
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
}
