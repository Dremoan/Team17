using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Otumn.FPSCounter
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private Text fpsText;
        float fps;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            fps = 1.0f / Time.deltaTime;
            fpsText.text = fps.ToString("F") + " fps";
        }
    }
}
