using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedPortalManager : Entity
    {
        [SerializeField] private SpeedPortal[] portalPool;

        protected override void Update()
        {
            base.Update();
            transform.localScale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
        }

        public void SpawnPortal(Vector3 pos, float rot)
        {
            for (int i = 0; i < portalPool.Length; i++)
            {
                if(portalPool[i].Available)
                {
                    portalPool[i].gameObject.SetActive(true);
                    portalPool[i].SetTargets(pos, rot); // also sets portal[i].Available to false
                    return;
                }
            }
        }

        public void DeactivateAllPortals()
        {
            for (int i = 0; i < portalPool.Length; i++)
            {
                portalPool[i].Available = true;
                portalPool[i].gameObject.SetActive(false);
                portalPool[i].transform.localPosition = Vector3.zero;
            }
        }
    }
}
