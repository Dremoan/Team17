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

        protected void OnDisable()
        {
            GameManager.state.UnregisterEntity(this);
        }
    
        #endregion

        #region Entity callbacks

        public virtual void OnPlayerTeleport()
        {

        }

        public virtual void OnBallShooted()
        {

        }

        public virtual void OnBallCanceled()
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

        #endregion

    }
}
