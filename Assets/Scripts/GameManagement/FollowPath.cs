using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class FollowPath : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private BossManagement bossManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private bool startMoveAlongSpline; 
    private float distanceTravelled;
    private bool launchNext;
    private int pathCount;

    public void Update()
    {
        Debug.Log(pathCreator.path.ended);
        if(Input.GetMouseButtonDown(1))
        {
            pathCreator.path.ended = false;
        }

        if(pathCreator.path.ended == false)
        {
            launchNext = false;
            distanceTravelled += speed * Time.fixedDeltaTime;
            transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
            transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        }
        else if(pathCreator.path.ended == true && launchNext)
        {
            pathCount++;
            launchNext = false;
            Debug.Log("Chemin numéro" + pathCount + "effectué");
        }

    }
}
