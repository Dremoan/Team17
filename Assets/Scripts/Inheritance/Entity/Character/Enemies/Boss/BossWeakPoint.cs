﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class BossWeakPoint : Character, IBallHitable
    {
        [SerializeField] private Boss linkedBoss;

        public void Hit(int index, float dmgs)
        {
            linkedBoss.Hit(index, dmgs);
        }
    }
}