using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace Team17.StreetHunt
{
    public class TutorialPlayer : Entity
    {
        private int caseIndex = 0;
        private int valueCount;
        [SerializeField] private PlayableDirector[] timelinesTransitions;
        [SerializeField] private int valueRequired;
        [SerializeField] private GameObject[] touchPlanes;
        [SerializeField] private TutorialCasesEvents[] endCaseEvents;

        public override void OnBallShot()
        {
            base.OnBallShot();
            if(valueCount < valueRequired && caseIndex == 0)
            {
                valueCount++;
            }
            else
            {
                StartCoroutine(DelayEndCase());
            }
        }

        public override void OnBallCriticalShot()
        {
            base.OnBallCriticalShot();
            if(valueCount < valueRequired && caseIndex == 1)
            {
                valueCount++;
            }
            else
            {
                StartCoroutine(DelayEndCase());
            }
        }

        public override void OnDummyDeath()
        {
            base.OnDummyDeath();
            if(caseIndex == 3)
            {
                endCaseEvents[caseIndex].eventEndCase.Invoke();
            }

            if (valueCount < valueRequired && caseIndex == 2)
            {
                valueCount++;
            }

            else
            {
                endCaseEvents[caseIndex].eventEndCase.Invoke();
                caseIndex++;
                valueCount = 0;
            }

        }

        public void SelectValues(int valueModified)
        {
            valueRequired = valueModified;
        }


        IEnumerator DelayEndCase()
        {
            touchPlanes[caseIndex].SetActive(false);
            yield return new WaitForSeconds(1f);
            endCaseEvents[caseIndex].eventEndCase.Invoke();
            caseIndex++;
            valueCount = 0;
        }
    }

    [System.Serializable]
    public class TutorialCasesEvents
    {
        [SerializeField] private string nameCases;
        public UnityEvent eventEndCase;
    }

}
