using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class Entity : MonoBehaviour
    { 
        protected virtual void OnEnable()
        {
            GameManager.state.RegisterEntity(this);
        }

        protected void OnDisable()
        {
            GameManager.state.UnregisterEntity(this);
        }

        public virtual void OnPlayerBeginTeleport()
        {

        }
    }
}
