using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    [System.Serializable]
    public class BossAimZone
    {
        [SerializeField] private Vector3 zoneCenter = Vector3.zero;
        [SerializeField] private float zoneWeigth = 1f;

        [SerializeField] private Vector3 topRight;
        [SerializeField] private Vector3 topLeft;
        [SerializeField] private Vector3 botRight;
        [SerializeField] private Vector3 botLeft;

        public void Contains(Vector3 pos)
        {
            
        }

        public float ZoneWeigth { get => zoneWeigth; set => zoneWeigth = value; }
        public Vector3 ZoneCenter { get => zoneCenter; set => zoneCenter = value; }
        public Vector3 TopRight { get => topRight; set => topRight = value; }
        public Vector3 TopLeft { get => topLeft; set => topLeft = value; }
        public Vector3 BotRight { get => botRight; set => botRight = value; }
        public Vector3 BotLeft { get => botLeft; set => botLeft = value; }
    }
}
