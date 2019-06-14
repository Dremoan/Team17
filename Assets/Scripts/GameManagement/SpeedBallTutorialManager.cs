using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedBallTutorialManager : Entity
    {
        [SerializeField] private SpeedPortal speedPortal;
        [SerializeField] private float maxX = 5f;
        [SerializeField] private float maxY = 5f;
        [SerializeField] private float timeBeforeActivation = 1.5f;

        protected override void Start()
        {
            base.Start();
            speedPortal.GetReadyForReactivation();
            speedPortal.Activate(transform.position + new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0), 0f, timeBeforeActivation);
        }

        public override void OnSpeedPortalCrossed()
        {
            base.OnSpeedPortalCrossed();
            speedPortal.GetReadyForReactivation();
            speedPortal.Activate(transform.position + new Vector3(Random.Range(-maxX, maxX), Random.Range(-maxY, maxY), 0), 0f, timeBeforeActivation);
        }

    }
}
