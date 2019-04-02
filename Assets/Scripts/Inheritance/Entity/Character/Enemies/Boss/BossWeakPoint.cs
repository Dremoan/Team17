using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class BossWeakPoint : Character, IBallHitable
    {
        [SerializeField] private Boss linkedBoss;

        public void Hit(float dmgs)
        {
            linkedBoss.Hit(dmgs);
        }
    }
}