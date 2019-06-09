using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedBallTutorialManager : Entity
    {
        [SerializeField] private SpeedPortal[] speedPortals;
        public int actualIndex;

        public void ActivePortal(float timerActivation)
        {
            speedPortals[actualIndex].gameObject.SetActive(true);
            StartCoroutine(PortalActivation(timerActivation));
        }

        public override void OnSpeedPortalCrossed()
        {
            base.OnSpeedPortalCrossed();
            actualIndex++;
            ActivePortal(1.5f);
            speedPortals[actualIndex].gameObject.SetActive(false);
        }

        IEnumerator PortalActivation(float timerActivation)
        {
            yield return new WaitForSeconds(timerActivation);
            speedPortals[actualIndex].TutorialActivation();
        }
    }
}
