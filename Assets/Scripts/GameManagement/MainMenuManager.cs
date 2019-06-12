using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class MainMenuManager : Entity
    {
        [SerializeField] private Animator compositionAnimator;
        [SerializeField] private Animator canvasUiAnim;

        public void PickRandomCompositionAnim(int MaxRange)
        {
            compositionAnimator.SetInteger("IndexToPick", Random.Range(0, MaxRange));
        }

        public void OpenLevelMenu(float timeToOpen)
        {
            
        }

        IEnumerator OpenPlayMenu(float openingDelay)
        {
            yield return new WaitForSeconds(openingDelay);
            canvasUiAnim.Play("FadeOut");

        }



    }
}
