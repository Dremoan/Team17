using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
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

        public virtual void OnIntroLaunched()
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

        public virtual void OnBallDestroyed()
        {

        }

        public virtual void OnBallHit(float hitPower)
        {

        }

        public virtual void OnBallSlowed()
        {

        }

        public virtual void OnBallBounced()
        {

        }

        public virtual void OnBossHurt()
        {

        }

        public virtual void OnBossChangeState()
        {

        }

        public virtual void OnBossDeath()
        {

        }

        public virtual void OnPause()
        {

        }

        public virtual void OnResume()
        {

        }

        #endregion

    }
}
