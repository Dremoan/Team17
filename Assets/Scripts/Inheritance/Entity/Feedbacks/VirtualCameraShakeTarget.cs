using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class VirtualCameraShakeTarget : Entity
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.state.RegisterVirtualCameraShakeTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.state.UnregisterVirtualCameraShakeTarget(this);
        }
    }
}
