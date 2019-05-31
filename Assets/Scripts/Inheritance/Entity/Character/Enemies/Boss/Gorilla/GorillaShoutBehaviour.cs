using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaShoutBehaviour : Entity
    {
        [Header("Components")]
        [SerializeField] private SphereCollider coll;
        [Header("Parameters")]
        [SerializeField] private float propagationSpeed = 0.1f;
        [SerializeField] private AnimationCurve propagationCurve;
        [SerializeField] private float initialSize = 1f;
        [SerializeField] private float targetSize = 5f;
        [Header("Prototype")]
        [SerializeField] private SpriteRenderer currentWaveSprite;
        [SerializeField] private SpriteRenderer goalWaveSprite;

        private float timeInc = 0f;

        protected override void OnEnable()
        {
            base.OnEnable();
            goalWaveSprite.transform.localScale = Vector3.one * targetSize;
            currentWaveSprite.transform.localScale = Vector3.one * initialSize;
            coll.radius = initialSize * 0.5f;
        }

        protected override void Update()
        {
            base.Update();
            ShoutManagement();
        }

        private void ShoutManagement()
        {
            if (timeInc < 1f)
            {
                float ratio = propagationCurve.Evaluate(timeInc);
                float size = Mathf.Lerp(initialSize, targetSize, ratio);
                coll.radius = size * 0.5f;
                currentWaveSprite.transform.localScale = Vector3.one * size;
                timeInc += propagationSpeed * Time.deltaTime;
            }
            else
            {
                timeInc = 0f;
                gameObject.SetActive(false);
            }
        }
    }
}
