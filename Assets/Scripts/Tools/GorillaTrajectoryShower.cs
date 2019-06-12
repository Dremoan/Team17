using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Team17.StreetHunt
{
    [ExecuteInEditMode]
    public class GorillaTrajectoryShower : MonoBehaviour
    {
        [SerializeField] private float lowJumpThreshHold = 5f;
        [SerializeField] private float maxSummitHeight = 12f;
        [SerializeField] private int jumpSteps = 15;
        [SerializeField] private Transform[] targets;
        [SerializeField] private List<Curve> curves;

        private Vector3[] path;

        private void Update()
        {
            DrawCurves();
        }

        private void DrawCurves()
        {
            for (int i = 0; i < curves.Count; i++)
            {
                Vector3 vectorToTarget = (curves[i].End.position - curves[i].Begin.position);
                Vector3 middlePoint = curves[i].Begin.position + (vectorToTarget * 0.5f);

                curves[i].Summit.position = new Vector3(middlePoint.x, curves[i].Summit.position.y, 0);

                if (Mathf.Abs(curves[i].Begin.position.y - curves[i].End.position.y) < lowJumpThreshHold) // jump is low
                {
                    curves[i].Summit.position = new Vector3(curves[i].Summit.position.x, curves[i].End.position.y + 8f, 0f);
                }
                else // jump is high
                {
                    if (curves[i].Begin.position.y > curves[i].End.position.y)
                    {
                        curves[i].Summit.position = new Vector3(curves[i].Summit.position.x, curves[i].Begin.position.y * 2, 0);
                    }
                    else
                    {
                        curves[i].Summit.position = new Vector3(curves[i].Summit.position.x, curves[i].End.position.y * 2, 0);
                    }
                }

                if (curves[i].Summit.position.y > maxSummitHeight * 2)
                {
                    curves[i].Summit.position = new Vector3(curves[i].Summit.position.x, maxSummitHeight * 2, 0);
                }
                if (curves[i].Summit.position.y < 0)
                {
                    curves[i].Summit.position = new Vector3(curves[i].Summit.position.x, 0, 0);
                }

                path = new Vector3[jumpSteps];
                for (int j = 0; j < path.Length; j++)
                {
                    float t = Mathf.InverseLerp(0, jumpSteps - 1, j);
                    path[j] = Vector3.Lerp(Vector3.Lerp(curves[i].Begin.position, curves[i].Summit.position, t), Vector3.Lerp(curves[i].Summit.position, curves[i].End.position, t), t);
                }

                for (int j = 1; j < path.Length; j++)
                {
                    Debug.DrawLine(path[j - 1], path[j], curves[i].Color);
                }

            }

        }

        [ContextMenu("Create curves")]
        public void CreateCurves()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                for (int j = 0; j < targets.Length; j++)
                {
                    if (targets[j] == targets[i]) continue;
                    bool shouldCreateCurve = true;
                    for (int h = 0; h < curves.Count; h++)
                    {
                        if (curves[h].Begin == targets[i] && curves[h].End == targets[j]) shouldCreateCurve = false;
                        if (curves[h].Begin == targets[j] && curves[h].End == targets[i]) shouldCreateCurve = false;
                    }
                    if(shouldCreateCurve)
                    {
                        GameObject summit = new GameObject();
                        summit.transform.parent = transform;
                        Color c = Random.ColorHSV();
                        curves.Add(new Curve());
                        curves[curves.Count - 1].Begin = targets[i];
                        curves[curves.Count - 1].End = targets[j];
                        curves[curves.Count - 1].Summit = summit.transform;
                        curves[curves.Count - 1].Color = c;
                    }
                }
            }
        }
    }

    [System.Serializable] 
    public class Curve
    {
        [SerializeField] private Transform begin;
        [SerializeField] private Transform summit;
        [SerializeField] private Transform end;
        [SerializeField] private Color color;

        public Transform Begin { get => begin; set => begin = value; }
        public Transform Summit { get => summit; set => summit = value; }
        public Transform End { get => end; set => end = value; }
        public Color Color { get => color; set => color = value; }
    }
}
