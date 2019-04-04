﻿using System.Collections;
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
            if (livesLeft == 4)
            {
                firstBall.gameObject.SetActive(true);
                playerCharacter.actualBall = firstBall.GetComponent<PlayerProjectile>();
                return firstBall;
            }
            if (livesLeft == 3)
            {
                secondBall.gameObject.SetActive(true);
                playerCharacter.actualBall = secondBall.GetComponent<PlayerProjectile>();
                return secondBall;
            }
            if (livesLeft == 2)
            {
                thirdBall.gameObject.SetActive(true);
                playerCharacter.actualBall = thirdBall.GetComponent<PlayerProjectile>();
                return thirdBall;
            }
            if (livesLeft == 1)
            {
                fourthBall.gameObject.SetActive(true);
                playerCharacter.actualBall = fourthBall.GetComponent<PlayerProjectile>();
                return fourthBall;
            }
            return null;
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            livesLeft--;
            GameManager.state.LivesLeft = livesLeft;
            if (livesLeft == 0)
            {

            }// game over;
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            bossDead = true;
        }

        public override void OnBallHit(float hitPower)
        {
            base.OnBallHit(hitPower);
            livesLeft--;
            GameManager.state.LivesLeft = livesLeft;
            if (livesLeft == 0 && !bossDead)
            {

            }// game over;
        }
    }
}
