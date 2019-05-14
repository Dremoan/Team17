using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class LivesManager : Entity
    {
        [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private PlayerProjectile playerProjectile;
        [SerializeField] private int lives = 4;

        private bool bossDead = false;

        protected override void Start()
        {
            base.Start();
            GameManager.state.LivesLeft = lives;
            GameManager.state.BallGameObject = playerProjectile.gameObject;
        }

        public bool BallAvailable()
        {
            /*for (int i = 0; i < projectiles.Length; i++)
            {
                if(!projectiles[i].Destroyed)
                {
                    return true;
                }
            }
            return false;/**/

            if(GameManager.state.LivesLeft > 0)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public PlayerProjectile GetNextBall()
        {
            //return projectiles[GameManager.state.LivesLeft - 1];
            playerProjectile.Destroyed = false;
            playerProjectile.CanStrike = true;
            return playerProjectile;
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            bossDead = true;
        }

        public void TeleportBallsTo(Transform targetPlace)
        { 
            playerCharacter.transform.position = targetPlace.position;
        }
    }
}
