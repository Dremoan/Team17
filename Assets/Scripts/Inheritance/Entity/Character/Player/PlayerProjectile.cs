using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class PlayerProjectile : Character
    {
        [Header("Components")]
        public Rigidbody body;
        [SerializeField] private TimersCalculator timer;

        [Header("Feedbacks")]
        [SerializeField] private Transform timerFeedback;
        [SerializeField] private Transform trajectory;
        [SerializeField] private PlayerCharacter character;
        [Tooltip ("The power threshold of the group must be sorted from the smallest to highest.")]
        [SerializeField] private PowerGroups[] powerGroups;

        [Header("Parameters")]
        [SerializeField] private float speed = 50f;
        [SerializeField] private float slowedTimeScale = 0.2f;
        [SerializeField] private AnimationCurve timeToHit;
        [SerializeField] private AnimationCurve powerGained;
        [SerializeField] private AnimationCurve feedBackRadius;
        [SerializeField] private float powerLostOnBounce = 5f;
        [SerializeField] private float speedPortalPrecision = 2f;
        [SerializeField] private float stunTime = 1.5f;

        [Header("Trajectory calculation")]
        [SerializeField] private LayerMask trajectoryCalculationMask;


        private float power = 0;
        private int reHitTimer;
        private int usedPowergroupIndex = 0;
        private bool canStrike = true;
        private bool destroyed = false;
        private bool wasCanceled = false;
        private bool isStriking = false;
        private Vector3 movementDirection;
        private Vector3 initialFeedbackScale;
        private Vector3 lastEnter;
        private Vector3 lastNormal;
        private Vector3 lastContact;
        private Vector3 lastNewDir;
        private PowerGroups usedPowerGroup;

        #region Monobehaviour callbacks

        protected override void Start()
        {
            base.Start();
            initialFeedbackScale = timerFeedback.localScale;
            usedPowerGroup = powerGroups[0];
        }

        protected override void Update()
        {
            base.Update();
            Debug.DrawRay(lastContact, lastNormal.normalized * 3, Color.magenta);
            Debug.DrawRay(lastContact, -lastEnter.normalized * 3, Color.blue);
            Debug.DrawRay(lastContact, lastNewDir.normalized * 3, Color.red);
            FuturPositionInArena();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SelectPowerGroup(power);
            initialFeedbackScale = timerFeedback.localScale;
            GameManager.state.BallGameObject = this.gameObject;
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
                //Hit();
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
                PassThroughSpeedPortal(body.velocity.normalized, coll.gameObject.transform.right);
                coll.gameObject.SetActive(false);
            }

            if (coll.gameObject.GetComponent<IBallHitable>() != null)
            {
                coll.gameObject.GetComponent<IBallHitable>().Hit(usedPowergroupIndex, power);
                Hit();
            }
        }

        #endregion

        #region Launching methods

        public void StartCalculation()
        {
            if(canStrike)
            {
                body.velocity *= slowedTimeScale;
                reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(power), StunCharacter);
                timerFeedback.gameObject.SetActive(true);
                trajectory.gameObject.SetActive(true);
                character.Physicate(false);
                wasCanceled = false;
                isStriking = true;

                GameManager.state.CallOnPlayerTeleport();
            }
        }

        public void FeedBack(Vector3 touchPos)
        {
            if(canStrike)
            {
                Timer t = timer.GetTimerFromUserIndex(reHitTimer);
                timerFeedback.localScale = (feedBackRadius.Evaluate(Mathf.InverseLerp(0, t.MaxTime, t.TimeLeft)) * initialFeedbackScale) + Vector3.one;
                trajectory.position = Vector3.Lerp(transform.position, touchPos, 0.5f);
                float zRot = Vector3.SignedAngle(transform.up, (touchPos - transform.position), Vector3.forward);
                trajectory.rotation = Quaternion.Euler(0, 0, zRot);
                trajectory.localScale = new Vector3(1, Vector3.Distance(transform.position, touchPos) * 2, 1);
                character.PrepareStrike(transform.position, touchPos);
            }
        }

        public void GetNewDirection(Vector3 newDirection)
        {
            if(canStrike)
            {
                if (wasCanceled)
                {
                    wasCanceled = false;
                    return;
                }
                body.useGravity = false;
                Timer t = timer.GetTimerFromUserIndex(reHitTimer);

                power += powerGained.Evaluate(t.Inc);
                SelectPowerGroup(power);
                movementDirection = newDirection.normalized * (usedPowerGroup.Speed);
                usedPowerGroup.Hit.Rotate3DStartRotation(GetRotationFromDirection(newDirection));
                usedPowerGroup.Launch.Rotate3DStartRotation(GetRotationFromDirection(newDirection));
                usedPowerGroup.Trail.RotateShapeEmitter(GetRotationFromDirection(newDirection));
                timer.DeleteTimer(reHitTimer);
                timerFeedback.gameObject.SetActive(false);
                trajectory.gameObject.SetActive(false);

                character.Physicate(true);
                character.Strike();

                GameManager.state.CallOnCharacterStartStrikeAnim();
            }
        }

        public void LaunchBall()
        {

            body.velocity = movementDirection;
            usedPowerGroup.Launch.Play();
            usedPowerGroup.Trail.Play();
            isStriking = false;

            GameManager.state.CallOnBallShot();
            
        }

        private void SelectPowerGroup(float actualPower)
        {
            for (int i = 0; i < powerGroups.Length - 1; i++)
            {
                if (power > powerGroups[i].PowerThreshold)
                {
                    usedPowerGroup.Trail.Stop();
                    usedPowerGroup = powerGroups[i];
                    usedPowergroupIndex = i;
                    Debug.Log("P: " + power + ": " + usedPowerGroup.Name);
                }
            }
        }

        public void PauseBehavior()
        {
            body.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }

        #endregion

        #region Ball interactions

        private void Hit()
        {
            power = 0;
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            gameObject.SetActive(false);
            destroyed = true;

            usedPowerGroup.Hit.Play();
            usedPowerGroup.Trail.Stop();

            GameManager.state.CallOnBallHit(usedPowergroupIndex, power);
        }

        private void CancelBall()
        {
            power = 0;
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            gameObject.SetActive(false);
            destroyed = true;

            usedPowerGroup.Destroyed.Play();
            usedPowerGroup.Trail.Stop();

            GameManager.state.CallOnBallDestroyed();
        }

        private void StunCharacter()
        {
            isStriking = false;
            body.velocity = movementDirection;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            canStrike = false;
            timer.LaunchNewTimer(stunTime, RecoverCharacter);
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
            if(isStriking) movementDirection = newDir.normalized * (usedPowerGroup.Speed) * slowedTimeScale;
            else movementDirection = newDir.normalized * (usedPowerGroup.Speed);

            body.velocity = movementDirection;

            usedPowerGroup.Trail.RotateShapeEmitter(GetRotationFromDirection(newDir));
            usedPowerGroup.Bounce.Play();
            usedPowerGroup.Hit.Rotate3DStartRotation(GetRotationFromDirection(newDir));
            usedPowerGroup.Launch.Rotate3DStartRotation(GetRotationFromDirection(newDir));
            usedPowerGroup.Hit.Play();
            usedPowerGroup.Trail.Play();

            GameManager.state.CallOnBallBounced();
        }


        public float GetRotationFromDirection(Vector3 lookDirection)
        {
            float rotZ = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;
            return 90 - rotZ;
        }


        private void PassThroughSpeedPortal(Vector3 entryVelocity, Vector3 portalRight)
        {
            float sqrMag = Vector3.SqrMagnitude(entryVelocity - portalRight);

            if(sqrMag < speedPortalPrecision)
            {
                //Speed up
                if (usedPowergroupIndex > powerGroups.Length - 2) return;
                usedPowergroupIndex++;
            }
            else
            {
                //Speed down
                if (usedPowergroupIndex < 1) return;
                usedPowergroupIndex--;
            }
            usedPowerGroup = powerGroups[usedPowergroupIndex];
            power = usedPowerGroup.PowerThreshold;
            movementDirection = body.velocity.normalized * (usedPowerGroup.Speed);
            body.velocity = movementDirection;
            Debug.Log("P: " + power + ": " + usedPowerGroup.Name);
        }

        #endregion

        #region Trajectory management

        public Vector3 FuturPositionInArena()
        {
            float dist = body.velocity.magnitude;
            float remainingDist = dist;
            Vector3 rayStart = transform.position;
            Vector3 futurMovementDir = body.velocity.normalized;
            Vector3 futurPos = Vector3.zero;

            while(remainingDist > 0)
            {
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(rayStart, futurMovementDir, out hit, remainingDist, trajectoryCalculationMask);
                Debug.DrawRay(rayStart, futurMovementDir.normalized * remainingDist, Color.green);
                if(hit.collider != null)
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
            Debug.DrawLine(transform.position, futurPos, Color.yellow);
            return futurPos;
        }

        #endregion

        [ContextMenu ("Setup score manager")]
        public void SetupScoreManager()
        {
            ScoreManager manager = GameObject.Find("GameManager").GetComponent<ScoreManager>();
            manager.ScoreHits = new ScoreHit[powerGroups.Length];
            for (int i = 0; i < manager.ScoreHits.Length; i++)
            {
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

        public string Name { get => name;}
        public float PowerThreshold { get => powerThreshold; }
        public float Speed { get => speed; }
        public FeedBack Launch { get => launch; }
        public FeedBack Bounce { get => bounce; }
        public FeedBack Trail { get => trail; }
        public FeedBack Destroyed { get => destroyed; }
        public FeedBack Hit { get => hit; }
    }
}
