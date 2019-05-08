using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class TouchPlane : Entity, Touchable
    {
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private LivesManager lives;

        private PlayerProjectile ball;
        private bool shouldLaunchIntro = true;
        private bool touchBegan = false;

        public void OnTouchBegin(Vector3 touchPos)
        {
            if(shouldLaunchIntro)
            {
                GameManager.state.CallOnBossEnters();
                shouldLaunchIntro = false;
                return;
            }

            if(ball != null && !ball.Destroyed && ball.gameObject.activeSelf && ball.CanStrike)
            {
                ball.StartCalculation();
                touchBegan = true;
            }
            else if(lives.BallAvailable())
            {
                ball = lives.GetNextBall();
                if(ball != null && ball.CanStrike)
                {
                    character.CurrentBall = lives.GetNextBall();
                    ball.transform.position = character.transform.position;
                    ball.gameObject.SetActive(true);
                    ball.StartCalculation();
                    touchBegan = true;
                }
            }
            else
            {
                return;
            }
        }

        public void OnTouchHeld(Vector3 touchPos)
        {
            if(ball != null)
            {
                if(ball.CanStrike && touchBegan)
                {
                    ball.FeedBack(touchPos);
                }
            }
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            if(ball != null)
            {
                if(ball.CanStrike && touchBegan)
                {
                    ball.GetNewDirection((touchPos - ball.transform.position));
                }
            }
            touchBegan = false;
        }

        public PlayerProjectile Ball { get => ball; set => ball = value; }
    }
}
