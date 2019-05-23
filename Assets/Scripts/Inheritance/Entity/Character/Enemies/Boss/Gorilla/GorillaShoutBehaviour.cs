using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaShoutBehaviour : Entity
    {
        [Header("Components")]
        [SerializeField] private SphereCollider coll;
        [Header("Parameters")]
        [SerializeField] private float propagationSpeed = 0.1f;
        [SerializeField] private AnimationCurve propagationCurve;
        [Header("Prototype")]
        [SerializeField] private SpriteRenderer currentWaveSprite;
        [SerializeField] private SpriteRenderer goalWaveSprite;



    }
}
