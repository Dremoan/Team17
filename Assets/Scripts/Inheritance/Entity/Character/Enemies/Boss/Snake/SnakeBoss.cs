using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace Team17.BallDash
{
    public class SnakeBoss : Entity
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
        [SerializeField] private FollowPath[] snakeBodyParts;
        [SerializeField] private PathCreator[] pathPull;
        private PathCreator actualPath;
        
        [SerializeField] private Transform snakeHead;
        [SerializeField] private Animator snakeAnim;
        

        [SerializeField] private float delayFollowSnakeChunks = 0.25f;
        private int indexPath;


        void Start()
        {
            GetPath(2);
            PickMoveIntro();
        }

        IEnumerator Delay()
        {
            snakeBodyParts[0].startPathFollowing = true;
            yield return new WaitForSeconds(delayFollowSnakeChunks);
            for (int i = 1; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].startPathFollowing = true;
                yield return new WaitForSeconds(delayFollowSnakeChunks);
            }
        }

        IEnumerator DelayIntro()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].speed = 10f;
                delayFollowSnakeChunks = 0.2f;
                snakeBodyParts[i].startPathFollowing = true;
                yield return null;
                snakeBodyParts[i].startPathFollowing = false;
                yield return new WaitForSeconds(delayFollowSnakeChunks);
            }
        }

        public void PickMove()
        {
            snakeBodyParts[0].pathCreator = actualPath;

            for (int i = 1; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].pathCreator = snakeBodyParts[0].pathCreator;
            }

            StartCoroutine(Delay());
        }

        public void PickMoveIntro()
        {
            snakeBodyParts[0].pathCreator = actualPath;

            for (int i = 1; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].pathCreator = snakeBodyParts[0].pathCreator;
            }

            StartCoroutine(DelayIntro());
        }

        public void GetPath(int index)
        {
            indexPath = index;
            AssignPath();
        }

        public PathCreator AssignPath()
        {
            ResetPositionsEvent();
            actualPath = pathPull[indexPath];
            return actualPath;
        }

        public void ResetPositionsEvent()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].EndSplineResetValues();
            }
        }

        public void AssignAttack(int indexToAssign)
        {
            snakeAnim.SetFloat("AttackZoneIndex", 0f);
            ResetPositionsEvent();
            snakeAnim.SetInteger("AttackZoneIndex", indexToAssign);
        }

        public void StopMovingSnake()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].startPathFollowing = false;
            }
        }

        public void RestartMovingSnake()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].startPathFollowing = true;
            }
        }


        public void StartMovingSnake()
        {
            StartCoroutine(Delay());
        }
    }
}


