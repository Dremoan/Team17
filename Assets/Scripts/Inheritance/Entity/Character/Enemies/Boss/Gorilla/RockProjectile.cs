using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class RockProjectile : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float disapearTime = 3f;
        [SerializeField] private GameObject[] possibleVisuals;
        private Transform holdingTransform;
        private bool available = true;
        private bool isHeld = false;

        protected override void Update()
        {
            base.Update();
            HoldingManagement();
        }

        public void HeldBy(Transform holdTransfom)
        {
            holdingTransform = holdTransfom;
            isHeld = true;
            for (int i = 0; i < possibleVisuals.Length; i++)
            {
                possibleVisuals[i].SetActive(false);
            }
            possibleVisuals[Random.Range(0, possibleVisuals.Length)].SetActive(true);
        }

        public void Launch(Vector3 direction)
        {
            isHeld = false;
            available = false;
            body.velocity = direction.normalized * speed;
        }

        private void HoldingManagement()
        {
            if(isHeld)
            {
                available = false;
                transform.position = holdingTransform.position;
                transform.rotation = holdingTransform.rotation;
            }
        }

        private IEnumerator DisapearDelay()
        {
            yield return new WaitForSeconds(disapearTime);
            available = true;
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!available)
            {
                body.velocity = Vector3.zero;
                body.angularVelocity = Vector3.zero;
                StartCoroutine(DisapearDelay());
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 12) // 12 == wall
            {
                if (!available)
                {
                    body.velocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                    StartCoroutine(DisapearDelay());
                }
            }
        }

        public bool Available { get => available; set => available = value; }
    }
}
