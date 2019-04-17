using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.BallDash
{
    public class UiMainMenu : UiElement
    {
        [SerializeField] Animator animatorMainMenu;
        [SerializeField] Animator animatorPlayMenu;

        public void ClickOnPlayButton()
        {
            animatorMainMenu.SetTrigger("animDisPlayMainMenu");
        }

        public void ClickOnHuntButton()
        {
            animatorPlayMenu.SetTrigger("animPlayMenuHunt");
        }

        public void ClickOnTrainingButton()
        {
            animatorPlayMenu.SetTrigger("animPlayMenuTraining");
        }

        public void ClickOnBackButton()
        {
            animatorPlayMenu.SetTrigger("PlayMenuBack");
        }
    }
}
