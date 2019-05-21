using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class DummyWeakPoint : Character, IBallHitable
    {
        [Header("Gameplay Fields")]
        [SerializeField] private PlayerCharacter characterController;
        [SerializeField] private Transform tauntPoint;
        [Range(0, 200)]
        [SerializeField] private float dummyHp;

        [Header("Feedbacks")]
        [SerializeField] private FeedBack deathFeedback;
        [SerializeField] private FeedBack hitFeedback;
        [SerializeField] private GameObject weaknessFx;
        [SerializeField] private Animator canvasAnim;
        [SerializeField] private float blinkTime = 0.1f;

        [Header("Material Swap")]
        [SerializeField] private Material actualBossMat;
        [SerializeField] private Material newMaterial;
        [SerializeField] private MeshRenderer skinWeakPoint;
        [SerializeField] private float changeMaterialDelay = 2f;
        private bool alreadyDead;
        private bool isVulnerable = true;


        protected override void Start()
        {
            base.Start();
            if (actualBossMat != null) actualBossMat.SetFloat("_Threshold", 0f);
        }


        public void Hit(int index, float dmgs)
        {
            if (isVulnerable && !alreadyDead)
            {
                dummyHp -= dmgs;
                hitFeedback.Play();
                if (dummyHp < 0)
                {
                    GameManager.state.CallOnDummyDeath();
                }
                if (!alreadyDead)
                {
                    StartCoroutine(Blink());
                }
            }
        }

        public override void OnDummyDeath()
        {
            base.OnDummyDeath();
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
            hitFeedback.gameObject.SetActive(false);
            weaknessFx.SetActive(false);
            canvasAnim.Play("FlashBlanc");
            skinWeakPoint.material = newMaterial;
            characterController.TeleportAndTaunt(tauntPoint);
        }
    }

}