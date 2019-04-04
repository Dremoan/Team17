using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace Team17.BallDash
{
    public class Dummy : Character, IBallHitable
    {
        [Header("UI Elements")]
        [SerializeField] TextMeshProUGUI textTutorial;
        [SerializeField] GameObject endGame;
        [SerializeField] GameObject uIcanvas;
        [SerializeField] Animator animatorTutorialText;

        string textDisplay;
        int shotNbre;
        bool tutorialEnding = false;
        bool colorRed = false;

        [Header("Power thresholds")]
        [SerializeField, Range(0, 400)] float smallThresholdPower = 2;
        [SerializeField, Range(0, 400)] float mediumThresholdPower = 5;
        [SerializeField, Range(0, 400)] float strongThresholdPower = 8;
        [SerializeField, Range(0, 400)] float eliteThresholdPower = 10;

        [Header("Color bad instructions"), Range(0,255), SerializeField] float Rb;
        [Range(0, 255),SerializeField] float Gb, Bb, Ab;
        [Header("Color good instructions"), Range(0, 255), SerializeField] float Rg;
        [Range(0, 255), SerializeField] float Gg, Bg, Ag;

        protected override void Start()
        {
            base.Start();
            textDisplay = "Hold and release!";
            SetActiveText(textDisplay);
        }

        protected override void Update()
        {
            if(GameManager.state.LivesLeft <= 0)
            {
                endGame.SetActive(true);
            }
        }

        public void Hit(float dmgs)
        {
            if (tutorialEnding == false)
            {
                if (dmgs < smallThresholdPower)
                {
                    //Debug.Log("smallPower : " + smallThresholdPower);
                    textDisplay = "Hit the ball several times before attack.";
                    colorRed = true;
                    SetActiveText(textDisplay);
                    shotNbre = 0;
                }
                else if (dmgs < mediumThresholdPower)
                {
                    //Debug.Log("mediumPower : " + mediumThresholdPower);
                    textDisplay = "You need more power!";
                    colorRed = true;
                    SetActiveText(textDisplay);
                    shotNbre = 0;
                }
                else if (dmgs < strongThresholdPower)
                {
                    //Debug.Log("strongPower : " + strongThresholdPower);
                    textDisplay = "You can do better!";
                    colorRed = true;
                    SetActiveText(textDisplay);
                    shotNbre = 0;
                }
                else
                {
                    //Debug.Log("elitePower : " + eliteThresholdPower);
                    textDisplay = "That's an elite strike!";
                    colorRed = false;
                    textTutorial.text = textDisplay;
                    animatorTutorialText.SetTrigger("animBigStrike");
                    //SetActiveText(textDisplay);
                    shotNbre = 0;
                    tutorialEnding = true;

                }
            }
        }

        void SetActiveText(string textToDisplay)
        {
            if (textTutorial.IsActive())
            {
                textTutorial.gameObject.SetActive(false); //Declanche l'anim de fin de texte avant de lancer la nouvelle.
                animatorTutorialText.SetTrigger("animTextDisappear");
                uIcanvas.SetActive(false);
            }
            if (!textTutorial.IsActive())
            {
                textTutorial.text = textToDisplay;
                if (colorRed == true)
                {
                    textTutorial.color = new Color(Rb/255, Gb/255, Bb/255, Ab/255);
                }
                else
                {
                    textTutorial.color = new Color(Rg/255, Gg/255, Bg/255, Ag/255);
                }

                textTutorial.gameObject.SetActive(true);
                uIcanvas.SetActive(true);
            }
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            if (tutorialEnding == false)
            {
                textDisplay = "Don't be too greedy! Balls are limited";
                colorRed = true;
                SetActiveText(textDisplay);
                shotNbre = 0;
            }
        }

        public override void OnBallShot()
        {
            base.OnBallShot();
            if (tutorialEnding == false)
            {
                shotNbre += 1;
                //Debug.Log("shotNbre : " + shotNbre);
                if (shotNbre == 8)
                {
                    textDisplay = "Strike at the last moment for maximum power.";
                    colorRed = false;
                    SetActiveText(textDisplay);
                }
            }
        }

        public void LoadMenu(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}
