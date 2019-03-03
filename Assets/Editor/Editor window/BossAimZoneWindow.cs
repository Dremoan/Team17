using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.BallDash
{
    public class BossAimZoneWindow : EditorWindow
    {

        private Vector3 guiZoneCenter;
        private Vector3 calculatedZoneCenter;
        private float zoneRay;

        private GUIStyle zoneCenterStyle;

        private Texture2D backgroundTex;

        private int wallWidth = 15;
        private Vector3[] circlePoints;

        public static BossAimZoneWindow ShowWindow()
        {
            BossAimZoneWindow window = EditorWindow.GetWindow<BossAimZoneWindow>();
            window.titleContent = new GUIContent("Edit boss aim zone");
            return window;
        }

        public void SetBaseValues(Vector3 center, float ray)
        {
            guiZoneCenter = new Vector3(minSize.x * 0.5f, minSize.y - wallWidth);
            zoneRay = ray;
        }

        private void OnEnable()
        {
            backgroundTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            backgroundTex.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.25f));
            backgroundTex.Apply();

            zoneCenterStyle = new GUIStyle();
            zoneCenterStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            zoneCenterStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            zoneCenterStyle.border = new RectOffset(4, 4, 12, 12);

            minSize = new Vector2(160, 90) * 7;
            maxSize = new Vector2(160, 90) * 7;
        }

        private void OnGUI()
        {
            DrawBackgroud();

            DrawGrid(20, 0.2f, Color.gray);
            DrawWalls();

            DrawZone();

            CalculateZoneCircle(5, 10);
            ProcessEvents(Event.current);
        }

        private void DrawBackgroud()
        {
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundTex, ScaleMode.StretchToFill);
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0), new Vector3(gridSpacing * i, position.height, 0f));
            }

            for (int i = 0; i < heightDivs; i++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * i, 0), new Vector3(position.width, gridSpacing * i, 0));
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawWalls()
        {
            Handles.BeginGUI();
            Handles.color = Color.gray;

            //up wall
            for (int i = 0; i < wallWidth; i++)
            {
                Handles.DrawLine(new Vector3(0, i), new Vector3(minSize.x, i));
            }
            //right wall
            for (int i = 0; i < wallWidth; i++)
            {
                Handles.DrawLine(new Vector3(i, 0), new Vector3(i, minSize.y));
            }
            //bot wall
            for (int i = 0; i < wallWidth; i++)
            {
                Handles.DrawLine(new Vector3(0, minSize.y - i), new Vector3(minSize.x, minSize.y - i));
            }
            //left wall
            for (int i = 0; i < wallWidth; i++)
            {
                Handles.DrawLine(new Vector3(minSize.x - i, 0), new Vector3(minSize.x - i, minSize.y));
            }
            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawZone()
        {
            Handles.BeginGUI();

            Rect zoneCenterRect = new Rect(guiZoneCenter.x - 10, guiZoneCenter.y - 10, 20, 20);
            GUI.Box(zoneCenterRect, "");
            

            Handles.EndGUI();
        }

        private void ProcessEvents(Event e)
        {
            switch(e.type)
            {
                case EventType.MouseDown:
                    if(PosInGameZone(e.mousePosition))
                    {
                        guiZoneCenter = e.mousePosition;
                        GUI.changed = true;
                    }
                    break;
                    
            }
        }

        private bool PosInGameZone(Vector3 pos)
        {
            if (pos.x > minSize.x - wallWidth) return false;
            if (pos.x < wallWidth) return false;
            if (pos.y > minSize.y - wallWidth) return false;
            if (pos.y < wallWidth) return false;
            return true;
        }

        private void CalculateZoneCircle(float ray, int segments)
        {
            circlePoints = new Vector3[segments];
            for (int i = 0; i < circlePoints.Length; i++)
            {
                /*float ratio = 360 / (entities - 1);
                for (int i = 0; i < entities - 1; i++)
                {
                    Vector3 pos;
                    pos.x = center.x + (circle.radius * Mathf.Sin((ratio * i) * Mathf.Deg2Rad));
                    pos.y = center.y + (circle.radius * Mathf.Cos((ratio * i) * Mathf.Deg2Rad));
                    pos.z = 0;
                    potentialCirclePos.Add(pos);
                }*/
            }
        }
    }
}
