using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class GuiHealthPoints : Entity
    {
        public GameObject[] nbreBallsArray;
        [SerializeField] int nbreBall = 4; //variable temporaire des HP du joueur (à remplacer par la vrai variable).

        public override void OnBallDestroyed()
        {
            base.OnBallDestroyed();
            GuiNbreBalls();
        }

        private void GuiNbreBalls()
        {
            nbreBall = GameManager.state.LivesLeft;
            if ((nbreBall-2) >= 0)
            {
                nbreBallsArray[nbreBall - 2].SetActive(false);
            }
            else if (nbreBall < 0)
            {
                Debug.LogWarning("Balls number can't be under 0 !");
            }
        }
    }
}
