using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class PlayerProjectile : Character
    {
        enum TypeOfTimer {Cancel, Nothing}
        [Header("Components")]
        [SerializeField] private Rigidbody body;
        [SerializeField] private TimersCalculator timer;
        [Header("Feedbacks")]
        [SerializeField] private Transform timerFeedback;
        [SerializeField] private Transform trajectory;
        [SerializeField] private PlayerCharacter character;

        [Header("Parameters")]
        [SerializeField] private TypeOfTimer type; 
        [SerializeField] private float speed = 50f;
        [SerializeField] private float slowedTimeScale = 0.2f;
        [SerializeField] private AnimationCurve speedMultiplier;
        [SerializeField] private AnimationCurve timeToHit;
        private float power = 0;
        private int reHitTimer;
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
        }

        public void StartCalculation()
        {
            body.velocity *= slowedTimeScale;
            reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(power), CancelBall);
            timerFeedback.gameObject.SetActive(true);
            trajectory.gameObject.SetActive(true);
            character.Physicate(false);
            wasCanceled = false;
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
            power += (t.Inc) * 1.7f;
            movementDirection = newDirection.normalized * (speed * speedMultiplier.Evaluate(power));
            body.velocity = movementDirection;
            timer.DeleteTimer(reHitTimer);
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
            character.Strike();
        }

        #region Type of timer

        private void CancelBall()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            power = 0;
            body.velocity = body.velocity * 0.7f;
            body.useGravity = true;
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            character.Physicate(true);
        }

        private void Nothing()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
        }

        #endregion

        private void Bounce(Vector3 enterVector, Vector3 collisionNormal)
        {
            if (body.useGravity) return;
            Vector3 newDir = Vector3.Reflect(enterVector, collisionNormal);
            lastNewDir = newDir;
            power -= 5;
            if (power < 0) power = 0;
            Debug.Log("Power after bounce : " + power);
            movementDirection = newDir.normalized * (speed * speedMultiplier.Evaluate(power));
            body.velocity = movementDirection;
        }

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
    }
}
