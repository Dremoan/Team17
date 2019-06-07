using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class TargetManager : Entity
    {
        [SerializeField] private TargetTutorial[] targets;
        private int targetIndex;

        public void NextTarget()
        {
            StartCoroutine(AppearNextTarget());
        }

        public override void OnTargetTutorialDestroyed()
        {
            base.OnTargetTutorialDestroyed();
            targetIndex++;
            NextTarget();
        }

        IEnumerator AppearNextTarget()
        {
            if (targetIndex < targets.Length)
            {
                yield return new WaitForSeconds(1f);
                targets[targetIndex].TriggerNextAnim();
            }
        }
    }
}
