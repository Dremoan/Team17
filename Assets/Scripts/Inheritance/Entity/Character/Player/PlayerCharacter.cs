using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider hardCollider;
        [SerializeField] private Animator anim;
        [SerializeField] private float distFromBall = 1.2f;
        [SerializeField] private FeedBack tpFeedback;
        [Header("Ground check parameters")]
        [SerializeField] private Vector3 feetPosition;
        [SerializeField] private float feetlength = 0.25f;
        [SerializeField] private LayerMask groundMask;

        private PlayerProjectile currentBall;
        private bool criticalShoot;
        private bool negativeAngle = false;
        private float angle = 0;
        private bool aiming = false;
        private bool playedTp = false;
        private bool grounded = false;

        protected override void Update()
        {
            base.Update();
            GroundCheck();
        }

        private void GroundCheck()
        {
            grounded = Physics.Raycast(transform.position + feetPosition, Vector3.down, feetlength, groundMask);
            Debug.DrawRay(transform.position + feetPosition, Vector3.down * feetlength, Color.red);
            anim.SetBool("Grounded", grounded);
        }

        public void Physicate(bool physicate)
        {
            body.useGravity = physicate;
            hardCollider.enabled = physicate;
            body.velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0, 90, 0);
            anim.SetBool("aiming", !physicate);
        }

        public void PrepareStrike(Vector3 ballPos, Vector3 touchPos)
        {
            transform.position = ballPos + (ballPos - touchPos).normalized * distFromBall;
            if (!playedTp)
            {
                tpFeedback.Play();
                playedTp = true;
            }
            angle = Vector3.SignedAngle(Vector3.up, (ballPos - touchPos), Vector3.forward);
            negativeAngle = (angle < 0);
            angle = Mathf.Abs(angle);
            anim.SetFloat("angle", angle);
            anim.SetBool("angleNegative", negativeAngle);
        }

        public void Strike()
        {
            playedTp = false;
            anim.SetTrigger("shoot");
            anim.SetBool("CriticalShoot", criticalShoot);
        }

        public void TriggerLaunchBall()
        {
            currentBall.LaunchBall();
        }

        public void TeleportToRoom(Transform spawnPoint)
        {
            transform.position = spawnPoint.position;
            if (currentBall != null) currentBall.PauseBehavior();
            currentBall.transform.position = spawnPoint.position + new Vector3(0.75f, 0.15f, 0f);
            tpFeedback.Play();
        }

        public void TeleportAndTaunt(Transform tauntPoint)
        {
            transform.position = tauntPoint.position;
            if (currentBall != null) currentBall.PauseBehavior();
            anim.Play("TauntIdle");
            tpFeedback.Play();
        }

        public void TeleportAndActiveBall(Transform spawnPoint)
        {
            anim.SetTrigger("TauntToIdle");
            transform.position = spawnPoint.position;
            if (currentBall != null) currentBall.gameObject.SetActive(true);
            currentBall.transform.position = spawnPoint.position + new Vector3(0.75f, 0.15f, 0f);
            tpFeedback.Play();
        }


        public FeedBack TpFeedback { get => tpFeedback; }
        public PlayerProjectile CurrentBall { get => currentBall; set => currentBall = value; }
        public bool CriticalShoot { get => criticalShoot; set => criticalShoot = value; }
    }
}
