using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class BallRelfecter : MonoBehaviour
    {
        [SerializeField] private float stunTime = 1f;

        public float StunTime { get => stunTime; }
    }
}
