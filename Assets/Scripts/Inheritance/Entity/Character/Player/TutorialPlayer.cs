using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.StreetHunt
{
    public class TutorialPlayer : Entity
    {
        private int caseIndex = 0;
        private int valueCount;
        [SerializeField] private PlayableDirector[] timelinesTransitions;
        [SerializeField] private int valueRequired;

        public override void OnBallShot()
        {
            base.OnBallShot();
            if(valueCount < valueRequired && caseIndex == 0)
            {
                valueCount++;
            }
            else
            {
                timelinesTransitions[caseIndex].Play();
                caseIndex++;
                valueCount = 0;
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
                timelinesTransitions[caseIndex].Play();
                caseIndex++;
                valueCount = 0;
            }
        }


        public void SelectValues(int valueShoot)
        {
            valueRequired = valueShoot;
        }

    }

}
