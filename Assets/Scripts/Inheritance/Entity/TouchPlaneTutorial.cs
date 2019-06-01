using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.StreetHunt
{
    public class TouchPlaneTutorial : Entity, Touchable
    {
        [SerializeField] private UnityEvent eventTouch;

        public void OnTouchBegin(Vector3 touchPos)
        {
            
        }

        public void OnTouchHeld(Vector3 touchPos)
        {
            
        }

        public void OnTouchReleased(Vector3 touchPos)
        {
            eventTouch.Invoke();
        }
    }
}
