using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class BodyPartAnimBender : Entity
    {
        [SerializeField] private Animator bendAnimator;
        [SerializeField] private Transform leftTip;
        [SerializeField] private Transform rightTip;
        [SerializeField] [Range(0, 1)] private float leftBend = 0.5f;
        [SerializeField] [Range(0, 1)] private float rightBend = 0.5f;

        private float initialDistFromRight = 1f;
        private float initialDistFromLeft = 1f;

        protected override void Start()
        {
            base.Start();
            initialDistFromRight = Vector3.Distance(transform.position, rightTip.position);
            initialDistFromLeft = Vector3.Distance(transform.position, leftTip.position);
        }

        protected override void Update()
        {
            base.Update();
            bendAnimator.transform.localPosition = Vector3.zero;
            bendAnimator.SetFloat("RightBend", leftBend);
            bendAnimator.SetFloat("LeftBend", rightBend);
            CalculateRightBend();
        }

        private void CalculateRightBend()
        {
            Vector3 centerTangent = transform.right * initialDistFromRight;
            float bendDist = Vector3.Distance(centerTangent, rightTip.position);
            float upDist = Vector3.Distance(transform.up, transform.up + transform.up * bendDist);
            Debug.Log("Dist : " + bendDist + " Updist : " + upDist + " Tip is up : " + (upDist < bendDist));
        }

        public float LeftBend { get => leftBend; set => leftBend = value; }
        public float RightBend { get => rightBend; set => rightBend = value; }
    }
}
