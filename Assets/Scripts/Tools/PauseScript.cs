using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private KeyCode pauseKey = KeyCode.Space;

    private bool paused = false;

    private void Update()
    {
        if(Input.GetKeyDown(pauseKey))
        {
            Debug.Break();
        }
    }
}
