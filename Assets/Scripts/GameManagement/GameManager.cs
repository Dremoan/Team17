﻿using System.Collections;
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
        private GameObject playerGameObject;
        private int livesLeft;

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

        public void CallOnBossBeginsPattern()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossBeginsPatterns();
            }
        }

        public void CallOnIntroLaunched()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnIntroLaunched();
            }
        }

        public void CallOnBallShot()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallShot();
            }
        }

        public void CallOnBallDestroyed()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallDestroyed();
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

        public void CallOnPause()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPause();
            }
        }

        public void CallOnResume()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnResume();
            }
        }

        public GameObject PlayerGameObject { get => playerGameObject; set => playerGameObject = value; }
        public int LivesLeft { get => livesLeft; set => livesLeft = value; }
    }
}
