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
            CalculateRightBend();
        }

        private void CalculateRightBend()
        {
            Vector3 centerTangent = transform.right * initialDistFromRight;
            Debug.DrawLine(transform.position, transform.position + centerTangent, Color.blue);

            float bendDist = Vector3.Distance(transform.position + centerTangent, rightTip.position);
            float upDist = Vector3.Distance(transform.up, rightTip.position);
            float downDist = Vector3.Distance(-transform.up, rightTip.position);

            Debug.Log("Dist : " + bendDist + " Updist : " + upDist + " Tip is up : " + (upDist < downDist));

            float clampedBendDist = Mathf.Clamp(bendDist, 0, 1);
            if (upDist > downDist) clampedBendDist *= -1;

            bendAnimator.SetFloat("LeftBend", Mathf.InverseLerp(-1, 1, clampedBendDist));
        }

        public float LeftBend { get => leftBend; set => leftBend = value; }
        public float RightBend { get => rightBend; set => rightBend = value; }
    }
}
