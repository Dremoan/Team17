using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class WarpManager : Entity
    {
        [SerializeField] private GameObject[] warps;
        [SerializeField] private Animator[] warpsAnims;
        [SerializeField] private SnakeBoss snakeBossScript;

        public void ResetAllWarps()
        {
            for (int i = 0; i < warps.Length; i++)
            {
                if(warps[i].active)
                {
                    warpsAnims[i].SetTrigger("AnimWarpDisappear");
                }
                else
                {
                    return;
                }
                //warps[i].SetActive(false);
            }
        }

        public void SpawnWarps()
        {
            StartCoroutine(WarpsAppear());
        }

        IEnumerator WarpsAppear()
        {
            //yield return new WaitForSeconds(.5f);
            for (int i = 0; i < warps.Length; i++)
            {
                warps[i].SetActive(true);
                yield return new WaitForSecondsRealtime(.5f);
            }
            snakeBossScript.PickMove();
        }
    }
}
