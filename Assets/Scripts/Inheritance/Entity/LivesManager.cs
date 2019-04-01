using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class LivesManager : Entity
    {
        [SerializeField] private PlayerProjectile firstBall;
        [SerializeField] private PlayerProjectile secondBall;
        [SerializeField] private PlayerProjectile thirdBall;
        [SerializeField] private PlayerProjectile fourthBall;
        private int livesLeft = 4;

        protected override void Start()
        {
            base.Start();

        }

        public bool BallAvailable()
        {
            if(firstBall.Destroyed && secondBall.Destroyed && thirdBall.Destroyed && fourthBall.Destroyed)
            {
                return false;
            }
            return true;
        }

        public PlayerProjectile GetNextBall()
        {
            if (livesLeft == 4) return firstBall;
            if (livesLeft == 3) return secondBall;
            if (livesLeft == 2) return thirdBall;
            if (livesLeft == 1) return fourthBall;
            return null;
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            livesLeft--;
            if (livesLeft == 4) firstBall.gameObject.SetActive(true);
            if (livesLeft == 3) secondBall.gameObject.SetActive(true);
            if (livesLeft == 2) thirdBall.gameObject.SetActive(true);
            if (livesLeft == 1) fourthBall.gameObject.SetActive(true);

            if (livesLeft == 0)
            {

            }// game over;
        }
    }
}
