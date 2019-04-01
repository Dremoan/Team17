using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.BallDash
{
    public class PauseMenu : MonoBehaviour
    {

        void Start()
        {
            //Time.timeScale = 0f;
        }

        public void ReturnToMenu()
        {
            Debug.Log("Load main menu scene");
            //SceneManager.LoadScene(0);
        }

        public void RetryLevel()
        {
            Debug.Log("Reload Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void PauseFinish()
        {
            //Time.timeScale = 1f;
        }
    }
}
