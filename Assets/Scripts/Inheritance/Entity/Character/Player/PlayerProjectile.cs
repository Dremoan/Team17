using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class PlayerProjectile : Character
    {
        [Header("Components")]
        [SerializeField] private Rigidbody body;
        [SerializeField] private TimersCalculator timer;

        [Header("Feedbacks")]
        [SerializeField] private Transform timerFeedback;
        [SerializeField] private Transform trajectory;
        [SerializeField] private PlayerCharacter character;

        [Header("Parameters")]
        [SerializeField] private float speed = 50f;
        [SerializeField] private float slowedTimeScale = 0.2f;
        [SerializeField] private AnimationCurve speedMultiplier;
        [SerializeField] private AnimationCurve timeToHit;
        [SerializeField] private AnimationCurve powerGained;
        [SerializeField] private float powerLostOnBounce = 5f;

        [Header("Trajectory calculation")]
        [SerializeField] private LayerMask trajectoryCalculationMask;


        private float power = 0;
        private int reHitTimer;
        private bool destroyed = false;
        private bool wasCanceled = false;
        private Vector3 movementDirection;
        private Vector3 initialFeedbackScale;
        private Vector3 lastEnter;
        private Vector3 lastNormal;
        private Vector3 lastContact;
        private Vector3 lastNewDir;

        protected override void Start()
        {
            base.Start();
            initialFeedbackScale = timerFeedback.localScale;
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
            GameManager.state.PlayerGameObject = this.gameObject;
        }

        public void StartCalculation()
        {
            body.velocity *= slowedTimeScale;
            reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(power), CancelBall);
            timerFeedback.gameObject.SetActive(true);
            trajectory.gameObject.SetActive(true);
            character.Physicate(false);
            wasCanceled = false;
            GameManager.state.CallOnPlayerTeleport();
        }

        public void FeedBack(Vector3 touchPos)
        {
            Timer t = timer.GetTimerFromUserIndex(reHitTimer);
            timerFeedback.localScale = (Mathf.InverseLerp(0, t.MaxTime, t.TimeLeft) * initialFeedbackScale) + Vector3.one;
            trajectory.position = Vector3.Lerp(transform.position, touchPos, 0.5f);
            float zRot = Vector3.SignedAngle(transform.up, (touchPos - transform.position), Vector3.forward);
            trajectory.rotation = Quaternion.Euler(0, 0, zRot);
            trajectory.localScale = new Vector3(1, Vector3.Distance(transform.position, touchPos) * 2, 1);
            character.PrepareStrike(transform.position, touchPos);
        }

        public void Launch(Vector3 newDirection)
        {
            if(wasCanceled)
            {
                wasCanceled = false;
                return;
            }
            body.useGravity = false;
            Timer t = timer.GetTimerFromUserIndex(reHitTimer);
            power += powerGained.Evaluate(t.Inc);
            movementDirection = newDirection.normalized * (speed * speedMultiplier.Evaluate(power));
            Debug.Log("Gained : " + powerGained.Evaluate(t.Inc) + " Power : " + power + " Speed : " + speed * speedMultiplier.Evaluate(power));
            body.velocity = movementDirection;
            timer.DeleteTimer(reHitTimer);
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            character.Strike();
            GameManager.state.CallOnBallShot();
        }

        #region Ball interactions

        private void CancelBall()
        {
            power = 0;
            body.velocity = body.velocity * 0.7f;
            body.useGravity = true;
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            gameObject.SetActive(false);
            destroyed = true;
            GameManager.state.CallOnBallDestroyed();
        }

        private void Bounce(Vector3 enterVector, Vector3 collisionNormal)
        {
            if (body.useGravity) return;
            Vector3 newDir = Vector3.Reflect(enterVector, collisionNormal);
            lastNewDir = newDir;
            power -= powerLostOnBounce;
            if (power < 0) power = 0;
            Debug.Log("Power after bounce : " + power);
            movementDirection = newDir.normalized * (speed * speedMultiplier.Evaluate(power));
            body.velocity = movementDirection;
            GameManager.state.CallOnBallBounced();
        }

        #endregion

        private void OnCollisionEnter(Collision coll)
        {
            if(coll.gameObject.GetComponent<BallCanceler>() != null)
            {
                CancelBall();
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
            if (coll.gameObject.GetComponent<IBallHitable>() != null)
            {
                coll.gameObject.GetComponent<IBallHitable>().Hit(power);
            }
        }

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

        public bool Destroyed { get => destroyed; set => destroyed = value; }

    }

    public interface IBallHitable
    {
        void Hit(float dmgs);
    }
}
