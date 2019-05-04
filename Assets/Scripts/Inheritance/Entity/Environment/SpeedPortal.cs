using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedPortal : Environment
    {
        [SerializeField] private float placementSpeed = 1;

        private Vector3 targetPos;
        private Vector3 refTargetPos;
        private float targetZRot;
        private bool available = true;

        protected override void Update()
        {
            base.Update();
            if(!available)
            {
                PosRotManagement();
            }
        }

        private void PosRotManagement()
        {
            transform.rotation = Quaternion.Euler(0, 0, targetZRot);
            //transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref refTargetPos, placementSpeed);
            transform.position = targetPos;
        }

        public void SetTargets(Vector3 pos, float rot)
        {
            targetPos = pos;
            targetZRot = rot;
            available = false;
        }

        public bool Available { get => available; set => available = value; }
    }
}
