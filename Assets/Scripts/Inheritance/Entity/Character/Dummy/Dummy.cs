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

        [Header("Power thresholds")]
        [SerializeField, Range(0, 400)] float smallThresholdPower = 2;
        [SerializeField, Range(0, 400)] float mediumThresholdPower = 5;
        [SerializeField, Range(0, 400)] float strongThresholdPower = 8;
        [SerializeField, Range(0, 400)] float eliteThresholdPower = 10;

        protected override void Start()
        {
            base.Start();
            textDisplay = "Hold and release !";
            SetActiveText(textDisplay);
        }

        public void Hit(float dmgs)
        {
            if (dmgs < smallThresholdPower)
            {
                Debug.Log("smallPower : " + smallThresholdPower);
                textDisplay = "Hit the ball to accumulate power before attack.";
                SetActiveText(textDisplay);
            }
            else if (dmgs < mediumThresholdPower)
            {
                Debug.Log("mediumPower : " + mediumThresholdPower);
                textDisplay = "Hit the ball more ! Balls are limited.";
                SetActiveText(textDisplay);
            }
            else if (dmgs < strongThresholdPower)
            {
                Debug.Log("strongPower : " + strongThresholdPower);
                textDisplay = "You can even do better !";
                SetActiveText(textDisplay);
            }
            else
            {
                Debug.Log("elitePower : " + eliteThresholdPower);
                textDisplay = "That's a big strike !";
                SetActiveText(textDisplay);
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
            textDisplay = "Don't forget to strike ! Balls are limited.";
            SetActiveText(textDisplay);
            shotNbre = 0;
        }

        public override void OnBallShot()
        {
            base.OnBallShot();
            shotNbre += 1;
            if(shotNbre == 8)
            {
                textDisplay = "Wait the last moment to accumulate more power.";
                SetActiveText(textDisplay);
            }

        }
    }
}
