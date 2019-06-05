using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SnakeTailFeathersFollower : Entity
    {
        [SerializeField] private Transform transformToFollow;

        protected override void Update()
        {
            base.Update();
            transform.position = transformToFollow.position;
            transform.rotation = transformToFollow.rotation;
        }
    }
}
