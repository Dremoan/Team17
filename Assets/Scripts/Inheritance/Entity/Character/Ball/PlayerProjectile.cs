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
        [SerializeField] private Transform player;
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
            Time.timeScale = slowedTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            switch(type)
            {
                case TypeOfTimer.Cancel:
                    reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(power) * slowedTimeScale, CancelRehit);
                    break;
                case TypeOfTimer.Nothing:
                    reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(power) * slowedTimeScale, Nothing);
                    break;
            }
            timerFeedback.gameObject.SetActive(true);
            trajectory.gameObject.SetActive(true);
            player.gameObject.SetActive(true);
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
            player.localPosition = transform.position - Vector3.Lerp(transform.position, touchPos, 0.2f);
            player.rotation = Quaternion.Euler(0, 0, zRot - 90);
        }

        public void Launch(Vector3 newDirection)
        {
            if(wasCanceled)
            {
                wasCanceled = false;
                return;
            }
            body.useGravity = false;
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            Timer t = timer.GetTimerFromUserIndex(reHitTimer);
            power += (t.Inc / slowedTimeScale) * 1.7f;
            Debug.Log("Power after shot : " + power + ", Added : " + (t.Inc / slowedTimeScale) * 1.2f);
            movementDirection = newDirection.normalized * (speed * speedMultiplier.Evaluate(power));
            body.velocity = movementDirection;
            timer.DeleteTimer(reHitTimer);
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }

        #region Type of timer

        private void CancelRehit()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            power = 0;
            body.velocity = body.velocity * 0.7f;
            body.useGravity = true;
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }

        private void Nothing()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
            wasCanceled = true;
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

        private void OnTriggerEnter(Collider coll)
        {

        }

        private void OnCollisionEnter(Collision coll)
        {
            if(coll.gameObject.GetComponent<BallCanceler>() != null)
            {
                CancelRehit();
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
