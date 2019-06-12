using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.StreetHunt
{
    public class MainMenuManager : Entity
    {
        [SerializeField] private Animator compositionAnimator;
        [SerializeField] private TransitionEvents[] transitionEvents;
        [SerializeField] private float timeTransition;
        [SerializeField] private string nameLevel;

        protected override void Start()
        {
            base.Start();
            SetLevelName("");
        }

        public void PickRandomCompositionAnim(int MaxRange)
        {
            compositionAnimator.SetInteger("IndexToPick", Random.Range(0, MaxRange));
        }

        public void SetLevelName(string newNameLevel)
        {
            nameLevel = newNameLevel;
        }

        public void CallOpenLevelMenu(float timeToOpen)
        {
            StartCoroutine(OpenLevelMenu(timeTransition));
        }

        public void CallCloseLevelMenu(float timeToClose)
        {
            StartCoroutine(CloseLevelMenu(timeTransition));
        }

        public void LaunchSelectedLevel(string levelName)
        {
            transitionEvents[2].EventTransition[0].Invoke();
        }

        IEnumerator OpenLevelMenu(float openingDelay)
        {
            yield return new WaitForSeconds(openingDelay);
            transitionEvents[0].EventTransition[0].Invoke();
            yield return null;
            transitionEvents[0].EventTransition[1].Invoke();
        }

        IEnumerator CloseLevelMenu(float closingDelay)
        {
            yield return new WaitForSeconds(closingDelay);
            transitionEvents[1].EventTransition[0].Invoke();
            yield return null;
            transitionEvents[1].EventTransition[1].Invoke();
        }
    }

    [System.Serializable]
    public class TransitionEvents
    {
        [SerializeField] private string nameTransition;
        [SerializeField] private UnityEvent[] eventTransition;

        public UnityEvent[] EventTransition { get => eventTransition; set => eventTransition = value; }
    }
}
