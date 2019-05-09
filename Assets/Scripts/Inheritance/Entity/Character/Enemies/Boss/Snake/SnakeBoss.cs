using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace Team17.StreetHunt
{
    public class SnakeBoss : Entity
    {
        [SerializeField] private SnakeHead[] snakeBodyParts;
        [SerializeField] private PathCreator[] pathPull;
        [SerializeField] private WarpManager[] warpPull;
        private WarpManager actualWarps;
        private PathCreator actualPath;
        
        [SerializeField] private Transform snakeHead;
        

        [SerializeField] private float delayFollowSnakeChunks = 0.25f;
        private int indexPath;


            #region Spline movement

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


        public void PickMove()
        {
            snakeBodyParts[0].pathCreator = actualPath;

            for (int i = 1; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].pathCreator = snakeBodyParts[0].pathCreator;
            }

            StartCoroutine(Delay());
        }
        
        public void GetPath(int index)
        {
            for (int i = 0; i < warpPull.Length; i++)
            {
                warpPull[i].ResetAllWarps();
            }

            indexPath = index;
            AssignPath();
            actualWarps.SpawnWarps();
        }

        public PathCreator AssignPath()
        {
            ResetPositionsEvent();
            actualPath = pathPull[indexPath];
            actualWarps = warpPull[indexPath];
            return actualPath;
        }

        public void ResetPositionsEvent()
        {
            for (int i = 0; i < snakeBodyParts.Length; i++)
            {
                snakeBodyParts[i].EndSplineResetValues();
            }
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

        #endregion
    }
}


