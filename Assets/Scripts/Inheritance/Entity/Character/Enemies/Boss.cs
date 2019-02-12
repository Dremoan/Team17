using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class Boss : Entity
    {
        [Header("Parameters")]
        [SerializeField] private TimersCalculator timer;
        [SerializeField] private float minWaitTime = 5f;
        [SerializeField] private float maxWaitTime = 10f;
        [SerializeField] private float speed = 0.2f;
        [SerializeField] private BossPattern[] patterns;
        [Header("Feedback")]
        [SerializeField] private Transform timerFeedback;
        private BossPattern currentPattern;
        private Vector3 currentWorldPosition;
        private Vector3 posRef;
        private Vector3 initialFeedbackScale;
        private int timerIndex;

        protected override void Start()
        {
            base.Start();
            currentWorldPosition = transform.position;
            timerIndex = timer.LaunchNewTimer(Random.Range(minWaitTime, maxWaitTime), SpawnNewPattern);
            initialFeedbackScale = timerFeedback.localScale;
        }

        protected override void Update()
        {
            base.Update();
            transform.position = Vector3.SmoothDamp(transform.position, currentWorldPosition, ref posRef, speed);
            Timer t = timer.GetTimerFromUserIndex(timerIndex);
            timerFeedback.localScale = Mathf.InverseLerp(0, t.MaxTime, t.TimeLeft) * initialFeedbackScale;
        }

        private void SpawnNewPattern()
        {
            if(currentPattern.PatternGameobject == null)
            {
                currentPattern = patterns[Random.Range(0, patterns.Length)];
            }
            else
            {
                currentPattern.PatternGameobject.SetActive(false);
                currentPattern = patterns[Random.Range(0, patterns.Length)];
            }
            currentPattern.PatternGameobject.SetActive(true);
            currentWorldPosition = currentPattern.BossWorldPosition;
            timerIndex = timer.LaunchNewTimer(Random.Range(minWaitTime, maxWaitTime), SpawnNewPattern);
        }
    }

    [System.Serializable]
    public struct BossPattern
    {
        [SerializeField] private string name;
        [SerializeField] private Vector3 bossWorldPosition;
        [SerializeField] private GameObject patternGameobject;

        public Vector3 BossWorldPosition { get => bossWorldPosition; set => bossWorldPosition = value; }
        public GameObject PatternGameobject { get => patternGameobject; set => patternGameobject = value; }
    }
}
