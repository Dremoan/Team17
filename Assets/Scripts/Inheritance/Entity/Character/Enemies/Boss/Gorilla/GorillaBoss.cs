using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaBoss : Entity
    {
        [Header("Components")]
        [SerializeField] private Boss assignedBossScript;
        [SerializeField] private Animator anim;
        [Header("Jump parameters")]
        [SerializeField] private Transform jumpTarget;
        [SerializeField] private Transform jumpSummit;
        [SerializeField] private float jumpSpeed = 2f;
        [Tooltip("0 = small, 1 = long, 2 = low, 3 = high")]
        [SerializeField] private AnimationCurve[] speedCurves;
        [SerializeField] private GorillaJumpTarget[] rightTargets;
        [SerializeField] private GorillaJumpTarget[] leftTargets;
        [SerializeField] private int jumpSteps = 7;
        [SerializeField] private float distToSwitchStep = 0.2f;
        [SerializeField] private float maxSummitHeight = 5f;
        [SerializeField] private float lowJumpThreshHold = 3f;
        [SerializeField] private float smallJumpThreshhold = 5f;
        [Header("Shout parameters")]
        [SerializeField] private Transform headTransform;
        [SerializeField] private GorillaShoutBehaviour shoutGO;
        [SerializeField] private int shoutingLoops = 5;
        [SerializeField] private int shoutingRecoverLoops = 5;
        [Header("Rocks parameters")]
        [SerializeField] private Transform rightHand;
        [SerializeField] private Transform leftHand;
        [SerializeField] private RockProjectile[] rocksPool;
        [Header("Spikes parameters")]
        [SerializeField] private bool canSpikesOnGround = true;
        [SerializeField] private Animator leftSpikes;
        [SerializeField] private Animator rightSpikes;
        [Header("FXs")]
        [SerializeField] private FeedBack landingFB;

        private AnimationCurve usedSpeedCurve;
        private RockProjectile launchedRock;
        private GorillaJumpTarget currentJumpTarget;
        private Vector3[] path;
        private Vector3 posRef;
        private Vector3 jumpStart;
        private bool isShouting = false;
        private bool isJumping = false;
        private bool jumpingRight = false;
        private int pathStepTarget = 0;
        private int shoutLoopsTodo = 5;
        private float jumpInc = 0f;
        private float jumpParcouredDist = 0f;
        private float jumpCalculatedDist = 0f;
        private float currentIdleType;

        #region Monobehaviour

        protected override void Update()
        {
            base.Update();
            JumpManagement();
        }

        #endregion

        #region Jump calculation

        public void JumpToRightSide()
        {
            GorillaJumpTarget chosenTarget;
            do
            {
                int c = Random.Range(0, rightTargets.Length);
                chosenTarget = rightTargets[c];
            } while (chosenTarget == currentJumpTarget);
            LaunchJump(chosenTarget);
        }

        public void JumpToLeftSide()
        {
            GorillaJumpTarget chosenTarget;
            do
            {
                int c = Random.Range(0, leftTargets.Length);
                chosenTarget = leftTargets[c];
            } while (chosenTarget == currentJumpTarget);
            LaunchJump(chosenTarget);
        }

        public void JumpToRandom()
        {
            GorillaJumpTarget chosenTarget;

            do
            {
                int l = Random.Range(0, 2);
                if (l == 0)
                {
                    int c = Random.Range(0, leftTargets.Length);
                    chosenTarget = leftTargets[c];
                }
                else
                {
                    int c = Random.Range(0, rightTargets.Length);
                    chosenTarget = rightTargets[c];
                }
            } while (chosenTarget == currentJumpTarget);

            LaunchJump(chosenTarget);
        }

        public void LaunchJump(GorillaJumpTarget target)
        {
            /*if(currentJumpTarget == target)
            {
                assignedBossScript.SkipCurrentAttack();
                return;
            }*/
            currentJumpTarget = target;
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
            anim.SetBool("jumping", isJumping);
            anim.SetBool("toTheRight", (transform.position.x < jumpTarget.position.x));
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
                        anim.SetBool("jumping", isJumping);
                        jumpCalculatedDist = 0;
                        jumpParcouredDist = 0;
                        landingFB.Play();
                        SetIdleType(currentJumpTarget.GorillaIdleValue);
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

        #endregion

        #region Attacks

        public void MediumAttack()
        {
            if(currentIdleType == 0.5f || currentIdleType == 0.75f) // rock launch (walled)
            {
                //GetNewRock();
            }
            else
            {
                if(currentIdleType == 0f) // is left
                {
                    anim.SetBool("shoutingRight", true);
                }
                if(currentIdleType == 0.25f) // is right
                {
                    anim.SetBool("shoutingLeft", true);
                }
                anim.SetBool("shoutingLoop", true);
                shoutLoopsTodo = shoutingLoops;
                shoutGO.gameObject.SetActive(true);
                //shoutGO.transform.position = new Vector3(headTransform.position.x, headTransform.position.y, 0);
                shoutGO.FollowedTransfom = headTransform;
            }
        }

        public void HardAttack()
        {
            if (currentIdleType == 0.5f || currentIdleType == 0.75f) // Spikes (walled)
            {
                anim.SetTrigger("spikes");
            }
            else
            {
                if (canSpikesOnGround)
                {
                    anim.SetTrigger("spikes");
                }
                else
                {
                    MediumAttack();
                }
            }
        }

        #region Rock launch

        private void GetNewRock()
        {
            for (int i = 0; i < rocksPool.Length; i++)
            {
                if (rocksPool[i].Available)
                {
                    launchedRock = rocksPool[i];
                    launchedRock.gameObject.SetActive(true);
                    return;
                }
            }

            if (currentIdleType == 0.5f)
            {
                launchedRock.HeldBy(rightHand);
                anim.SetBool("rightArmRockLaunch", true);
            }
            if (currentIdleType == 0.75f)
            {
                launchedRock.HeldBy(leftHand);
                anim.SetBool("leftArmRockLaunch", true);
            }
        }

        public void LaunchCurrentRock()
        {
            launchedRock.transform.position = new Vector3(launchedRock.transform.position.x, launchedRock.transform.position.y, 0);
            Vector3 dir = (GameManager.state.BallGameObject.transform.position - launchedRock.transform.position);
            //Vector3 dir = (GameManager.state.BallGameObject.GetComponent<PlayerProjectile>().FuturPositionInArena() - launchedRock.transform.position);
            launchedRock.Launch(dir);
        }

        public void EndRockLaunch()
        {
            anim.SetBool("rightArmRockLaunch", false);
            anim.SetBool("leftArmRockLaunch", false);
        }

        #endregion

        #region Shout

        public void EndOneShoutLoop()
        {
            shoutLoopsTodo--;
            if(shoutLoopsTodo == 0)
            {
                shoutLoopsTodo = shoutingRecoverLoops;
                anim.SetBool("shoutingLoop", false);
                anim.SetBool("shoutingRecoverLoop", true);
            }
        }

        public void EndOneShoutRecoverLoop()
        {
            shoutLoopsTodo--;
            if(shoutLoopsTodo == 0)
            {
                anim.SetBool("shoutingRecoverLoop", false);
            }
        }

        public void EndShoutAttack()
        {
            anim.SetBool("shoutingRight", false);
            anim.SetBool("shoutingLeft", false);
        }


        #endregion

        #region Spikes

        public void ActivateSpikes()
        {
            if (currentIdleType == 0.5f)
            {
                rightSpikes.SetTrigger("spikes");
            }
            if (currentIdleType == 0.75f)
            {
                leftSpikes.SetTrigger("spikes");
            }
            if (currentIdleType == 0f)
            {
                rightSpikes.SetTrigger("spikes");
            }
            if (currentIdleType == 0.25f)
            {
                leftSpikes.SetTrigger("spikes");
            }
        }

        #endregion

        #endregion

        #region Animations 

        public void SetIdleType(float type)
        {
            currentIdleType = type;
            anim.SetFloat("idle", currentIdleType);
        }

        #endregion
    }
}
