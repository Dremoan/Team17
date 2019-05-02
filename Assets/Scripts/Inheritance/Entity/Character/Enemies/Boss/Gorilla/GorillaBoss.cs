using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaBoss : Entity
    {
        [SerializeField] private Boss assignedBossScript;
        [SerializeField] private Transform jumpTarget;
        [SerializeField] private Transform jumpSummit;
        [SerializeField] private int jumpSteps = 7;
        [SerializeField] private float jumpSpeed = 2f;
        [SerializeField] private float distToSwitchStep = 0.2f;
        [SerializeField] private AnimationCurve[] speedCurves;
        [SerializeField] private float maxSummitHeight = 5f;
        private bool isJumping = false;
        private Vector3[] path;
        private int pathStepTarget = 0;
        private Vector3 posRef;
        private float jumpInc = 0f;
        private Vector3 jumpStart;
        private AnimationCurve usedSpeedCurve;
        private float jumpParcouredDist = 0f;
        private float jumpCalculatedDist = 0f;

        protected override void Update()
        {
            base.Update();
            JumpManagement();
        }

        public void SetTargetPos(Vector3 pos)
        {
            jumpTarget.position = assignedBossScript.RoomZero.position + pos;
        }

        [ContextMenu ("Launch jump")]
        public void LaunchJump()
        {
            usedSpeedCurve = speedCurves[0];
            // calculate summit pos
            Vector3 vectorToTarget = (jumpTarget.position - transform.position);
            Vector3 middlePoint = transform.position + (vectorToTarget * 0.5f);
            Vector3 normal = (Vector3.Cross(vectorToTarget, Vector3.forward).normalized) * vectorToTarget.magnitude * 0.5f;
         
            if(transform.position.x < jumpTarget.position.x) // gorilla pos is to the left
            {
                jumpSummit.position = middlePoint - normal;
                if (jumpSummit.position.x < transform.position.x)
                {
                    jumpSummit.position = new Vector3(transform.position.x, jumpSummit.position.y, 0);
                }
                if(jumpSummit.position.x > jumpTarget.position.x)
                {
                    jumpSummit.position = new Vector3(jumpTarget.position.x, jumpSummit.position.y, 0);
                }
            }
            else // gorilla to the right
            {
                jumpSummit.position = middlePoint + normal;
                if (jumpSummit.position.x < jumpTarget.position.x)
                {
                    jumpSummit.position = new Vector3(jumpTarget.position.x, jumpSummit.position.y, 0);
                }
                if (jumpSummit.position.x > transform.position.x)
                {
                    jumpSummit.position = new Vector3(transform.position.x, jumpSummit.position.y, 0);
                }
            }

            if(jumpSummit.position.y > assignedBossScript.RoomZero.position.y + maxSummitHeight)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, assignedBossScript.RoomZero.position.y + maxSummitHeight, 0);
            }
            if(jumpSummit.position.y < 0)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, 0, 0);
            }
            //
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

        public void LaunchJump(int curveIndex)
        {
            usedSpeedCurve = speedCurves[curveIndex];
            // calculate summit pos
            Vector3 vectorToTarget = (jumpTarget.position - transform.position);
            Vector3 middlePoint = transform.position + (vectorToTarget * 0.5f);
            Vector3 normal = (Vector3.Cross(vectorToTarget, Vector3.forward).normalized) * vectorToTarget.magnitude * 0.5f;

            if (transform.position.x < jumpTarget.position.x) // gorilla pos is to the left
            {
                jumpSummit.position = middlePoint - normal;
                if (jumpSummit.position.x < transform.position.x)
                {
                    jumpSummit.position = new Vector3(transform.position.x, jumpSummit.position.y, 0);
                }
                if (jumpSummit.position.x > jumpTarget.position.x)
                {
                    jumpSummit.position = new Vector3(jumpTarget.position.x, jumpSummit.position.y, 0);
                }
            }
            else // gorilla to the right
            {
                jumpSummit.position = middlePoint + normal;
                if (jumpSummit.position.x < jumpTarget.position.x)
                {
                    jumpSummit.position = new Vector3(jumpTarget.position.x, jumpSummit.position.y, 0);
                }
                if (jumpSummit.position.x > transform.position.x)
                {
                    jumpSummit.position = new Vector3(transform.position.x, jumpSummit.position.y, 0);
                }
            }

            if (jumpSummit.position.y > assignedBossScript.RoomZero.position.y + maxSummitHeight)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, assignedBossScript.RoomZero.position.y + maxSummitHeight, 0);
            }
            if (jumpSummit.position.y < 0)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, 0, 0);
            }
            //
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

        public void LaunchJump(int curveIndex, float summitHeight)
        {
            usedSpeedCurve = speedCurves[curveIndex];
            // calculate summit pos
            Vector3 vectorToTarget = (jumpTarget.position - transform.position);
            Vector3 middlePoint = transform.position + (vectorToTarget * 0.5f);
            Vector3 normal = (Vector3.Cross(vectorToTarget, Vector3.forward).normalized) * summitHeight;

            if (transform.position.x < jumpTarget.position.x) // gorilla pos is to the left
            {
                jumpSummit.position = middlePoint - normal;
                if (jumpSummit.position.x < transform.position.x)
                {
                    jumpSummit.position = new Vector3(transform.position.x, jumpSummit.position.y, 0);
                }
                if (jumpSummit.position.x > jumpTarget.position.x)
                {
                    jumpSummit.position = new Vector3(jumpTarget.position.x, jumpSummit.position.y, 0);
                }
            }
            else // gorilla to the right
            {
                jumpSummit.position = middlePoint + normal;
                if (jumpSummit.position.x < jumpTarget.position.x)
                {
                    jumpSummit.position = new Vector3(jumpTarget.position.x, jumpSummit.position.y, 0);
                }
                if (jumpSummit.position.x > transform.position.x)
                {
                    jumpSummit.position = new Vector3(transform.position.x, jumpSummit.position.y, 0);
                }
            }

            if (jumpSummit.position.y > assignedBossScript.RoomZero.position.y + maxSummitHeight)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, assignedBossScript.RoomZero.position.y + maxSummitHeight, 0);
            }
            if (jumpSummit.position.y < 0)
            {
                jumpSummit.position = new Vector3(jumpSummit.position.x, 0, 0);
            }
            //
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
                /**///debug
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
