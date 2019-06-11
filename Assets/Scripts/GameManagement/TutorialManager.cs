using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace Team17.StreetHunt
{
    public class TutorialManager : Entity
    {
        private int caseIndex = 0;
        public int valueCount;
        [SerializeField] private int valueRequired;
        [SerializeField] private GameObject[] touchPlanes;
        [SerializeField] private TutorialCasesEvents[] endCaseEvents;
        private bool case1Valid;
        private bool case2Valid;
        private bool case3Valid;
        private bool case4Valid;
        private bool case5Valid;

        public int ValueCount { get => valueCount; set => valueCount = value; }

        public override void OnTargetTutorialDestroyed()
        {
            base.OnTargetTutorialDestroyed();
            if (valueCount < valueRequired && caseIndex == 0 && !case1Valid)
            {
                valueCount++;
            }
            if (!case1Valid && valueCount == valueRequired && caseIndex == 0)
            {
                case1Valid = true;
                StartCoroutine(DelayEndCase(endCaseEvents[caseIndex].endingCount));
            }

        }

        public override void OnBallCriticalShot()
        {
            base.OnBallCriticalShot();
            if (valueCount < valueRequired && caseIndex == 1 && !case2Valid)
            {
                valueCount++;
            }

            if (!case2Valid && valueCount == valueRequired && caseIndex == 1)
            {
                case2Valid = true;
                StartCoroutine(DelayEndCase(endCaseEvents[caseIndex].endingCount));
            }

        }

        public override void OnDummyDeath()
        {
            base.OnDummyDeath();

            #region Case3

            if (valueCount < valueRequired && caseIndex == 2 && !case3Valid)
            {
                valueCount++;
            }

            if (valueCount == valueRequired && !case3Valid && caseIndex == 2)
            {
                case3Valid = true;
                StartCoroutine(DelayEndCase(endCaseEvents[caseIndex].endingCount));
            }

            #endregion

            #region Case4

            if (valueCount < valueRequired && caseIndex == 3 && !case4Valid)
            {
                valueCount++;
            }


            if (valueCount == valueRequired && !case4Valid && caseIndex == 3)
            {
                case4Valid = true;
                StartCoroutine(DelayEndCase(endCaseEvents[caseIndex].endingCount));
            }

            #endregion

            #region Case5

            if (valueCount < valueRequired && caseIndex == 4 && !case5Valid)
            {
                valueCount++;
            }

            if (valueCount == valueRequired && !case5Valid && caseIndex == 4)
            {
                case5Valid = true;
                StartCoroutine(DelayEndCase(endCaseEvents[caseIndex].endingCount));
            }

            #endregion


        }

        public void SelectValues(int valueModified)
        {
            valueRequired = valueModified;
        }


        IEnumerator DelayEndCase(float delayEndCase)
        {
            touchPlanes[caseIndex].SetActive(false);
            yield return new WaitForSeconds(delayEndCase);
            endCaseEvents[caseIndex].eventEndCase.Invoke();
            caseIndex++;
            valueCount = 0;
        }
    }

    [System.Serializable]
    public class TutorialCasesEvents
    {
        [SerializeField] private string nameCases;
        public float endingCount;
        public UnityEvent eventEndCase;
    }

}
