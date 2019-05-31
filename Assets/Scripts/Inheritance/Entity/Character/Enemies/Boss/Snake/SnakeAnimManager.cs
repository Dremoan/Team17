using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SnakeAnimManager : Entity
    {
        [SerializeField] private Animator snakeAnim;
        [SerializeField] private Animator animatorCanvasAnim;

        private void OnTriggerStay(Collider other)
        {
            if(other.GetComponent<Wall>() != null)
            {
                snakeAnim.SetBool("Close", true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Wall>() != null)
            {
                snakeAnim.SetBool("Close", false);
            }
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            animatorCanvasAnim.Play("FlashRed");
        }
    }
}
