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
    }

    [System.Serializable]
    public struct SnakeBossPattern
    {
        [SerializeField] private string name;
        [SerializeField] private BossAimZone zone;
    }
}
