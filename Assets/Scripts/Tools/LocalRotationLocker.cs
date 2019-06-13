using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRotationLocker : MonoBehaviour
{
    private Quaternion initialRotation;

    private void Awake()
    {
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = initialRotation;
    }
}
