using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class PlayerProjectile : Character
    {
        [Header("Components")]
        [SerializeField] private Rigidbody body;
        [SerializeField] private TimersCalculator timer;

        [Header("Feedbacks")]
        [SerializeField] private FeedBack accuracyFeedback;
        [SerializeField] private FeedBack powergroupIncreasedFeedback;
        [SerializeField] private Transform timerFeedback;
        [SerializeField] private float initialFeedbackScale = 5f;
        [SerializeField] private Transform trajectory;
        [SerializeField] private SpriteRenderer aimArrow;
        [SerializeField] private PlayerCharacter character;
        [SerializeField] private FeedBack criticalShotFeedBack;
        [SerializeField] private FeedBack criticalSignFeedback;
        [Tooltip("The power threshold of the group must be sorted from the smallest to highest.")]
        [SerializeField] private PowerGroups[] powerGroups;

        [Header("Parameters")]
        [SerializeField] private float speed = 50f;
        [SerializeField] private float slowedTimeScale = 0.2f;
        [SerializeField] private AnimationCurve timeToHit;
        [SerializeField] private AnimationCurve powerGained;
        [SerializeField] private AnimationCurve feedBackRadius;
        [SerializeField] private float maxPowerMargin = 50f;
        [SerializeField] private float powerLostOnBounce = 5f;
        [SerializeField] private float speedPortalPrecision = 2f;
        [SerializeField] private float stunTime = 1.5f;

        [Header("Trajectory calculation")]
        [SerializeField] private LayerMask trajectoryCalculationMask;

        [SerializeField] private float power = 0;

        private int reHitTimer;
        private int usedPowergroupIndex = 0;
        private bool canStrike = true;
        private bool destroyed = false;
        private bool wasCanceled = false;
        private bool isStriking = false;
        private bool shouldTriggerCriticalSign = true;
        private Vector3 movementDirection;
        private Vector3 lastEnter;
        private Vector3 lastNormal;
        private Vector3 lastContact;
        private Vector3 lastNewDir;
        private PowerGroups usedPowerGroup;

        #region Monobehaviour callbacks

        protected override void Start()
        {
            base.Start();
            character.CurrentBall = this.GetComponent<PlayerProjectile>();
            Debug.Log("initial : " + initialFeedbackScale);
            usedPowerGroup = powerGroups[0];
        }

        protected override void Update()
        {
            base.Update();
            Debug.DrawRay(lastContact, lastNormal.normalized * 3, Color.magenta);
            Debug.DrawRay(lastContact, -lastEnter.normalized * 3, Color.blue);
            Debug.DrawRay(lastContact, lastNewDir.normalized * 3, Color.red);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            power = 5;
            SelectPowerGroup(power);
            GameManager.state.BallGameObject = this.gameObject;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.state.BallGameObject = character.gameObject;
        }

        private void OnCollisionEnter(Collision coll)
        {
            if (coll.gameObject.GetComponent<BallCanceler>() != null)
            {
                CancelBall();
            }

            if (coll.gameObject.GetComponent<IBallHitable>() != null)
            {
                lastNormal = coll.contacts[0].normal;
                lastContact = coll.contacts[0].point;
                lastEnter = movementDirection;
                Bounce(movementDirection, coll.contacts[0].normal);
                coll.gameObject.GetComponent<IBallHitable>().Hit(usedPowergroupIndex, power);
                Hit();
            }
            else
            {
                lastNormal = coll.contacts[0].normal;
                lastContact = coll.contacts[0].point;
                lastEnter = movementDirection;
                Bounce(movementDirection, coll.contacts[0].normal);
            }
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.GetComponent<BallCanceler>() != null)
            {
                CancelBall();
            }

            if (coll.gameObject.GetComponent<SpeedPortal>() != null)
            {
                PassThroughSpeedPortal(coll.gameObject.GetComponent<SpeedPortal>(), body.velocity.normalized, coll.gameObject.transform.right);
            }

            if (coll.gameObject.GetComponent<IBallHitable>() != null)
            {
                coll.gameObject.GetComponent<IBallHitable>().Hit(usedPowergroupIndex, power);
                Hit();
            }

            if (coll.gameObject.GetComponent<BallRelfecter>() != null)
            {
                Vector3 newDir = (transform.position - coll.transform.position).normalized;
                SetMovementDir(newDir);
                usedPowerGroup.Trail.RotateFeedback(GetRotationFromDirection(newDir));

                StunCharacter(coll.gameObject.GetComponent<BallRelfecter>().StunTime);
            }
        }

        #endregion

        #region Launching methods

        public void StartCalculation()
        {
            if (canStrike)
            {
                isStriking = true;
                SetMovementDir(movementDirection);
                reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(power), StunCharacter);
                accuracyFeedback.Play();
                timerFeedback.gameObject.SetActive(true);
                trajectory.gameObject.SetActive(true);
                character.Physicate(false);
                character.AimingParameterSetup(false);
                wasCanceled = false;
                shouldTriggerCriticalSign = true;

                GameManager.state.CallOnPlayerTeleport();
            }
        }

        public void FeedBack(Vector3 touchPos)
        {
            if (canStrike)
            {
                isStriking = true;
                SetMovementDir(movementDirection);
                Timer t = timer.GetTimerFromUserIndex(reHitTimer);
                timerFeedback.localScale = (feedBackRadius.Evaluate(Mathf.InverseLerp(0, t.MaxTime, t.TimeLeft)) * initialFeedbackScale * Vector3.one) + Vector3.one;
                trajectory.position = Vector3.Lerp(transform.position, touchPos, 0.5f);

                if(t.TimeLeft < 0.7f && shouldTriggerCriticalSign)
                {
                    shouldTriggerCriticalSign = false;
                    criticalSignFeedback.Play();
                }

                float zRot = Vector3.SignedAngle(transform.up, (touchPos - transform.position), Vector3.forward);
                trajectory.rotation = Quaternion.Euler(0, 0, zRot + 90);
                //trajectory.localScale = new Vector3(0.5f, Vector3.Distance(transform.position, touchPos) * 1f, 0.5f);
                aimArrow.size = new Vector2(Vector3.Distance(transform.position, touchPos) * 1f, aimArrow.size.y);
                character.PrepareStrike(transform.position, touchPos);
            }
        }

        public void GetNewDirection(Vector3 newDirection)
        {
            if (canStrike)
            {
                if (wasCanceled)
                {
                    wasCanceled = false;
                    return;
                }
                body.useGravity = false;
                Timer t = timer.GetTimerFromUserIndex(reHitTimer);

                power += powerGained.Evaluate(t.Inc);

                if (powerGained.Evaluate(t.Inc) > 8)
                {
                    character.CriticalShoot = true;
                }
                else
                {
                    character.CriticalShoot = false;
                }


                if (power > powerGroups[powerGroups.Length - 1].PowerThreshold + maxPowerMargin)
                {
                    power = powerGroups[powerGroups.Length - 1].PowerThreshold + maxPowerMargin;
                }

                SelectPowerGroup(power);
                movementDirection = newDirection.normalized * (usedPowerGroup.Speed);

                usedPowerGroup.Trail.RotateFeedback(GetRotationFromDirection(movementDirection));
                accuracyFeedback.Stop(ParticleSystemStopBehavior.StopEmittingAndClear);
                criticalSignFeedback.Stop(ParticleSystemStopBehavior.StopEmittingAndClear);

                timer.DeleteTimer(reHitTimer);
                timerFeedback.gameObject.SetActive(false);
                trajectory.gameObject.SetActive(false);

                character.AimingParameterSetup(true);
                character.Strike();

                GameManager.state.CallOnCharacterStartStrikeAnim();
            }
        }

        public void LaunchBall()
        {
            usedPowerGroup.Launch.RotateFeedback(GetRotationFromDirection(movementDirection));
            usedPowerGroup.Launch.Play();
            usedPowerGroup.Trail.Play();

            isStriking = false;
            SetMovementDir(movementDirection);

            GameManager.state.CallOnBallShot();

        }

        /// <summary>
        /// Change the usedPowerGroup and the usedPowerGroupIndex depending on actualPower.
        /// </summary>
        /// <param name="actualPower"></param>
        private void SelectPowerGroup(float actualPower)
        {
            int lastIndex = usedPowergroupIndex;

            for (int i = 0; i < powerGroups.Length - 1; i++)
            {
                if (power > powerGroups[i].PowerThreshold)
                { 
                    if(usedPowerGroup.Trail != null) usedPowerGroup.Trail.Stop(ParticleSystemStopBehavior.StopEmittingAndClear);
                    usedPowerGroup = powerGroups[i];
                    usedPowergroupIndex = i;
                }
            }

            if (lastIndex != usedPowergroupIndex)
            {
                if (lastIndex < usedPowergroupIndex) //increase
                {
                    powergroupIncreasedFeedback.RotateFeedback(GetRotationFromDirection(movementDirection));
                    powergroupIncreasedFeedback.Play();
                    GameManager.state.CallOnBallIncreasePowerGroup();
                }
                else // decrease
                {
                    GameManager.state.CallOnBallDecreasePowerGroup();
                }
            }

        }

        public void PauseBehavior()
        {
            SetMovementDir(Vector3.zero);
            usedPowerGroup.Trail.Stop();
            gameObject.SetActive(false);
        }

        #endregion

        #region Ball interactions

        private void Hit()
        {
            power = 5;
            SelectPowerGroup(power);
            SetMovementDir(movementDirection);
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);

            usedPowerGroup.Hit.Play();
            //usedPowerGroup.Trail.Stop();

            GameManager.state.CallOnBallHit(usedPowergroupIndex, power);
        }

        public void CriticalShot()
        {
            criticalShotFeedBack.Play();
            GameManager.state.CallOnBallCriticalShot();
        }

        private void CancelBall()
        {
            power = 0;
            SelectPowerGroup(power);
            SetMovementDir(Vector3.zero);
            canStrike = true;
            wasCanceled = true;
            isStriking = false;
            timer.DeleteTimer(reHitTimer);
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            gameObject.SetActive(false);
            destroyed = true;
            timerFeedback.localScale = initialFeedbackScale * Vector3.one;

            usedPowerGroup.Destroyed.Play();
            usedPowerGroup.Trail.Stop(ParticleSystemStopBehavior.StopEmittingAndClear);

            GameManager.state.CallOnBallDestroyed();
        }

        private void StunCharacter()
        {
            character.AimingParameterSetup(true);
            character.Exhausted();
            accuracyFeedback.Stop(ParticleSystemStopBehavior.StopEmittingAndClear);
            criticalSignFeedback.Stop(ParticleSystemStopBehavior.StopEmittingAndClear);
            isStriking = false;
            SetMovementDir(movementDirection);
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            canStrike = false;
            timer.LaunchNewTimer(stunTime, RecoverCharacter);
        }

        public void StunCharacter(float time)
        {
            isStriking = false;
            SetMovementDir(movementDirection);
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            canStrike = false;
            timer.LaunchNewTimer(time, RecoverCharacter);
        }

        private void RecoverCharacter()
        {
            canStrike = true;
        }

        private void Bounce(Vector3 enterVector, Vector3 collisionNormal)
        {
            if (body.useGravity) return;
            Vector3 newDir = Vector3.Reflect(enterVector, collisionNormal);
            lastNewDir = newDir;

            power -= powerLostOnBounce;
            SelectPowerGroup(power);
            if (power < 0) power = 0;
            SetMovementDir(newDir);

            usedPowerGroup.Bounce.RotateFeedback(-GetRotationFromDirection(collisionNormal));
            usedPowerGroup.Trail.RotateFeedback(GetRotationFromDirection(movementDirection));
            usedPowerGroup.Bounce.Play();
            usedPowerGroup.Trail.Play();

            GameManager.state.CallOnBallBounced();
        }

        public float GetRotationFromDirection(Vector3 lookDirection)
        {
            float rotZ = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
            return 90 - rotZ;
        }

        private void PassThroughSpeedPortal(SpeedPortal portal, Vector3 entryVelocity, Vector3 portalRight)
        {
            entryVelocity = new Vector3(Mathf.Abs(entryVelocity.x), Mathf.Abs(entryVelocity.y), entryVelocity.z);
            portalRight = new Vector3(Mathf.Abs(portalRight.x), Mathf.Abs(portalRight.y), portalRight.z);
            float sqrMag = Vector3.SqrMagnitude(entryVelocity - portalRight);

            //Speed up
            if (usedPowergroupIndex < powerGroups.Length - 1)
            {
                power = powerGroups[usedPowergroupIndex + 1].PowerThreshold + 10;
            }
            else
            {
                power = powerGroups[powerGroups.Length - 1].PowerThreshold + maxPowerMargin;
            }
            portal.SpeedPortalDesactivation();
            SelectPowerGroup(power);
            SetMovementDir(body.velocity);
            usedPowerGroup.Trail.RotateFeedback(GetRotationFromDirection(movementDirection));
            usedPowerGroup.Trail.Play();
            GameManager.state.CallOnSpeedPortalCrossed();
        }

        #endregion

        #region Trajectory management

        private void SetMovementDir(Vector3 dir)
        {
            movementDirection = dir.normalized * usedPowerGroup.Speed;
            if (isStriking)
            {
                body.velocity = movementDirection * slowedTimeScale;
            }
            else
            {
                body.velocity = movementDirection;
            }
        }

        public Vector3 FuturPositionInArena()
        {
            float dist = body.velocity.magnitude;
            float remainingDist = dist;
            Vector3 rayStart = transform.position;
            Vector3 futurMovementDir = body.velocity.normalized;
            Vector3 futurPos = Vector3.zero;

            while (remainingDist > 0)
            {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(rayStart, futurMovementDir, out hit, remainingDist, trajectoryCalculationMask);
                //Debug.DrawRay(rayStart, futurMovementDir.normalized * remainingDist, Color.green);
                if (hit.collider != null)
                {
                    rayStart = hit.point;
                    futurMovementDir = Vector3.Reflect(futurMovementDir, hit.normal);
                    remainingDist -= hit.distance;
                    futurPos = rayStart + (futurMovementDir * remainingDist);
                }
                else
                {
                    futurPos = rayStart + (futurMovementDir * remainingDist);
                    break;
                }
            }
            //Debug.DrawLine(transform.position, futurPos, Color.yellow);
            return futurPos;
        }

        #endregion

        #region Tutorial Functions


        public void AddPower(float powerToAdd)
        {
            power += powerToAdd;
            SelectPowerGroup(power);
        }

        public override void OnBallIncreasePowerGroup()
        {
            base.OnBallIncreasePowerGroup();
            usedPowerGroup.Trail.Play();
        }

        #endregion

        [ContextMenu("Setup score manager")]
        public void SetupScoreManager()
        {
            ScoreManager manager = GameObject.Find("UiManager").GetComponent<ScoreManager>();
            manager.ScoreHits = new ScoreHit[powerGroups.Length];
            for (int i = 0; i < manager.ScoreHits.Length; i++)
            {
                manager.ScoreHits[i] = new ScoreHit();
                manager.ScoreHits[i].Name = powerGroups[i].Name;
            }
        }

        public bool Destroyed { get => destroyed; set => destroyed = value; }
        public bool CanStrike { get => canStrike; set => canStrike = value; }
    }

    public interface IBallHitable
    {
        void Hit(int index, float dmgs);
    }

    [System.Serializable]
    public struct PowerGroups
    {
        [SerializeField] private string name;
        [Tooltip("Define the power threshold at which this group will be used. If the power of the ball is higher than this threshold, it will be used.")]
        [SerializeField] private float powerThreshold;
        [SerializeField] private float speed;
        [SerializeField] private FeedBack launch;
        [SerializeField] private FeedBack bounce;
        [SerializeField] private FeedBack trail;
        [SerializeField] private FeedBack destroyed;
        [SerializeField] private FeedBack hit;
        [SerializeField] private FeedBack stunned;

        public string Name { get => name; }
        public float PowerThreshold { get => powerThreshold; }
        public float Speed { get => speed; }
        public FeedBack Launch { get => launch; }
        public FeedBack Bounce { get => bounce; }
        public FeedBack Trail { get => trail; }
        public FeedBack Destroyed { get => destroyed; }
        public FeedBack Hit { get => hit; }
    }
}
