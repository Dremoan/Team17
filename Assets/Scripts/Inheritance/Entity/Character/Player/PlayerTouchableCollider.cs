using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class PlayerTouchableCollider : Entity, Touchable
    {
        [SerializeField] private Player player;
        [SerializeField] private float slowMoRate = 0.2f;

        #region Touchable

        public void OnTouchBegin(Vector3 touchPos)
        {
            player.SetBeginSwipe();
        }

        public void OnTouchHeld(Vector3 touchPos)
        {
            player.ShowTrajectory(touchPos);
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            player.SetEndSwipe(touchPos);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerProjectile>() != null)
            {
                player.CanLaunch = true;
                Time.timeScale = slowMoRate;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                Debug.Log("Detected ball");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerProjectile>() != null)
            {
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                Debug.Log("Ball is away");
            }
        }

        #endregion
    }
}
