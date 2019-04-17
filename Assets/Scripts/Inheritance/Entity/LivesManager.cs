using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class LivesManager : Entity
    {
        [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private PlayerProjectile[] projectiles;

        private bool bossDead = false;

        protected override void Start()
        {
            base.Start();
            GameManager.state.LivesLeft = projectiles.Length;
            GameManager.state.PlayerGameObject = projectiles[0].gameObject;
        }

        public bool BallAvailable()
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                if(!projectiles[i].Destroyed)
                {
                    return true;
                }
            }
            return false;
        }

        public PlayerProjectile GetNextBall()
        {
            return projectiles[GameManager.state.LivesLeft - 1];
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
