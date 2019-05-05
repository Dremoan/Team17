﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaBoss : Entity
    {
        [Header("Components")]
        [SerializeField] private Boss assignedBossScript;
        [SerializeField] private Transform jumpTarget;
        [SerializeField] private Transform jumpSummit;
        [Header("Jump parameters")]
        [SerializeField] private int jumpSteps = 7;
        [SerializeField] private float distToSwitchStep = 0.2f;
        [SerializeField] private float maxSummitHeight = 5f;
        [SerializeField] private float lowJumpThreshHold = 3f;
        [SerializeField] private float smallJumpThreshhold = 5f;
        [SerializeField] private Transform[] possibleJumpTargets;
        [Header("Speed parameters")]
        [SerializeField] private float jumpSpeed = 2f;
        [Tooltip("0 = small, 1 = long, 2 = low, 3 = high")]
        [SerializeField] private AnimationCurve[] speedCurves;
        [Header("FXs")]
        [SerializeField] private FeedBack landingFB;

        private bool isJumping = false;
        private int pathStepTarget = 0;
        private Vector3[] path;
        private Vector3 posRef;
        private Vector3 jumpStart;
        private float jumpInc = 0f;
        private float jumpParcouredDist = 0f;
        private float jumpCalculatedDist = 0f;
        private AnimationCurve usedSpeedCurve;

        protected override void Update()
        {
            base.Update();
            JumpManagement();
        }

        [ContextMenu ("Launch jump")]
        public void LaunchJump(GameObject target)
        {
            jumpTarget.position = target.transform.position;

            usedSpeedCurve = speedCurves[0];

            Vector3 vectorToTarget = (jumpTarget.position - transform.position);
            Vector3 middlePoint = transform.position + (vectorToTarget * 0.5f);

            jumpSummit.position = new Vector3(middlePoint.x, jumpSummit.position.y, 0);

            if (Mathf.Abs(transform.position.x - jumpTarget.position.x) > smallJumpThreshhold) // jump is long
            {
                usedSpeedCurve = speedCurves[1];
            }
            else // jump is low
            {
                usedSpeedCurve = speedCurves[0];
            }

            if (Mathf.Abs(transform.position.y - jumpTarget.position.y) < lowJumpThreshHold) // jump is low
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, jumpTarget.position.y + 8f, 0f);
                usedSpeedCurve = speedCurves[2];
            }
            else // jump is high
            {
                if (transform.position.y > jumpTarget.position.y)
                {
                    jumpSummit.position = new Vector3(jumpSummit.position.x, transform.position.y * 2, 0);
                }
                else
                {
                    jumpSummit.position = new Vector3(jumpSummit.position.x, jumpTarget.position.y * 2, 0);
                }
                usedSpeedCurve = speedCurves[3];
            }

            if (jumpSummit.position.y > assignedBossScript.RoomZero.position.y + maxSummitHeight * 2)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, assignedBossScript.RoomZero.position.y + maxSummitHeight * 2, 0);
            }
            if(jumpSummit.position.y < 0)
            {
                 jumpSummit.position = new Vector3(jumpSummit.position.x, 0, 0);
            }


            path = new Vector3[jumpSteps];
            for (int i = 0; i < path.Length; i++)
            {
                float t = Mathf.InverseLerp(0, jumpSteps - 1, i);
                path[i] = Vector3.Lerp(Vector3.Lerp(transform.position, jumpSummit.position, t), Vector3.Lerp(jumpSummit.position, jumpTarget.position, t), t);
            }
            for (int i = 0; i < path.Length; i++)
            {
                if (i < path.Length - 1)
                {
                    jumpCalculatedDist += Vector3.Distance(path[i], path[i + 1]);
                }
            }
            jumpParcouredDist = 0f;
            pathStepTarget = 0;
            jumpStart = transform.position;
            jumpInc = 0;
            isJumping = true;
        }

        private void JumpManagement()
        {
            if (isJumping)
            {
                //debug
                for (int i = 0; i < path.Length; i++)
                {
                    if (i < path.Length - 1)
                    {
                        Debug.DrawLine(path[i], path[i + 1], Color.red);
                    }
                }/**/
                Debug.DrawLine(path[0], path[path.Length - 1], Color.gray);
                Vector3 lastPos = transform.position;
                transform.position = Vector3.Lerp(jumpStart, path[pathStepTarget], jumpInc);
                jumpParcouredDist += Vector3.Distance(lastPos, transform.position);
                float parcouredDistRatio = Mathf.InverseLerp(0, jumpCalculatedDist, jumpParcouredDist);
                jumpInc += Time.deltaTime * usedSpeedCurve.Evaluate(parcouredDistRatio) * jumpSpeed;
                if (Vector3.Distance(transform.position, path[pathStepTarget]) < distToSwitchStep)
                {
                    jumpInc = 0;
                    jumpStart = transform.position;
                    if (pathStepTarget == path.Length - 1)
                    {
                        transform.position = path[path.Length - 1];
                        isJumping = false;
                        jumpCalculatedDist = 0;
                        jumpParcouredDist = 0;
                        landingFB.Play();
                    }
                    else
                    {
                        pathStepTarget++;
                        if (pathStepTarget > path.Length - 1)
                        {
                            pathStepTarget = path.Length - 1;
                        }
                    }
                }
            }

        }
    }
}