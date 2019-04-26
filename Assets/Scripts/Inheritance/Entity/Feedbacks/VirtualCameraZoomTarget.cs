using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class VirtualCameraZoomTarget : Entity
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            GameManager.state.RegisterVirtualCameraZoomTarget(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            GameManager.state.UnregisterVirtualCameraZoomTarget(this);
        }
    }
}
