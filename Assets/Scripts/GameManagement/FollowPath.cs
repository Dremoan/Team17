using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.Events;

public class FollowPath : MonoBehaviour
{
    public PathCreator pathCreator;
    public float speed = 5f;
    public bool pathEnded;
    public bool startPathFollowing;
    private float distanceTravelled;
    public UnityEvent endSpline;
    private Vector3 initialPos;
    private Quaternion initialRot;

    private void Start()
    {
        initialPos = transform.localPosition;
        initialRot = transform.localRotation;
    }
    public void Update()
    {
        MoveAlongSpline();
        EndSplineResetValues();
    }

    public void MoveAlongSpline()
    {
        if (startPathFollowing)
        {
            pathCreator.path.ended = false;
            distanceTravelled += speed * Time.fixedDeltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.StopAndTrigger);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.StopAndTrigger);
        }

    }

    public void EndSplineResetValues()
    {
        if (pathCreator.path.ended == true)
        {
            startPathFollowing = false;
            distanceTravelled = 0f;
            endSpline.Invoke();
        }
    }

    public void ResetPositions()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
    }
    
}
