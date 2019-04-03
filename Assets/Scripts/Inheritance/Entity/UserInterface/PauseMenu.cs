using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.BallDash
{
    public class PauseMenu : Entity
    {

        public void ReturnToMenu(int menuIndex)
        {
            Debug.Log("Load main menu scene");
            SceneManager.LoadScene(menuIndex);
        }

        public void RetryLevel()
        {
            Debug.Log("Reload Level");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ResumeLevel()
        {
            Debug.Log("Resume Level");
            GameManager.state.CallOnResume();
        }

        public void GamePause()
        {
            Debug.Log("Pause Menu");
            GameManager.state.CallOnPause();
        }
    }
}
