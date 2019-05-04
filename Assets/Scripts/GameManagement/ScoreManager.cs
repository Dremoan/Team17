using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class ScoreManager : Entity
    {
        [SerializeField] private ScoreHit[] scoreHits;

        public override void OnBossHurt(int powerGroupIndex, float hitPower)
        {
            base.OnBossHurt(powerGroupIndex, hitPower);
            AddHit(powerGroupIndex);
        }

        private void AddHit(int powerGroupIndex)
        {
            if (powerGroupIndex > scoreHits.Length - 1) return;
            scoreHits[powerGroupIndex].Count++;
        }



        public ScoreHit[] ScoreHits { get => scoreHits; set => scoreHits = value; }
    }

    [System.Serializable]
    public struct ScoreHit
    {
        [SerializeField] private string name;
        [SerializeField] private int count;

        public string Name { get => name; set => name = value; }
        public int Count { get => count; set => count = value; }
    }
}
