using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Team17.StreetHunt
{
    public class UiManager : UiElement
    {
        [SerializeField] private UiSceneManagement sceneManager;
        [SerializeField] private Animator transitionsCanvas;

        [Header("End Level Ui"), SerializeField] private GameObject endLevelUi;
        [SerializeField] private Animator animatorUiEndLevel;

        [HideInInspector] public bool endLevelVictory = false;

        [Header("Health Management"), SerializeField] private GameObject[] nbreBallsArray;
        int nbreBall = 0;

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
        }

        public override void OnEndCutSceneEnds()
        {
            base.OnEndCutSceneEnds();
            endLevelUi.SetActive(true);
            animatorUiEndLevel.SetTrigger("AnimWinScreen");
        }

        public override void OnLevelEnd()
        {
            base.OnLevelEnd();
        }

        public override void OnBallHit(int powerGroupIndex, float hitPower)
        {
            base.OnBallHit(powerGroupIndex, hitPower);
            //GuiNbreBalls();
        }

        #region Buttons management

        public void LoadMenuScene()
        {
            animatorUiEndLevel.SetTrigger("animEndLevelMenu");
        }

        public void ReloadScene()
        {
            animatorUiEndLevel.SetTrigger("animEndLevelReload");
        }

        #endregion

        #region GuiHealth

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

        #endregion

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            GuiNbreBalls();
            animatorCanvasAnim.Play("FlashRed");
            if (GameManager.state.LivesLeft <= 0)
            {
                OnPlayerDeath();
            }

        }

        private void OnPlayerDeath()
        {
            endLevelUi.SetActive(true);
            animatorUiEndLevel.SetTrigger("AnimLoseScreen");
        }

        public void BackToMenu()
        {
            StartCoroutine(DelayBackMenu());
        }

        IEnumerator DelayBackMenu()
        {
            animatorCanvasAnim.Play("FadeIn");
            yield return new WaitForSeconds(1f);
            sceneManager.LoadSceneIndex(0);
        }

    }
}
