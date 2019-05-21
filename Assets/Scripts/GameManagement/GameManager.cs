using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    public class GameManager : MonoBehaviour
    {
        public static GameState state = new GameState();
    }

    public class GameState
    {
        private List<Entity> entities = new List<Entity>();
        private List<VirtualCameraShakeTarget> virtualCameraShakeTargets = new List<VirtualCameraShakeTarget>();
        private List<VirtualCameraZoomTarget> virtualCameraZoomTargets = new List<VirtualCameraZoomTarget>();
        private GameObject ballGameObject;
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

        public void RegisterVirtualCameraShakeTarget(VirtualCameraShakeTarget target)
        {
            virtualCameraShakeTargets.Add(target);
        }

        public void UnregisterVirtualCameraShakeTarget(VirtualCameraShakeTarget target)
        {
            virtualCameraShakeTargets.Remove(target);
        }

        public void RegisterVirtualCameraZoomTarget(VirtualCameraZoomTarget target)
        {
            virtualCameraZoomTargets.Add(target);
        }

        public void UnregisterVirtualCameraZoomTarget(VirtualCameraZoomTarget target)
        {
            virtualCameraZoomTargets.Remove(target);
        }

        #endregion

        #region Entity events

        #region Cutscenes Events

        public void CallOnIntroCutScene()
        {
            Debug.Log("Intro cut scene");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnIntroCutScene();
            }
        }

        public void CallOnIntroCutSceneEnds()
        {
            Debug.Log("Intro cut scene ends");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnIntroCutSceneEnds();
            }
        }

        public void CallOnPhaseTwoCutScene()
        {
            Debug.Log("Phase two cut scene");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPhaseTwoCutScene();
            }
        }

        public void CallOnPhaseTwoCutSceneEnds()
        {
            Debug.Log("Phase two cut scene ends");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPhaseTwoCutSceneEnds();
            }
        }

        public void CallOnPhaseThreeCutScene()
        {
            Debug.Log("Phase three cut scene");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPhaseThreeCutScene();
            }
        }

        public void CallOnPhaseThreeCutSceneEnds()
        {
            Debug.Log("Phase three cut scene ends");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPhaseThreeCutSceneEnds();
            }
        }

        public void CallOnEndCutScene()
        {
            Debug.Log("End cut scene");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnEndCutScene();
            }
        }

        public void CallOnEndCutSceneEnds()
        {
            Debug.Log("End cut scene ends");
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnEndCutSceneEnds();
            }
        }

        #endregion

        #region Player Events

        public void CallOnPlayerTeleport()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPlayerTeleport();
            }
        }

        public void CallOnCharacterStunned()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnCharacterStunned();
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

        public void CallOnBallHit(int powerGroupIndex, float hitPower)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBallHit(powerGroupIndex, hitPower);
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

        #region Boss Events

        public void CallOnBossEnters()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnIntroCutScene();
            }
        }

        public void CallOnBossBeginsPattern()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossBeginsPatterns();
            }
        }

        public void CallOnBossHurt(int powerGroupIndex, float hitPower)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnBossHurt(powerGroupIndex, hitPower);
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

        public void CallOnLevelEnd()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnLevelEnd();
            }
        }

        #endregion

        #region Game Management Events

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


        #region Tutorial Events

        public void CallOnDummyDeath()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnDummyDeath();
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
            virtualCameraShakeTargets.Clear();
            virtualCameraZoomTargets.Clear();
            livesLeft = 0;
            ballGameObject = null;
        }

        public GameObject BallGameObject { get => ballGameObject; set => ballGameObject = value; }
        public int LivesLeft { get => livesLeft; set => livesLeft = value; }
        public List<VirtualCameraShakeTarget> VirtualCameraShakeTargets { get => virtualCameraShakeTargets; }
        public List<VirtualCameraZoomTarget> VirtualCameraZoomTargets { get => virtualCameraZoomTargets; }
    }
}
