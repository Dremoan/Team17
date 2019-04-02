using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Team17.BallDash
{
    public class Dummy : Character, IBallHitable
    {
        public GameObject[] ask;
        [SerializeField] float smallThresholdPower = 2;
        [SerializeField] float mediumThresholdPower = 5;
        [SerializeField] float strongThresholdPower = 8;
        [SerializeField] float eliteThresholdPower = 10;

        public void Hit(float dmgs)
        {
            if (dmgs < smallThresholdPower)
            {
                Debug.Log("smallPower : " + smallThresholdPower);
                ask[0].gameObject.SetActive(true);
            }
            else if (dmgs < mediumThresholdPower)
            {
                Debug.Log("mediumPower : " + mediumThresholdPower);
                ask[1].gameObject.SetActive(true);
            }
            else if (dmgs < strongThresholdPower)
            {
                Debug.Log("strongPower : " + strongThresholdPower);
                ask[2].gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("elitePower : " + eliteThresholdPower);
                ask[3].gameObject.SetActive(true);
            }
        }

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            ask[4].gameObject.SetActive(true);
        }

        //Faire une fonction pour activer le texte qui l'efface au bout d'un moment.
    }
}
