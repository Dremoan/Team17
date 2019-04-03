using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.BallDash
{
public class CustomAnimEvents : MonoBehaviour
{
    public AnimEvent[] animEventsArray;

    public void InvokeAnimEvent(int indexEvent)
    {
        animEventsArray[indexEvent].eventTrigger.Invoke();
    }
}


[System.Serializable]
public class AnimEvent
{
    public string EventName;
    public UnityEvent eventTrigger;
}
}
