using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

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
        public Transform snakeHead;
        public PathCreator[] pathPull;
        private PathCreator actualPath;
        public float delayFollowSnakeChunks = 0.25f;
        public FollowPath[] snakeBodyParts;

        protected override void Start()
        {
            base.Start();
            PickMove();
        }

        protected override void Update()
        {
            base.Update();
        }


        IEnumerator Delay()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].startPathFollowing = true;
                yield return new WaitForSeconds(delayFollowSnakeChunks);
            }
        }

        public void PickMove()
        {
            ResetPositionsEvent();
            snakeBodyParts[0].pathCreator = GetRandomPath();

            for (int i = 1; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].pathCreator = snakeBodyParts[0].pathCreator;
            }

            StartCoroutine(Delay());
        }

        public void PickAttack()
        {
            snakeBodyParts[0].pathCreator = GetRandomPath();

            for (int i = 1; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].pathCreator = snakeBodyParts[0].pathCreator;
            }
            
        }

        public PathCreator GetRandomPath()
        {
            actualPath = pathPull[Random.Range(0, pathPull.Length)];
            return actualPath;
        }

        public void ResetPositionsEvent()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].ResetPositions();
            }
        }
    }
}


