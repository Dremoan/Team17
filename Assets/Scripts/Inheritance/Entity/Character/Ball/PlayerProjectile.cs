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
        [SerializeField] private Transform player;
        [Header("Parameters")]
        [SerializeField] private float speed = 50f;
        [SerializeField] private float slowedTimeScale = 0.2f;
        [SerializeField] private AnimationCurve speedMultiplier;
        [SerializeField] private AnimationCurve timeToHit;
        private int numberOfHits = 0;
        private int reHitTimer;
        private bool wasCanceled = false;
        private Vector3 initialFeedbackScale;

        protected override void Start()
        {
            base.Start();
            initialFeedbackScale = timerFeedback.localScale;
        }

        public void StartCalculation()
        {
            Time.timeScale = slowedTimeScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            reHitTimer = timer.LaunchNewTimer(timeToHit.Evaluate(numberOfHits) * slowedTimeScale, CancelRehit);
            timerFeedback.gameObject.SetActive(true);
            trajectory.gameObject.SetActive(true);
            player.gameObject.SetActive(true);
            wasCanceled = false;
        }

        public void FeedBack(Vector3 touchPos)
        {
            Timer t = timer.GetTimerFromUserIndex(reHitTimer);
            timerFeedback.localScale = Mathf.InverseLerp(0, t.MaxTime, t.TimeLeft) * initialFeedbackScale;
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
            body.velocity = newDirection.normalized * (speed * speedMultiplier.Evaluate(numberOfHits));
            timer.DeleteTimer(reHitTimer);
            numberOfHits++;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }

        private void CancelRehit()
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            numberOfHits = 0;
            body.velocity = body.velocity * 0.5f;
            body.useGravity = true;
            wasCanceled = true;
            timerFeedback.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            player.gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision coll)
        {
            if(coll.gameObject.GetComponent<BallCanceler>() != null)
            {
                CancelRehit();
            }
        }
    }
}
