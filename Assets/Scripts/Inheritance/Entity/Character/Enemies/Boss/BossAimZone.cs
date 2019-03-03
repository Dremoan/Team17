using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    [System.Serializable]
    public class BossAimZone
    {
        [SerializeField] private Vector3 zoneCenter = Vector3.zero;
        [SerializeField] private float zoneRay = 1f;
        [SerializeField] private float zoneWeigth = 1f;

        public Vector3 ZoneCenter { get => zoneCenter; set => zoneCenter = value; }
        public float ZoneRay { get => zoneRay; set => zoneRay = value; }
        public float ZoneWeigth { get => zoneWeigth; set => zoneWeigth = value; }
    }
}
