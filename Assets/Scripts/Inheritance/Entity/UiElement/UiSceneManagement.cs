using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.BallDash
{
    public class UiSceneManagement : UiElement
    {
        public void LoadSceneIndex(int index)
        {
            Debug.Log("Load scene");
            SceneManager.LoadScene(index);
        }

        public void ReloadActualScene()
        {
            Debug.Log("Reload actual scene");
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
