using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class BossWeakPoint : Character, IBallHitable
    {
        [Header("Gameplay Fields")]
        [SerializeField] private Boss linkedBoss;

        [Header("Feedbacks")]
        [SerializeField] private FeedBack deathFeedback;
        [SerializeField] private FeedBack hitFeedback;
        [SerializeField] private Animator canvasAnim;
        [SerializeField] private float blinkTime = 0.1f;

        [Header("Material Swap")]
        [SerializeField] private Material actualBossMat;
        [SerializeField] private Material newMaterial;
        [SerializeField] private SkinnedMeshRenderer skinWeakPoint;
        [SerializeField] private float changeMaterialDelay = 2f;
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
                hitFeedback.Play();
                if (!alreadyDead)
                {
                    StartCoroutine(Blink());
                }
            }
        }

        public override void OnBossDeath()
        {
            base.OnBossDeath();
            StartCoroutine(ExplosionWeakPoint());
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

        IEnumerator ExplosionWeakPoint()
        {
            alreadyDead = true;
            deathFeedback.Play();
            yield return new WaitForSeconds(changeMaterialDelay);
            hitFeedback.Play();
            canvasAnim.Play("FlashBlanc");
            skinWeakPoint.material = newMaterial;
        }
    }

}