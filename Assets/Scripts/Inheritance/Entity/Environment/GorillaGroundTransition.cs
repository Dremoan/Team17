using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaGroundTransition : Environment
    {
        [SerializeField] private GameObject baseGO;
        [SerializeField] private GameObject transitionGO;
        [SerializeField] private FeedBack transitionFeedback;

        [ContextMenu("Test transition")]
        public void TriggerTransition()
        {
            baseGO.SetActive(false);
            transitionGO.SetActive(true);
            transitionFeedback.Play();
        }
    }
}
