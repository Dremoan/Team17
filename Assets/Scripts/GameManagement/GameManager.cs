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
        private List<VirtualCameraTarget> virtualCameraTargets = new List<VirtualCameraTarget>();
        private GameObject playerGameObject;
        private int livesLeft;

        #region Entity registration

        public void RegisterEntity(Entity ent)
        {
            entities.Add(ent);
        }

        public void UnregisterEntity(Entity ent)
        {
            entities.Remove(ent);
        }

        #endregion

        #region Virtual camera targets registration

        public void RegisterVirtualCameraTarget(VirtualCameraTarget target)
        {
            virtualCameraTargets.Add(target);
        }

        public void UnregisterVirtualCameraTarget(VirtualCameraTarget target)
        {
            virtualCameraTargets.Remove(target);
        }

        #endregion

        #region Entity events

        #region Player Events

        public void CallOnPlayerTeleport()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPlayerTeleport();
            }
        }

        public void CallOnCharacterStartStrikeAnim()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnCharacterStartStrikeAnim();
            }
        }

        public void CallOnBallShot()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallShot();
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

        public void CallOnBallHit(float hitPower)
        {
            livesLeft--;
            if (livesLeft == 0)
            {
                //game over;
            }
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallHit(hitPower);
            }
        }

        public void CallOnBallDestroyed()
        {
            livesLeft--;
            if (livesLeft == 0)
            {
                //game over;
            }
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallDestroyed();
            }
        }

        #endregion

        #region Boss events

        public void CallOnBossEnters()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossEnters();
            }
        }

        public void CallOnBossBeginsPattern()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossBeginsPatterns();
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

        public void CallOnStateIntroBegins(FightGlobalState state)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnStateIntroBegins(state);
            }
        }

        public void CallOnStateIntroEnds(FightGlobalState state)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnStateIntroEnds(state);
            }
        }

        public void CallOnStateExitBegins(FightGlobalState state)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnStateExitBegins(state);
            }
        }

        public void CallOnStateExitEnds(FightGlobalState state)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnStateExitEnds(state);
            }
        }

        public void CallOnBossDeath()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossDeath();
            }
        }

        public void CallOnLevelEnd()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnLevelEnd();
            }
        }

        #endregion

        #region Game management events

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

        #endregion

        #endregion

        /// <summary>
        /// Clear all attributes of the GameState. Call this method before loading a new scene.
        /// </summary>

        public void ResetState()
        {
            entities.Clear();
            livesLeft = 0;
            playerGameObject = null;
        }

        public GameObject PlayerGameObject { get => playerGameObject; set => playerGameObject = value; }
        public int LivesLeft { get => livesLeft; set => livesLeft = value; }
        public List<VirtualCameraTarget> VirtualCameraTargets { get => virtualCameraTargets; }
    }
}
