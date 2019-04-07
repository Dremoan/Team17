using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Team17.BallDash
{
    public class BeginingInstructions : UiElement
    {
        [SerializeField] private GameObject beginGameText;
        [SerializeField] private GameObject controlsText;

        protected override void Start()
        {
            base.Start();
            beginGameText.SetActive(true);
        }

        public override void OnBossEnters()
        {
            base.OnBossEnters();
            beginGameText.SetActive(false);
        }

        public override void OnBossBeginsPatterns()
        {
            base.OnBossBeginsPatterns();
            controlsText.SetActive(true);
        }

        public override void OnPlayerTeleport()
        {
            base.OnPlayerTeleport();
            controlsText.SetActive(false);
        }
    }
}
