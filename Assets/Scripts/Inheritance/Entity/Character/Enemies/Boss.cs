using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class Boss : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float timeBetweenJumps = 2;
        [SerializeField] private float jumpForce = 2;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(DelayJump());
        }

        IEnumerator DelayJump()
        {
            yield return new WaitForSeconds(timeBetweenJumps);
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            StartCoroutine(DelayJump());
        }
    }
}
