using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.BallDash
{
    public class UiMainMenu : Entity
    {
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
