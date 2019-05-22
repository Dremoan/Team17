using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaJumpTarget : Environment
    {
        [SerializeField] private float gorillaIdleValue = 0f;

        public float GorillaIdleValue { get => gorillaIdleValue; set => gorillaIdleValue = value; }
    }
}
