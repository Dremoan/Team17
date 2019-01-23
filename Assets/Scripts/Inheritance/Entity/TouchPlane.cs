using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class TouchPlane : Entity, Touchable
    {
        [SerializeField] private Player player;

        public void OnTouchBegin(Vector3 touchPos)
        {
            player.Teleport(touchPos);
        }

        public void OnTouchHeld(Vector3 touchPos)
        {

        }

        public void OnTouchReleased(Vector3 touchPos)
        {

        }
    }
}
