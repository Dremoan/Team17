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

        [SerializeField] private Vector3 topRight = new Vector3(2, 2);
        [SerializeField] private Vector3 topLeft = new Vector3(-2, 2);
        [SerializeField] private Vector3 botRight = new Vector3(2, -2);
        [SerializeField] private Vector3 botLeft = new Vector3(-2, -2);

        public bool Contains(Vector3 pos)
        {
            // from top right
            Vector3 topRightTopLeft = (topLeft - topRight) * 0.5f;
            Vector3 topRightBotRight = (botRight - topRight) * 0.5f;

            Vector3 center = topRight + new Vector3(topRightTopLeft.x, topRightBotRight.y, 0);
            float width = Vector3.Distance(new Vector3(topRight.x, center.y, 0), center);
            float height = Vector3.Distance(new Vector3(center.x, topRight.y, 0), center);

            Rect rect = new Rect(center, new Vector2(width, height));
            return rect.Contains(pos);
        }

        public float ZoneWeigth { get => zoneWeigth; set => zoneWeigth = value; }
        public Vector3 ZoneCenter { get => zoneCenter; set => zoneCenter = value; }
        public Vector3 TopRight { get => topRight; set => topRight = value; }
        public Vector3 TopLeft { get => topLeft; set => topLeft = value; }
        public Vector3 BotRight { get => botRight; set => botRight = value; }
        public Vector3 BotLeft { get => botLeft; set => botLeft = value; }
    }
}
