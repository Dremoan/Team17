using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Team17.BallDash
{
    public class Dummy : Character, IBallHitable
    {
        [Header("UI Elements")]
        public TextMeshProUGUI textTutorial;
        public GameObject uIcanvas;
        public Animator animatorTutorialText;

        string textDisplay;
        int shotNbre;
        bool tutorialEnding = false;

        [Header("Power thresholds")]
        [SerializeField, Range(0, 400)] float smallThresholdPower = 2;
        [SerializeField, Range(0, 400)] float mediumThresholdPower = 5;
        [SerializeField, Range(0, 400)] float strongThresholdPower = 8;
        [SerializeField, Range(0, 400)] float eliteThresholdPower = 10;

        protected override void Start()
        {
            base.Start();
            textDisplay = "Hold and release!";
            SetActiveText(textDisplay);
        }

        public void Hit(float dmgs)
        {
            if (tutorialEnding == false)
            {
                if (dmgs < smallThresholdPower)
                {
                    //Debug.Log("smallPower : " + smallThresholdPower);
                    textDisplay = "Hit the ball to stack power.";
                    SetActiveText(textDisplay);
                    shotNbre = 0;
                }
                else if (dmgs < mediumThresholdPower)
                {
                    //Debug.Log("mediumPower : " + mediumThresholdPower);
                    textDisplay = "You need more power!";
                    SetActiveText(textDisplay);
                    shotNbre = 0;
                }
                else if (dmgs < strongThresholdPower)
                {
                    //Debug.Log("strongPower : " + strongThresholdPower);
                    textDisplay = "You can do better!";
                    SetActiveText(textDisplay);
                    shotNbre = 0;
                }
                else
                {
                    //Debug.Log("elitePower : " + eliteThresholdPower);
                    textDisplay = "That's an elite strike!";
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
                textTutorial.gameObject.SetActive(true);
                uIcanvas.SetActive(true);
            }
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            if (tutorialEnding == false)
            {
                textDisplay = "Be careful! Balls are limited.";
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
                Debug.Log("shotNbre : " + shotNbre);
                if (shotNbre == 8)
                {
                    textDisplay = "Last moment is always stronger!";
                    SetActiveText(textDisplay);
                }
            }
        }
    }
}
