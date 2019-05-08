using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class BossWeakPoint : Character, IBallHitable
    {
        [SerializeField] private Boss linkedBoss;
        [SerializeField] private GameObject actualTouchPlane;
        [SerializeField] private FeedBack deathFeedback;
        [SerializeField] private Material healthBarMat;
        [SerializeField] private Texture newTexture;
        [SerializeField] private float blinkTime = 0.1f;
        private bool alreadyDead;

        public void Hit(int index, float dmgs)
        {
            linkedBoss.Hit(index, dmgs);
            StartCoroutine(Blink());
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            actualTouchPlane.SetActive(false);
            deathFeedback.Play();
            healthBarMat.SetTexture("_Tex", newTexture);
        }

        IEnumerator Blink()
        {
            if (healthBarMat != null) healthBarMat.SetFloat("_Threshold", 1f);
            yield return new WaitForSeconds(.1f);
            if (healthBarMat != null) healthBarMat.SetFloat("_Threshold", 0f);
            yield return new WaitForSeconds(.1f);
            if (healthBarMat != null) healthBarMat.SetFloat("_Threshold", 1f);
            yield return new WaitForSeconds(.1f);
            if (healthBarMat != null) healthBarMat.SetFloat("_Threshold", 0f);
        }
    }

}