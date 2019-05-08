using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.StreetHunt
{
    [RequireComponent(typeof(TimersCalculator))]
    public class Boss : Character, IBallHitable
    {
        [Header("Components")]
        [SerializeField] private TimersCalculator timers;
        [SerializeField] private SpeedPortalManager portalManager;
        [Header("Health and state")]
        [SerializeField] private BossPhaseState currentState = BossPhaseState.Entry;
        [SerializeField] private float health = 50f;

        [Header("Rooms zero")]
        [SerializeField] private Transform roomZero;

        [Header("Patterns states serie")]
        [SerializeField] private BossAttackState[] attackStates;

        [Header("Patterns")]
        [SerializeField] private BossPattern entryPattern;
        [SerializeField] private BossPattern[] easyPatterns;
        [SerializeField] private BossPattern[] mediumPatterns;
        [SerializeField] private BossPattern[] hardPatterns;
        [SerializeField] private BossPattern exitPattern;

        private CutSceneEvent entryBeginsEvent;
        private CutSceneEvent entryEndsEvent;
        private CutSceneEvent exitBeginsEvent;
        private CutSceneEvent exitEndsEvent;

        private int currentAttackStateIndex = 0;
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
            SetMoveListsAttributes();
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

        public void Hit(int index, float dmgs)
        {
            currentHealth -= dmgs;
            Debug.Log(gameObject.name + " has " + currentHealth + " hp. Damaged " + dmgs);
            if (currentHealth < 0)
            {
                Death();
            }
            else
            {
                GameManager.state.CallOnBossHurt(index, dmgs);
            }
        }

        private void Death()
        {
            currentState = BossPhaseState.Exit;
            GameManager.state.CallOnBossDeath();
        }

        #endregion

        #region Attacks management

        private void ChooseAttack()
        {
            if (canAttack)
            {
                int index = -1;
                int lastPriority = -1;

                // go through all patterns and choose;
                switch (attackStates[currentAttackStateIndex])
                {
                    case BossAttackState.Easy:
                        for (int i = 0; i < easyPatterns.Length; i++)
                        {
                            if (easyPatterns[i].Priority > lastPriority)
                            {
                                if (easyPatterns[i].IsUsableAndUseful(roomZero, GameManager.state.BallGameObject.transform.position))
                                {
                                    index = i;
                                    lastPriority = easyPatterns[i].Priority;
                                }
                            }
                        }
                        break;

                    case BossAttackState.Medium:
                        for (int i = 0; i < mediumPatterns.Length; i++)
                        {
                            if (mediumPatterns[i].Priority > lastPriority)
                            {
                                if (mediumPatterns[i].IsUsableAndUseful(roomZero, GameManager.state.BallGameObject.transform.position))
                                {
                                    index = i;
                                    lastPriority = mediumPatterns[i].Priority;
                                }
                            }
                        }
                        break;

                    case BossAttackState.Hard:
                        for (int i = 0; i < hardPatterns.Length; i++)
                        {
                            if (hardPatterns[i].Priority > lastPriority)
                            {
                                if (hardPatterns[i].IsUsableAndUseful(roomZero, GameManager.state.BallGameObject.transform.position))
                                {
                                    index = i;
                                    lastPriority = hardPatterns[i].Priority;
                                }
                            }
                        }
                        break;
                }

                if (index == -1)
                {
                    Debug.LogWarning("No useful attack found");
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
            switch (currentState)
            {
                case BossPhaseState.Entry:
                    if (entryBeginsEvent != null) entryBeginsEvent.Invoke();
                    entryPattern.LaunchAttack(EntryEnd);
                    break;
                case BossPhaseState.Attacking:

                    switch (attackStates[currentAttackStateIndex])
                    {
                        case BossAttackState.Easy:
                            easyPatterns[index].LaunchAttack(AttackEnd);
                            currentAttackStateIndex++;
                            if (currentAttackStateIndex > attackStates.Length - 1) currentAttackStateIndex = 0;
                            break;

                        case BossAttackState.Medium:
                            mediumPatterns[index].LaunchAttack(AttackEnd);
                            currentAttackStateIndex++;
                            if (currentAttackStateIndex > attackStates.Length - 1) currentAttackStateIndex = 0;
                            break;

                        case BossAttackState.Hard:
                            hardPatterns[index].LaunchAttack(AttackEnd);
                            currentAttackStateIndex++;
                            if (currentAttackStateIndex > attackStates.Length - 1) currentAttackStateIndex = 0;
                            break;
                    }

                    break;
                case BossPhaseState.Exit:
                    //call exit begins
                    if (exitBeginsEvent != null) exitBeginsEvent.Invoke();
                    exitPattern.LaunchAttack(ExitEnd);
                    break;

            }
            canAttack = false;
        }

        private void EntryEnd()
        {
            canAttack = true;
            if (entryBeginsEvent != null) entryEndsEvent.Invoke();
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
            if (exitEndsEvent != null) exitEndsEvent.Invoke();
            gameObject.SetActive(false);
        }

        private void SetMoveListsAttributes()
        {
            entryPattern.Timers = timers;

            for (int i = 0; i < easyPatterns.Length; i++)
            {
                easyPatterns[i].Timers = timers;
            }

            for (int i = 0; i < mediumPatterns.Length; i++)
            {
                mediumPatterns[i].Timers = timers;
            }

            for (int i = 0; i < hardPatterns.Length; i++)
            {
                hardPatterns[i].Timers = timers;
            }

            exitPattern.Timers = timers;

            entryPattern.PortalManager = portalManager;

            for (int i = 0; i < easyPatterns.Length; i++)
            {
                easyPatterns[i].PortalManager = portalManager;
            }

            for (int i = 0; i < mediumPatterns.Length; i++)
            {
                mediumPatterns[i].PortalManager = portalManager;
            }

            for (int i = 0; i < hardPatterns.Length; i++)
            {
                hardPatterns[i].PortalManager = portalManager;
            }

            exitPattern.PortalManager = portalManager;
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

        public float CurrentHealthToNextState { get => currentHealth; }
        public CutSceneEvent EntryBeginsEvent { get => entryBeginsEvent; set => entryBeginsEvent = value; }
        public CutSceneEvent EntryEndsEvent { get => entryEndsEvent; set => entryEndsEvent = value; }
        public CutSceneEvent ExitBeginsEvent { get => exitBeginsEvent; set => exitBeginsEvent = value; }
        public CutSceneEvent ExitEndsEvent { get => exitEndsEvent; set => exitEndsEvent = value; }
        public Transform RoomZero { get => roomZero; }

        #endregion

        [ContextMenu("Reference portal manager")]
        public void ReferencePortalManager()
        {
            portalManager = GameObject.Find("SpeedPortalsManager").GetComponent<SpeedPortalManager>();
        }
    }

    [System.Serializable]
    public class BossPattern
    {
        [Header("Base parameter")]
        [SerializeField] private string name;
        [SerializeField] private BossAimZone zone;
        [SerializeField] private int priority = 0;
        [Header("Attack parameter")]
        [Tooltip("Time it takes for the attack to be considered finished. After that time, the boss can choose and launch another attack.")]
        [SerializeField] private float timeToEnd = 3f;
        [Tooltip("Time it takes for spawned portal to disapear.")]
        [SerializeField] private float timeForPortalsToDisapear = 2.5f;
        [Tooltip("Time it takes for the attack to be considered usable again after the boss used it once. During this time, the boss will ignore this attack.")]
        [SerializeField] private float coolDown = 4f;
        [SerializeField] private UnityEngine.Events.UnityEvent pattern;
        [SerializeField] private PortalPlacement[] portals;

        private SpeedPortalManager portalManager;
        private TimersCalculator timers;
        private bool canBeUsed = true;
        private System.Action endAction;

        public void LaunchAttack(System.Action endAct)
        {
            Debug.Log(name);
            pattern.Invoke();
            endAction = endAct;
            canBeUsed = false;
            for (int i = 0; i < portals.Length; i++)
            {
                portalManager.SpawnPortal(portals[i].Position, portals[i].Rotation);
            }
            timers.LaunchNewTimer(timeToEnd, EndAttack);
        }

        private void EndAttack()
        {
            portalManager.DeactivateAllPortals();
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
        public SpeedPortalManager PortalManager { get => portalManager; set => portalManager = value; }
    }

    [System.Serializable]
    public struct PortalPlacement
    {
        [SerializeField] private Vector3 position;
        [SerializeField] private float rotation;

        public Vector3 Position { get => position; set => position = value; }
        public float Rotation { get => rotation; set => rotation = value; }
    }

    public enum BossAttackState
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }
}
