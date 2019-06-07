using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class Entity : MonoBehaviour
    {
        #region Monobehaviour callbacks

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected virtual void FixedUpdate()
        {

        }

        protected virtual void OnEnable()
        {
            GameManager.state.RegisterEntity(this);
        }

        protected virtual void OnDisable()
        {
            GameManager.state.UnregisterEntity(this);
        }

        #endregion

        #region Entity callbacks

        public virtual void OnIntroCutScene()
        {

        }

        public virtual void OnIntroCutSceneEnds()
        {

        }

        public virtual void OnPhaseTwoCutScene()
        {

        }

        public virtual void OnPhaseTwoCutSceneEnds()
        {

        }

        public virtual void OnPhaseThreeCutScene()
        {

        }

        public virtual void OnPhaseThreeCutSceneEnds()
        {

        }

        public virtual void OnEndCutScene()
        {

        }

        public virtual void OnEndCutSceneEnds()
        {

        }

        public virtual void OnBossBeginsPatterns()
        {

        }

        public virtual void OnPlayerTeleport()
        {

        }

        public virtual void OnBallShot()
        {

        }

        public virtual void OnBallIncreasePowerGroup()
        {

        }

        public virtual void OnBallDecreasePowerGroup()
        {

        }

        public virtual void OnCharacterStunned()
        {

        }

        public virtual void OnBallDestroyed()
        {

        }

        public virtual void OnBallHit(int powerGroupIndex, float hitPower)
        {

        }

        public virtual void OnBallSlowed()
        {

        }

        public virtual void OnBallBounced()
        {

        }

        public virtual void OnBossHurt(int powerGroupIndex, float hitPower)
        {

        }

        public virtual void OnBossChangeState()
        {

        }

        public virtual void OnCharacterStartStrikeAnim()
        {

        }

        public virtual void OnBossDeath()
        {

        }

        public virtual void OnDummyDeath()
        {

        }

        public virtual void OnTargetTutorialDestroyed()
        {

        }

        public virtual void OnLevelEnd()
        {

        }

        public virtual void OnPause()
        {

        }

        public virtual void OnResume()
        {

        }

        public virtual void OnBallCriticalShot()
        {

        }

        public virtual void OnSpeedPortalCrossed()
        {

        }

        #endregion

    }
}
