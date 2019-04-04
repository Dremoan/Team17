using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class BodyPartAnimBender : Entity
    {
        [SerializeField] private Animator bendAnimator;
        [SerializeField] [Range(0, 1)] private float leftBend = 0.5f;
        [SerializeField] [Range(0, 1)] private float rightBend = 0.5f;

        protected override void Update()
        {
            base.Update();
            bendAnimator.transform.localPosition = Vector3.zero;
            bendAnimator.SetFloat("LeftBend", leftBend);
            bendAnimator.SetFloat("RightBend", rightBend);
        }
    }
}
