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

        public void CallOnPlayerBeginTeleport()
        {
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].OnPlayerBeginTeleport();
            }
        }
    }
}
