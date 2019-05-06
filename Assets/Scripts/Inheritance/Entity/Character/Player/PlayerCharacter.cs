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

        private PlayerProjectile currentBall;
        private bool negativeAngle = false;
        private float angle = 0;
        private bool aiming = false;
        private bool playedTp = false;

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Ground")
            {
                anim.SetTrigger("Grounded");
            }
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
        }

        public void TriggerLaunchBall()
        {
            currentBall.LaunchBall();
        }

        public void TeleportToRoom(Transform spawnPoint)
        {
            transform.position = spawnPoint.position;
            if(currentBall != null) currentBall.PauseBehavior();
            currentBall.transform.position = spawnPoint.position + new Vector3(0.75f,0.15f,0f);
            tpFeedback.Play();
        }

        public FeedBack TpFeedback { get => tpFeedback; }
        public PlayerProjectile CurrentBall { get => currentBall; set => currentBall = value; }
    }
}
