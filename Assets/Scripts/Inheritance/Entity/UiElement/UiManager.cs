using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Team17.StreetHunt
{
    public class UiManager : UiElement
    {

        [SerializeField] private Animator transitionsCanvas;

        [Header("End Level Ui"), SerializeField] private GameObject endLevelUi;
        [SerializeField] private Animator animatorUiEndLevel;

        [Header("Health Management"), SerializeField] private GameObject[] nbreBallsArray;
        int nbreBall = 0;
        bool endLevelVictory = false;

        [Header("FeedBacks"), SerializeField] private Animator animatorCanvasAnim;

        protected override void Start()
        {
            base.Start();
            //transitionsCanvas.Play("FadeOut");
        }

        protected override void Update()
        {
            if (GameManager.state.LivesLeft <= 0)
            {
                //GUiEndLevel(endLevelVictory);
            }
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            endLevelVictory = true;
            //Debug.LogError("ON BOSS DEATH");
        }

        public override void OnEndCutSceneEnds()
        {
            base.OnEndCutSceneEnds();
            //Debug.LogError("ON END CUT SCENE");
            endLevelUi.SetActive(true);
            animatorUiEndLevel.SetTrigger("AnimWinScreen");
        }

        public override void OnLevelEnd()
        {
            base.OnLevelEnd();
            //Debug.LogError("ON LEVEL END");
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
            animatorCanvasAnim.Play("FlashRed");
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
