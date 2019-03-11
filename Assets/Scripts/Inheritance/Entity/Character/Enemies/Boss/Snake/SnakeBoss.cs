using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class SnakeBoss : Boss
    {
        #region NotNow
        /*
                [Header("Snake parameters")]
                [SerializeField] private BossAimZone zone;
                [SerializeField] private SnakeBossPattern[] patterns;
                [SerializeField] private BossAimZone[] zones;

                protected override void Update()
                {
                    base.Update();
                }

                [ContextMenu("Add")]
                public void Add()
                {
                    zones = new BossAimZone[1];
                    zones[0] = new BossAimZone();
                }
            */
        #endregion
        [SerializeField] private SnakeBossPattern[] patternsArray;


    }

    [System.Serializable]
    public struct SnakeBossPattern
    {
        [SerializeField] private string name;
        [SerializeField] private FollowPath splineArray;
        //[SerializeField] private BossAimZone zone;
    }





}
