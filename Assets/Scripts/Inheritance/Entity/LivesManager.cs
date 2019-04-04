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
        [SerializeField] private PlayerCharacter playerCharacter;
        private int livesLeft = 4;
        private bool bossDead = false;

        protected override void Start()
        {
            base.Start();
            GameManager.state.LivesLeft = livesLeft;
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
            if (GameManager.state.LivesLeft == 4)
            {
                firstBall.gameObject.SetActive(true);
                playerCharacter.actualBall = firstBall.GetComponent<PlayerProjectile>();
                return firstBall;
            }
            if (GameManager.state.LivesLeft == 3)
            {
                secondBall.gameObject.SetActive(true);
                playerCharacter.actualBall = secondBall.GetComponent<PlayerProjectile>();
                return secondBall;
            }
            if (GameManager.state.LivesLeft == 2)
            {
                thirdBall.gameObject.SetActive(true);
                playerCharacter.actualBall = thirdBall.GetComponent<PlayerProjectile>();
                return thirdBall;
            }
            if (GameManager.state.LivesLeft == 1)
            {
                fourthBall.gameObject.SetActive(true);
                playerCharacter.actualBall = fourthBall.GetComponent<PlayerProjectile>();
                return fourthBall;
            }
            return null;
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            bossDead = true;
        }

    }
}
