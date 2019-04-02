using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class TouchPlane : Entity, Touchable
    {
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private LivesManager lives;

        private PlayerProjectile ball;
        private bool shouldLaunchIntro = true;


        public void OnTouchBegin(Vector3 touchPos)
        {
            if(shouldLaunchIntro)
            {
                GameManager.state.CallOnIntroLaunched();
                shouldLaunchIntro = false;
                return;
            }

            if(ball != null && !ball.Destroyed)
            {
                ball.StartCalculation();
            }
            else if(lives.BallAvailable())
            {
                ball = lives.GetNextBall();
                ball.transform.position = character.transform.position;
                ball.StartCalculation();
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
                ball.FeedBack(touchPos);
            }
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            if(ball != null)
            {
                ball.Launch((touchPos - ball.transform.position));
            }
        }

        public PlayerProjectile Ball { get => ball; set => ball = value; }
    }
}
