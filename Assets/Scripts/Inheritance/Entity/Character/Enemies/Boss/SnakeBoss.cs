using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class SnakeBoss : Boss
    {
        [Header("Snake parameters")]
        [SerializeField] private BossAimZone zone;
        [SerializeField] private int y;

        protected override void Update()
        {
            base.Update();
            Debug.DrawLine(zone.ZoneCenter, zone.TopLeft, Color.blue);
            Debug.DrawLine(zone.ZoneCenter, zone.TopRight, Color.magenta);
            Debug.DrawLine(zone.ZoneCenter, zone.BotLeft, Color.red);
            Debug.DrawLine(zone.ZoneCenter, zone.BotRight, Color.green);
        }
    }

    [System.Serializable]
    public struct SnakeBossPattern
    {
        [SerializeField] private string name;
        [SerializeField] private BossAimZone zone;
    }
}
