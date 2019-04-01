using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class BallFxTest : MonoBehaviour
    {

        [SerializeField] private GameObject ballTestPrefab;

        [SerializeField] private KeyCode testKey = KeyCode.L;
        [SerializeField] private float testSpeed = 10f;
        [SerializeField] private float launchLifeTime = 5f;
        [SerializeField] private float impactLifeTime = 5f;

        [Header("Partcles")]
        [SerializeField] private GameObject launchToTest;
        [SerializeField] private GameObject trailToTest;
        [SerializeField] private GameObject impactToTest;

        private void Update()
        {
            if(Input.GetKeyDown(testKey))
            {
                GameObject clone = GameObject.Instantiate(ballTestPrefab, transform.position, Quaternion.identity);
                clone.GetComponent<BallTestBehaviour>().Launch(testSpeed, launchLifeTime, impactLifeTime, launchToTest, trailToTest, impactToTest);
            }
        }
    }
}
