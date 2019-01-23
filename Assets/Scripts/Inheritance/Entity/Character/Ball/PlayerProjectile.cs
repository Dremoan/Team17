using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class PlayerProjectile : Character
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float speed = 50f;

        public void SetNewCourse(Vector3 newDirection)
        {
            body.velocity = newDirection.normalized * speed;
        }
    }
}
