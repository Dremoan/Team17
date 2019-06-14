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
            if(paused)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02f;
                paused = false;
            }
            else
            {
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0f;
                paused = true;
            }
        }
    }
}
