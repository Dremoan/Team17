using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedPortalTutorialManager : Entity
    {
        [SerializeField] private SpeedPortal[] speedPortals;
        private int actualIndex;

        public void ActivePortal()
        {
            speedPortals[actualIndex].gameObject.SetActive(true);
            StartCoroutine(PortalActivation());
        }

        public override void OnSpeedPortalCrossed()
        {
            base.OnSpeedPortalCrossed();
            speedPortals[actualIndex].gameObject.SetActive(false);
            actualIndex++;
        }


        IEnumerator PortalActivation()
        {
            yield return new WaitForSeconds(2f);
            speedPortals[actualIndex].TutorialActivation();
        }
    }
}
