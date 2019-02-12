using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class TouchPlane : Entity, Touchable
    {
        [SerializeField] private PlayerProjectile ball;

        public void OnTouchBegin(Vector3 touchPos)
        {
            ball.StartCalculation();
        }

        public void OnTouchHeld(Vector3 touchPos)
        {
            ball.FeedBack(touchPos);
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            ball.Launch((touchPos - ball.transform.position));
        }
    }
}
