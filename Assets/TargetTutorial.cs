using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class TargetTutorial : Entity
    {
        [SerializeField] private Animator targetAnim;

        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.GetComponent<PlayerProjectile>() != null)
            {
                this.GetComponent<BoxCollider>().enabled = false;
                GameManager.state.CallOnTargetTutorialDestroyed();
                TriggerNextAnim();
            }
        }

        public void TriggerNextAnim()
        {
            targetAnim.SetTrigger("Next");
        }
    }
}
