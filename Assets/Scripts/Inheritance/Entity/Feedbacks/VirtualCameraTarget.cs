using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class VirtualCameraTarget : Entity
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.state.RegisterVirtualCameraTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.state.UnregisterVirtualCameraTarget(this);
        }
    }
}
