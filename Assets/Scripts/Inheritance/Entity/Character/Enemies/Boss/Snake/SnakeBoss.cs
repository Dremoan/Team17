using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace Team17.BallDash
{
    public class SnakeBoss : Entity
    {
        [SerializeField] private SnakeHead[] snakeBodyParts;
        [SerializeField] private PathCreator[] pathPull;
        private PathCreator actualPath;
        
        [SerializeField] private Transform snakeHead;
        [SerializeField] private Animator snakeAnim;
        

        [SerializeField] private float delayFollowSnakeChunks = 0.25f;
        private int indexPath;

        protected override void Start()
        {
            //GetPath(2);
            //PickMoveIntro();
        }

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

        public void AssignAttack(int indexToAssign)
        {
            snakeAnim.SetFloat("AttackZoneIndex", 0f);
            ResetPositionsEvent();
            snakeAnim.SetInteger("AttackZoneIndex", indexToAssign);
        }
    }
}


