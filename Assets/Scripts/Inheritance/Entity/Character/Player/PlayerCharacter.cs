using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class PlayerCharacter : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private Collider hardCollider;

        private float angle = 0;

        public void Physicate(bool physicate)
        {
            body.useGravity = physicate;
            hardCollider.enabled = physicate;
            body.velocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        public void PrepareStrike(Vector3 ballPos, Vector3 touchPos)
        {
            transform.position = ballPos + (ballPos - touchPos).normalized * 1.2f;
            angle = Vector3.SignedAngle(Vector3.up, (ballPos - touchPos), Vector3.forward);
            Debug.Log(angle);
        }

        public void Strike()
        {

        }
    }
}
