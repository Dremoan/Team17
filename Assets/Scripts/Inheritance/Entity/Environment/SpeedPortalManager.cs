using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class SpeedPortalManager : Entity
    {
        [SerializeField] private SpeedPortal[] portalPool;

        private List<SpeedPortal> chosenPortals = new List<SpeedPortal>();
        private float timeToAppear = 1f;
        private bool apparitionEnabled = false;

        protected override void Update()
        {
            base.Update();
            transform.localScale = new Vector3(1 / transform.parent.localScale.x, 1 / transform.parent.localScale.y, 1 / transform.parent.localScale.z);
            PortalApparition();
        }

        public void SpawnPortal(Vector3 pos, float rot, float time)
        {
            timeToAppear = time;
            for (int i = 0; i < portalPool.Length; i++)
            {
                if(portalPool[i].Available)
                {
                    portalPool[i].Available = false;
                    chosenPortals.Add(portalPool[i]);
                    portalPool[i].SetTargets(pos, rot); // also sets portal[i].Available to false
                    return;
                }
            }
        }

        private void PortalApparition()
        {
            if(apparitionEnabled)
            {
                if(timeToAppear > 0)
                {
                    timeToAppear -= Time.deltaTime;
                }
                else
                {
                    for (int i = 0; i < chosenPortals.Count; i++)
                    {
                        portalPool[i].gameObject.SetActive(true);
                    }
                    chosenPortals.Clear();
                    apparitionEnabled = false;
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

        public bool ApparitionEnabled { get => apparitionEnabled; set => apparitionEnabled = value; }
    }
}
