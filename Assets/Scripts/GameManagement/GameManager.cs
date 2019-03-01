using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.BallDash
{
    public class GameManager : MonoBehaviour
    {
        public static GameState state = new GameState();
    }

    public class GameState
    {
        private List<Entity> entities = new List<Entity>();

        public void RegisterEntity(Entity ent)
        {
            entities.Add(ent);
        }

        public void UnregisterEntity(Entity ent)
        {
            entities.Remove(ent);
        }

        public void CallOnPlayerTeleport()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPlayerTeleport();
            }
        }

        public void CallOnBallShooted()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallShooted();
            }
        }

        public void CallOnBallCanceled()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallCanceled();
            }
        }

        public void CallOnBallSlowed()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallSlowed();
            }
        }

        public void CallOnBallBounced()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallBounced();
            }
        }

        public void CallOnBossHurt()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossHurt();
            }
        }

        public void CallOnBossChangeState()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossChangeState();
            }
        }

        public void CallOnBossDeath()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossDeath();
            }
        }
    }
}
