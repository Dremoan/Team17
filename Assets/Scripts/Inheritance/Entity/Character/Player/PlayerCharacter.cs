﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider hardCollider;
        [SerializeField] private Animator anim;
        [SerializeField] private float distFromBall = 1.2f;
        [SerializeField] private FeedBack tpFeedback;

        private PlayerProjectile actualBall;
        private bool negativeAngle = false;
        private float angle = 0;
        private bool aiming = false;
        private bool playedTp = false;


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
            actualBall.LaunchBall();
        }

        public FeedBack TpFeedback { get => tpFeedback; }
        public PlayerProjectile ActualBall { get => actualBall; set => actualBall = value; }
    }
}
