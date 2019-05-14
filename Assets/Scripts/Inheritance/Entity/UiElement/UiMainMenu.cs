using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Team17.StreetHunt
{
    public class UiMainMenu : UiElement
    {
        [SerializeField] Animator animatorMainMenu;
        [SerializeField] Animator animatorPlayMenu;

        public void ClickOnPlayButton()
        {
            animatorMainMenu.SetTrigger("anim_Trans_MainMenu_PlayMenu");
        }

        public void ClickOnTrainingButton()
        {
            animatorPlayMenu.SetTrigger("animPlayMenuTraining");
        }

        public void ClickOnBackButton()
        {
            animatorPlayMenu.SetTrigger("PlayMenuBack");
        }

        // ------ PlayMenu ------

        [SerializeField] private int indexBoss = 1;

        public void ClickOnHuntButton()
        {
            SceneManager.LoadScene(indexBoss);
        }

        public void DisplayNextBoss()
        {
            if (indexBoss > 1 && indexBoss < 3)
            {
                indexBoss += 1;
            }
        }

        public void DisplayPreviousBoss()
        {
            if (indexBoss > 1 && indexBoss < 3)
            {
                indexBoss -= 1;
            }
        }
    }
}
