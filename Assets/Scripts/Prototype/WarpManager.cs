using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class WarpManager : Entity
    {
        [SerializeField] private GameObject[] warps;
        [SerializeField] private SnakeBoss snakeBossScript;

        public void SpawnWarps()
        {
            StartCoroutine(WarpsAppear());
        }

        IEnumerator WarpsAppear()
        {
            yield return new WaitForSeconds(.5f);
            snakeBossScript.PickMove();
            for (int i = 0; i < warps.Length; i++)
            {
                warps[i].SetActive(true);
                yield return new WaitForSeconds(.5f);
            }
        }

        public void ResetAllWarps()
        {
            for (int i = 0; i < warps.Length; i++)
            {
                warps[i].SetActive(false);
            }
        }
    }
}
