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
        [SerializeField] private int valueRequired;
        [SerializeField] private GameObject[] touchPlanes;
        [SerializeField] private TutorialCasesEvents[] endCaseEvents;
        private bool case1Valid;
        private bool case2Valid;
        private bool case3Valid;
        private bool case4Valid;
        private bool case5Valid;

        public override void OnBallShot()
        {
            base.OnBallShot();
            if (valueCount < valueRequired && caseIndex == 0 && !case1Valid)
            {
                valueCount++;
            }
            else
            {
                if (!case1Valid)
                {
                    case1Valid = false;
                    StartCoroutine(DelayEndCase());
                }
            }
        }

        public override void OnBallCriticalShot()
        {
            base.OnBallCriticalShot();
            if (valueCount < valueRequired && caseIndex == 1 && !case2Valid)
            {
                valueCount++;
            }
            else
            {
                case2Valid = false;
                StartCoroutine(DelayEndCase());
            }
        }

        public override void OnDummyDeath()
        {
            base.OnDummyDeath();
            if (caseIndex == 3 && !case3Valid)
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
