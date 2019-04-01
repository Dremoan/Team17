using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class TouchPlane : Entity, Touchable
    {
        [SerializeField] private PlayerProjectile ball;
        [SerializeField] private LivesManager lives;


        public void OnTouchBegin(Vector3 touchPos)
        {
            if(ball != null)
            {
                ball.StartCalculation();
            }
            else
            {
                ball = lives.GetNextBall();
            }
        }

        public void OnTouchHeld(Vector3 touchPos)
        {
            if (ball != null)
            {
                ball.FeedBack(touchPos);
            }
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            if (ball != null)
            {
                ball.Launch((touchPos - ball.transform.position));
            }   
        }

        public PlayerProjectile Ball { get => ball; set => ball = value; }
    }
}
