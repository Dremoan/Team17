using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class Player : Entity
    {
        [SerializeField] private Rigidbody body;
        [SerializeField] private PlayerProjectile ball;
        [SerializeField] private SpriteRenderer background;
        [SerializeField] private Transform trajectory;
        private bool canLaunch = true;
        private Vector3 beginSwipe;
        private Vector3 newBallDirection;

        public void Teleport(Vector3 pos)
        {
            body.velocity = Vector3.zero;
            transform.position = pos;
        }

        public void ShowTrajectory(Vector3 endPos)
        {
            Vector3 dir = (endPos - transform.position);
            float zRot = Vector3.SignedAngle(Vector3.up, dir, Vector3.forward) + 90;
            float distance = Vector3.Distance(transform.position, endPos);
            trajectory.transform.localScale = new Vector3(distance, 0.7f, 1);
            trajectory.transform.position = Vector3.Lerp(transform.position, endPos, 0.5f);
            trajectory.transform.rotation = Quaternion.Euler(0, 0, zRot);
        }

        public void SetBeginSwipe()
        {
            if (!canLaunch) return;
            beginSwipe = transform.position;
            background.gameObject.SetActive(true);
            trajectory.gameObject.SetActive(true);
        }

        public void SetEndSwipe(Vector3 pos)
        {
            if (!canLaunch) return;
            newBallDirection = (pos - beginSwipe);
            ball.transform.position = transform.position;
            ball.SetNewCourse(newBallDirection);
            background.gameObject.SetActive(false);
            trajectory.gameObject.SetActive(false);
            canLaunch = false;
        }

        public bool CanLaunch { get => canLaunch; set => canLaunch = value; }
    }
}
