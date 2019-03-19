using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.BallDash
{
    public class BossAimZoneWindow : EditorWindow
    {
        private BossAimZone linkedZone;

        private GUIBossAimZoneCenter bossAimZoneCenter;
        private GUIBossAimZoneArea bossAimZoneArea;
        private GUIBossAimZoneHandle topHandle;
        private GUIBossAimZoneHandle rightHandle;
        private GUIBossAimZoneHandle botHandle;
        private GUIBossAimZoneHandle leftHandle;
        private Vector3 guiZoneCenter;
        private Vector3 calculatedZoneCenter;

        private float maxGameX = 9.8f;
        private float minGameX = -9.9f;
        private float maxGameY = 10.4f;
        private float minGameY = -0.4f;

        private Texture2D backgroundTex;

        private int wallWidth = 15;

        static SerializedObject serObject;

        public static BossAimZoneWindow ShowWindow()
        {
            BossAimZoneWindow window = EditorWindow.GetWindow<BossAimZoneWindow>();
            window.titleContent = new GUIContent("Edit boss aim zone");
            //serObject = new SerializedObject(_target);
            return window;
        }

        private void OnEnable()
        {
            backgroundTex = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            backgroundTex.SetPixel(0, 0, new Color(0.25f, 0.25f, 0.25f));
            backgroundTex.Apply();

            minSize = new Vector2(160, 90) * 7;
            maxSize = new Vector2(160, 90) * 7;
        }



        public void SetBaseValues(BossAimZone zone)
        {
            linkedZone = zone;
            // TODO : make the conversion method to actually get the GUI pos from the game pos
            // feed the GUI position there
            bossAimZoneCenter = new GUIBossAimZoneCenter(new Rect(guiZoneCenter.x - 10, guiZoneCenter.y - 10, 20, 20), GameToGUIPos(zone.ZoneCenter));

            Vector3 guiCenter = GameToGUIPos(zone.ZoneCenter);
            Vector3 guiTopRight = GameToGUIPos(zone.TopRight);
            Vector3 guiTopLeft = GameToGUIPos(zone.TopLeft);
            Vector3 guiBotRight = GameToGUIPos(zone.BotRight);
            Vector3 guiBotLeft = GameToGUIPos(zone.BotLeft);

            float topDist = -Vector3.Distance(guiCenter, new Vector3(guiCenter.x, guiTopRight.y));
            float rightDist = Vector3.Distance(guiCenter, new Vector3(guiTopRight.x, guiCenter.y));
            float botDist = Vector3.Distance(guiCenter, new Vector3(guiCenter.x, guiBotRight.y));
            float leftDist = -Vector3.Distance(guiCenter, new Vector3(guiBotLeft.x, guiCenter.y));

            bossAimZoneArea = new GUIBossAimZoneArea(bossAimZoneCenter, topDist, rightDist, botDist, leftDist);

            topHandle = new GUIBossAimZoneHandle(BossAimZoneHandleType.Top, bossAimZoneArea);
            rightHandle = new GUIBossAimZoneHandle(BossAimZoneHandleType.Right, bossAimZoneArea);
            botHandle = new GUIBossAimZoneHandle(BossAimZoneHandleType.Bottom, bossAimZoneArea);
            leftHandle = new GUIBossAimZoneHandle(BossAimZoneHandleType.Left, bossAimZoneArea);

        }

        private void OnGUI()
        {
            DrawBackgroud();

            DrawGrid(20, 0.2f, Color.gray);
            DrawWalls();

            bossAimZoneCenter.Draw();
            bossAimZoneArea.Draw();
            topHandle.Draw();
            rightHandle.Draw();
            botHandle.Draw();
            leftHandle.Draw();

            ProcessEvents(Event.current);

            linkedZone.ZoneCenter = GUIToGamePos(bossAimZoneCenter.GuiCenterPosition);
            linkedZone.TopRight = GUIToGamePos(bossAimZoneArea.RightLineStart);
            linkedZone.TopLeft = GUIToGamePos(bossAimZoneArea.TopLineStart);
            linkedZone.BotLeft = GUIToGamePos(bossAimZoneArea.LeftLineStart);
            linkedZone.BotRight = GUIToGamePos(bossAimZoneArea.BotLineStart);
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

        private void ProcessEvents(Event e)
        {
            if(WindowContains(e.mousePosition))
            {
                if(bossAimZoneCenter.ProcessEvents(e)) ValueChanged();
                if (topHandle.ProcessEvents(e)) ValueChanged();
                if (rightHandle.ProcessEvents(e)) ValueChanged();
                if (botHandle.ProcessEvents(e)) ValueChanged();
                if (leftHandle.ProcessEvents(e)) ValueChanged();
            }
        }

        private void ValueChanged()
        {
            GUI.changed = true;
            linkedZone.ZoneCenter = GUIToGamePos(bossAimZoneCenter.GuiCenterPosition);
            linkedZone.TopRight = GUIToGamePos(bossAimZoneArea.RightLineStart);
            linkedZone.TopLeft = GUIToGamePos(bossAimZoneArea.TopLineStart);
            linkedZone.BotLeft = GUIToGamePos(bossAimZoneArea.LeftLineStart);
            linkedZone.BotRight = GUIToGamePos(bossAimZoneArea.BotLineStart);
            // save values here
        }

        private bool WindowContains(Vector3 pos)
        {
            if (pos.x > minSize.x - wallWidth) return false;
            if (pos.x < wallWidth) return false;
            if (pos.y > minSize.y - wallWidth) return false;
            if (pos.y < wallWidth) return false;
            return true;
        }

        private Vector3 GUIToGamePos(Vector3 pos)
        {
            float x = Mathf.InverseLerp(0, maxSize.x, pos.x);
            float y = Mathf.InverseLerp(0, maxSize.y, pos.y);
            x = Mathf.Lerp(minGameX, maxGameX, x);
            y = Mathf.Lerp(maxGameY, minGameY, y);
            return new Vector3(x, y, 0);
        }

        private Vector3 GameToGUIPos(Vector3 pos)
        {
            float x = Mathf.InverseLerp(minGameX, maxGameX, pos.x);
            float y = Mathf.InverseLerp(minGameY, maxGameY, pos.y);
            x = Mathf.Lerp(0, minSize.x, x);
            y = Mathf.Lerp(minSize.y, 0, y);
            return new Vector3(x, y, 0);
        }
    }

    public class GUIBossAimZoneCenter
    {
        private Vector3 guiCenterPosition;

        private Rect rect;

        public GUIBossAimZoneCenter(Rect centerRect, Vector3 initialGameCenterPos)
        {
            guiCenterPosition = initialGameCenterPos;
            rect = centerRect;
        }

        public void Draw()
        {
            Handles.BeginGUI();

            rect = new Rect(guiCenterPosition.x - rect.width * 0.5f, guiCenterPosition.y - rect.height * 0.5f, rect.width, rect.height);
            GUI.Box(rect, "");

            Handles.EndGUI();
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1)
                    {
                        guiCenterPosition = e.mousePosition;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public Vector3 GuiCenterPosition { get => guiCenterPosition; set => guiCenterPosition = value; }

    }

    public class GUIBossAimZoneHandle
    {
        private BossAimZoneHandleType type;
        private GUIBossAimZoneArea area;

        private Rect rect;

        private float width = 10f;

        private bool dragging = false;

        public GUIBossAimZoneHandle(BossAimZoneHandleType t, GUIBossAimZoneArea a)
        {
            area = a;
            type = t;
        }

        public bool ProcessEvents(Event e)
        {
            switch(e.type)
            {
                case EventType.MouseDown:
                    if(e.button == 0)
                    {
                        if(rect.Contains(e.mousePosition))
                        {
                            dragging = true;
                        }
                    }
                    break;
                case EventType.MouseDrag:
                    if (dragging)
                    {
                        switch (type)
                        {
                            case BossAimZoneHandleType.Top:
                                area.TopDist = -Vector3.Distance(area.CenterPos, new Vector3(area.CenterPos.x, e.mousePosition.y));
                                break;
                            case BossAimZoneHandleType.Right:
                                area.RightDist = Vector3.Distance(area.CenterPos, new Vector3(e.mousePosition.x, area.CenterPos.y));
                                break;
                            case BossAimZoneHandleType.Bottom:
                                area.BotDist = Vector3.Distance(area.CenterPos, new Vector3(area.CenterPos.x, e.mousePosition.y));
                                break;
                            case BossAimZoneHandleType.Left:
                                area.LeftDist = -Vector3.Distance(area.CenterPos, new Vector3(e.mousePosition.x, area.CenterPos.y));
                                break;
                        }
                    }
                    break;
                case EventType.MouseUp:
                    if (e.button == 0)
                    {
                        dragging = false;
                    }
                    break;
            }
            return dragging;
        }

        public void Draw()
        {
            Handles.BeginGUI();

            switch(type)
            {
                case BossAimZoneHandleType.Top:
                    rect = new Rect(area.TopHandlePos.x - width * 0.5f, area.TopHandlePos.y - width * 0.5f, width, width);
                    break;
                case BossAimZoneHandleType.Right:
                    rect = new Rect(area.RightHandlePos.x - width * 0.5f, area.RightHandlePos.y - width * 0.5f, width, width);
                    break;
                case BossAimZoneHandleType.Bottom:
                    rect = new Rect(area.BotHandlePos.x - width * 0.5f, area.BotHandlePos.y - width * 0.5f, width, width);
                    break;
                case BossAimZoneHandleType.Left:
                    rect = new Rect(area.LeftHandlePos.x - width * 0.5f, area.LeftHandlePos.y - width * 0.5f, width, width);
                    break;
            }

            GUI.Box(rect, "");

            Handles.EndGUI();
        }
    }

    public class GUIBossAimZoneArea
    {
        private GUIBossAimZoneCenter center;
        private float topDist = -50f;
        private float rightDist = 50f;
        private float botDist = 50f;
        private float leftDist = -50f;

        private Vector3 topLineStart = Vector3.zero;
        private Vector3 rightLineStart = Vector3.zero;
        private Vector3 botLineStart = Vector3.zero;
        private Vector3 leftLineStart = Vector3.zero;

        private Vector3 topHandlePos;
        private Vector3 rightHandlePos;
        private Vector3 botHandlePos;
        private Vector3 leftHandlePos;

        public GUIBossAimZoneArea(GUIBossAimZoneCenter c, float t, float r, float b, float l)
        {
            center = c;
            topDist = t;
            rightDist = r;
            botDist = b;
            leftDist = l;
        }

        public void Draw()
        {
            Handles.BeginGUI();

            topLineStart = new Vector3(leftDist, topDist) + center.GuiCenterPosition;
            rightLineStart = new Vector3(rightDist, topDist) + center.GuiCenterPosition;
            botLineStart = new Vector3(rightDist, botDist) + center.GuiCenterPosition;
            leftLineStart = new Vector3(leftDist, botDist) + center.GuiCenterPosition;

            /*topLineStart = new Vector3(leftDist, topDist);
            rightLineStart = new Vector3(rightDist, topDist);
            botLineStart = new Vector3(rightDist, botDist);
            leftLineStart = new Vector3(leftDist, botDist);*/

            Handles.DrawLine(topLineStart, rightLineStart);
            Handles.DrawLine(rightLineStart, botLineStart);
            Handles.DrawLine(botLineStart, leftLineStart);
            Handles.DrawLine(leftLineStart, topLineStart);

            topHandlePos = new Vector3(center.GuiCenterPosition.x, topLineStart.y);
            rightHandlePos = new Vector3(rightLineStart.x, center.GuiCenterPosition.y);
            botHandlePos = new Vector3(center.GuiCenterPosition.x, botLineStart.y);
            leftHandlePos = new Vector3(leftLineStart.x, center.GuiCenterPosition.y);

            Handles.EndGUI();
        }

        public Vector3 CenterPos { get => center.GuiCenterPosition; }
        public float TopDist { get => topDist; set => topDist = value; }
        public float RightDist { get => rightDist; set => rightDist = value; }
        public float BotDist { get => botDist; set => botDist = value; }
        public float LeftDist { get => leftDist; set => leftDist = value; }

        public Vector3 TopHandlePos { get => topHandlePos; set => topHandlePos = value; }
        public Vector3 RightHandlePos { get => rightHandlePos; set => rightHandlePos = value; }
        public Vector3 BotHandlePos { get => botHandlePos; set => botHandlePos = value; }
        public Vector3 LeftHandlePos { get => leftHandlePos; set => leftHandlePos = value; }

        public Vector3 TopLineStart { get => topLineStart; set => topLineStart = value; }
        public Vector3 RightLineStart { get => rightLineStart; set => rightLineStart = value; }
        public Vector3 BotLineStart { get => botLineStart; set => botLineStart = value; }
        public Vector3 LeftLineStart { get => leftLineStart; set => leftLineStart = value; }
    }

    public enum BossAimZoneHandleType
    {
        Top,
        Right,
        Bottom,
        Left,
    };
}
