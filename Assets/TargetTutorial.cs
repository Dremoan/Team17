using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class TargetTutorial : MonoBehaviour
    {
        [SerializeField] private TutorialManager tutorialManager;
        [SerializeField] private Animator targetAnim;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerProjectile>() != null)
            {
                tutorialManager.ValueCount++;
                targetAnim.Play("TutorialDisappearTarget");
            }
        }

        public void TutorialAppearTarget()
        {
            targetAnim.Play("TutorialAppearTarget");
        }
    }
}
