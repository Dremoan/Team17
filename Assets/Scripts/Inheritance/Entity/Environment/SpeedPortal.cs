using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedPortal : Environment
    {
        [SerializeField] private float placementSpeed = 1;
        [Header("Prototype")]
        [SerializeField] private Collider coll;
        [SerializeField] private GameObject deactivatedVisual;
        [SerializeField] private GameObject activatedVisual;
        [SerializeField] private Animator speedPortalAnimator;

        private Vector3 targetPos;
        private Vector3 refTargetPos;
        private float targetZRot;
        private float timeToActivate = 1f;
        private bool available = true;
        private bool activated = false;


        protected override void Update()
        {
            base.Update();
            if(!available)
            {
                PosRotManagement();
                ActivationManagement();
            }
        }

        private void PosRotManagement()
        {
            transform.rotation = Quaternion.Euler(0, 0, targetZRot);
            transform.position = targetPos;
        }

        private void ActivationManagement()
        {
            if(activated)
            {
                if(timeToActivate > 0)
                {
                    timeToActivate -= Time.deltaTime;
                }
                else
                {
                    // TO DO : replace game object activation with animation
                    coll.enabled = true;
                    deactivatedVisual.SetActive(false);
                    activatedVisual.SetActive(true);
                    activated = false;
                }
            }
        }

        public void Activate(Vector3 pos, float rot, float time)
        {
            targetPos = pos;
            targetZRot = rot;
            timeToActivate = time;
            deactivatedVisual.SetActive(true);
            speedPortalAnimator.SetTrigger("AnimSpeedPortalDisplay");
            available = false;
            activated = true;
        }

        public void TutorialActivation()
        {
            coll.enabled = true;
            deactivatedVisual.SetActive(false);
            activatedVisual.SetActive(true);
        }

        public void GetReadyForReactivation()
        {
            coll.enabled = false;
            deactivatedVisual.SetActive(false);
            activatedVisual.SetActive(false);
            available = true;
        }

        public void SpeedBallDesactivation()
        {
            coll.enabled = false;
            speedPortalAnimator.SetTrigger("AnimSpeedPortalDisable");
            available = true;
        }

        public bool Available { get => available; set => available = value; }

        void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<BallCanceler>() != null)
            {
                SpeedBallDesactivation();
            }
        }
    }
}
