using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GorillaBoss : Entity
    {
        [SerializeField] private Animator gorillaAnimator;
        public void Attack(string attackName)
        {
            //Enable Boosts
            gorillaAnimator.SetTrigger(attackName);
        }
    }
}
