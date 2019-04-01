using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(TimersCalculator))]
    public class Boss : Character, IBallHitable
    {
        [Header("Components")]
        [SerializeField] protected Rigidbody body;
        [SerializeField] protected TimersCalculator timers;
        [Header("Health and state")]
        [SerializeField] protected BossState bossState = BossState.First;
        [SerializeField] protected float firstPhaseHealth = 50f;
        [SerializeField] protected float secondPhaseHealth = 75f;
        [SerializeField] protected float thirdPhaseHealth = 100f;

        [Header("Move list")]
        [SerializeField] protected BossAttack[] firstPhaseAttacks;
        [SerializeField] protected BossAttack[] secondPhaseAttacks;
        [SerializeField] protected BossAttack[] thirdPhaseAttacks;

        [SerializeField] protected Transform testTarget; // TO DO : find a way to get player's position instead of that

        protected float currentHealthToNextState = 0f;
        protected int bossStateIndex = 0;
        protected int nextAttackIndex = 0;
        protected bool canAttack = true;

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
            Attack();
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
            int index = -1;
            int lastPriority = -1;
            switch (bossState) 
            {
                case BossState.First:
                    for (int i = 0; i < firstPhaseAttacks.Length; i++)
                    {
                        if(firstPhaseAttacks[i].IsUsableAndUseful(GameManager.state.PlayerGameObject.transform.position))
                        {
                            if(firstPhaseAttacks[i].Priority > lastPriority)
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
                        if (secondPhaseAttacks[i].IsUsableAndUseful(GameManager.state.PlayerGameObject.transform.position))
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
                        if (thirdPhaseAttacks[i].IsUsableAndUseful(GameManager.state.PlayerGameObject.transform.position))
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
            if (index == -1) return;
            nextAttackIndex = index;
        }

        protected virtual void Attack()
        {
            if(canAttack)
            {
                switch (bossState)
                {
                    case BossState.First:
                        firstPhaseAttacks[nextAttackIndex].LaunchAttack(AttackEnd);
                        break;
                }
                canAttack = false;
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
        [SerializeField] private UnityEngine.Events.UnityEvent attack;

        private TimersCalculator timers;
        private bool canBeUsed = true;
        private System.Action endAction;

        public void LaunchAttack(System.Action endAct)
        {
            Debug.Log(name);
            attack.Invoke();
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

        public bool IsUsableAndUseful(Vector3 targetPos)
        {
            if (!canBeUsed) return false;
            if (zone.Contains(targetPos)) return true;
            return false;
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
