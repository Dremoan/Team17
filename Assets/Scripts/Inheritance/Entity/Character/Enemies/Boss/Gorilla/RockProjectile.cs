using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class RockProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float speed = 5f;
        private bool available = true;
        
        public void Launch(Vector3 direction)
        {
            available = false;
            body.velocity = direction.normalized * speed;

        }

        public bool Available { get => available; set => available = value; }
    }
}
