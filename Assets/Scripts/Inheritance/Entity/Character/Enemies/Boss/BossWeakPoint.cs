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
        [SerializeField] private Material actualBossMat;
        [SerializeField] private Material newMaterial;
        [SerializeField] private SkinnedMeshRenderer skinWeakPoint;
        [SerializeField] private float blinkTime = 0.1f;
        private bool alreadyDead;
        private bool isVulnerable = true;


        public void Start()
        {
            base.Start();
            if (actualBossMat != null) actualBossMat.SetFloat("_Threshold", 0f);
        }

        public void Hit(int index, float dmgs)
        {
            if (isVulnerable && !alreadyDead)
            {
                linkedBoss.Hit(index, dmgs);
                if (!alreadyDead)
                {
                    StartCoroutine(Blink());
                }
            }
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            alreadyDead = true;
            actualTouchPlane.SetActive(false);
            deathFeedback.Play();
            skinWeakPoint.material = newMaterial;
        }

        IEnumerator Blink()
        {
            for (int i = 0; i < 2; i++)
            {
                if (actualBossMat != null) actualBossMat.SetFloat("_Threshold", 1f);
                yield return new WaitForSeconds(.1f);
                if (actualBossMat != null) actualBossMat.SetFloat("_Threshold", 0f);
                yield return new WaitForSeconds(.1f);
            }
        }
    }

}