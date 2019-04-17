using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class SnakeWarpSpawner : Entity
    {
        [SerializeField] private FeedBack warpFB;
        [SerializeField] private Transform warpPoint;
        [SerializeField] private Vector3 frontWarpDir;
        [SerializeField] private float frontCastDist = 1.3f;
        [SerializeField] private Vector3 backWarpDir;
        [SerializeField] private float backCastDist = 1.3f;
        [SerializeField] private LayerMask warpLayerMask;

        private bool shouldPlay = true;

        protected override void Update()
        {
            FrontWarpCalculation();
            BackWarpCalculation();
        }

        private void FrontWarpCalculation()
        {
            Vector3 castDir = transform.rotation * frontWarpDir;
            Debug.DrawRay(transform.position, castDir * frontCastDist, Color.green);
            RaycastHit hit;
            Physics.Raycast(transform.position, castDir, out hit, frontCastDist, warpLayerMask);
            if (hit.collider != null)
            {
                if(shouldPlay)
                {
                    warpPoint.position = hit.point;
                    warpFB.Play();
                    shouldPlay = false;
                }
            }
            else
            {
                warpFB.Stop();
            }
        }

        private void BackWarpCalculation()
        {
            Vector3 castDir = transform.rotation * backWarpDir;
            Debug.DrawRay(transform.position, castDir * backCastDist, Color.blue);
            RaycastHit hit;
            Physics.Raycast(transform.position, castDir, out hit, backCastDist, warpLayerMask);
            if(hit.collider != null)
            {
                warpPoint.position = hit.point;
                warpFB.Play();
                if (shouldPlay)
                {
                    shouldPlay = false;
                }
            }
            else
            {
                if(!shouldPlay)
                {
                    warpFB.Stop();
                    shouldPlay = true;
                }
            }
        }

    }
}
