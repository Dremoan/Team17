using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class GuiHealthPoints : MonoBehaviour
    {
        public GameObject[] healthPointsArray;
        [Range(0, 3)]
        public int PlayerHpTempActual = 3; //variable temporaire des HP du joueur (à remplacer par la vrai variable).
        int LastPlayerHp = 0;

        private void Start()
        {
            LastPlayerHp = PlayerHpTempActual;
        }

        void Update()
        {
            if (PlayerHpTempActual < LastPlayerHp)
            {
                LastPlayerHp = PlayerHpTempActual;
                healthPointsArray[PlayerHpTempActual].SetActive(false);
            }
        }
    }
}
