using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Team17.StreetHunt
{
    public class UiManager : UiElement
    {

        [SerializeField] private Animator transitionsCanvas;
        // ------ Player Health Management ------
        [Header("End Level Ui"), SerializeField] private GameObject EndLevelUi;
        [SerializeField] private TextMeshProUGUI textEndGame;
        [SerializeField] private Animator animatorUiEndLevel;

        [Header("Health Management"), SerializeField] private GameObject[] nbreBallsArray;
        int nbreBall = 0;
        bool endLevelVictory = false;

        protected override void Start()
        {
            base.Start();
            transitionsCanvas.Play("FadeOut");
        }

        protected override void Update()
        {
            if (GameManager.state.LivesLeft <= 0)
            {
                //GUiEndLevel(endLevelVictory);
            }
        }

        public override void OnLevelEnd()
        {
            base.OnLevelEnd();
            endLevelVictory = true;
            GUiEndLevel(endLevelVictory);
        }

        public override void OnBallHit(int powerGroupIndex, float hitPower)
        {
            base.OnBallHit(powerGroupIndex, hitPower);
            //GuiNbreBalls();
        }
        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            GuiNbreBalls();
        }

        private void GuiNbreBalls()
        {
            nbreBall = GameManager.state.LivesLeft;
            //Debug.Log("GameManager.state.LivesLeft : " + GameManager.state.LivesLeft);
            //Debug.Log("nbreBall : " + nbreBall);
            if ((nbreBall) >= 0)
            {
                nbreBallsArray[nbreBall].SetActive(false);
            }
            else if (nbreBall < 0)
            {
                Debug.LogWarning("Balls number can't be under 0 !");
            }
        }
        private void GUiEndLevel(bool endLevelVictory)
        {
            if (endLevelVictory == false)
            {
                textEndGame.color = new Color(186 / 255, 0 / 255, 2 / 255);
                textEndGame.text = "Defeat!";
                EndLevelUi.SetActive(true);
            }
            else
            {
                textEndGame.color = new Color(20 / 255, 220 / 255, 0 / 255);
                textEndGame.text = "Victory!";
                EndLevelUi.SetActive(true);
            }
        }

        // ------ Button management ------

        public void LoadMenuScene()
        {
            animatorUiEndLevel.SetTrigger("animEndLevelMenu");
        }

        public void ReloadScene()
        {
            animatorUiEndLevel.SetTrigger("animEndLevelReload");
        }

    }
}
