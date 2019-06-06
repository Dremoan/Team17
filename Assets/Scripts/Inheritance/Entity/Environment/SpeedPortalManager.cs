using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedPortalManager : Entity
    {
        [SerializeField] private bool usePortals = true;
        [SerializeField] private SpeedPortal[] portalPool;

        protected override void Update()
        {
            base.Update();
            transform.localScale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
        }

        public void SpawnPortal(Vector3 pos, float rot, float time)
        {
            if (!usePortals) return;
            for (int i = 0; i < portalPool.Length; i++)
            {
                if(portalPool[i].Available)
                {
                    portalPool[i].Activate(pos, rot, time); // also sets portal[i].Available to false
                    return;
                }
            }
        }

        public void DeactivateAllPortals()
        {
            if (!usePortals) return;
            for (int i = 0; i < portalPool.Length; i++)
            {
                portalPool[i].gameObject.SetActive(true);
                portalPool[i].GetReadyForReactivation();
            }
        }
    }
}
