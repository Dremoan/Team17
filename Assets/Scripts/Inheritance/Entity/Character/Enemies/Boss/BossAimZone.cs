using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    [CreateAssetMenu(fileName = "New Boss Aim Zone", menuName = "Ball Dash/Boss aim zone")]
    public class BossAimZone : ScriptableObject
    {
        [SerializeField] private Vector3 zoneCenter = new Vector3(0, 5);

        [SerializeField] private Vector3 topRight = new Vector3(2, 2);
        [SerializeField] private Vector3 topLeft = new Vector3(-2, 2);
        [SerializeField] private Vector3 botRight = new Vector3(2, -2);
        [SerializeField] private Vector3 botLeft = new Vector3(-2, -2);
        private Vector3 realCenter;

        public bool Contains(Vector3 pos)
        {
            // from top right
            Vector3 topRightTopLeft = (topLeft - topRight) * 0.5f;
            Vector3 topRightBotRight = (botRight - topRight) * 0.5f;

            Vector3 center = topRight + new Vector3(topRightTopLeft.x, topRightBotRight.y, 0);
            realCenter = center;
            float width = Vector3.Distance(new Vector3(topRight.x, center.y, 0), center);
            float height = Vector3.Distance(new Vector3(center.x, topRight.y, 0), center);

            Rect rect = new Rect(botLeft, new Vector2(width*2, height*2));
            return rect.Contains(new Vector2(pos.x, pos.y));
        }

        public Vector3 ZoneCenter { get => zoneCenter; set => zoneCenter = value; }
        public Vector3 TopRight { get => topRight; set => topRight = value; }
        public Vector3 TopLeft { get => topLeft; set => topLeft = value; }
        public Vector3 BotRight { get => botRight; set => botRight = value; }
        public Vector3 BotLeft { get => botLeft; set => botLeft = value; }
        public Vector3 RealCenter { get => realCenter; set => realCenter = value; }
    }
}
