using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.BallDash
{
    [CustomPropertyDrawer(typeof(BossAimZone))]
    public class BossAimZoneDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 6 + 14;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BossAimZone target = (BossAimZone)fieldInfo.GetValue(property.serializedObject.targetObject);

            EditorGUI.BeginProperty(position, label, property);

            int ident = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            float spacing = 16;
            Rect centerRect = new Rect(position.x, position.y + spacing, position.width, 16);
            Rect topRightRect = new Rect(position.x, position.y + spacing*2, position.width, 16);
            Rect topLeftRect = new Rect(position.x, position.y + spacing*3, position.width, 16);
            Rect botRightRect = new Rect(position.x, position.y + spacing*4, position.width, 16);
            Rect botLeftRect = new Rect(position.x, position.y + spacing*5, position.width, 16);
            Rect buttonRect = new Rect(position.x, position.y + spacing*6, position.width, 16);

            EditorGUI.LabelField(position, label);

            GUI.enabled = false;
            EditorGUI.Vector3Field(centerRect, "Center :", target.ZoneCenter);
            EditorGUI.Vector3Field(topRightRect, "Top right", target.TopRight);
            EditorGUI.Vector3Field(topLeftRect, "Top left :", target.TopLeft);
            EditorGUI.Vector3Field(botRightRect, "Bot right :", target.BotRight);
            EditorGUI.Vector3Field(botLeftRect, "Bot left:", target.BotLeft);
            GUI.enabled = true;

            if (GUI.Button(buttonRect, "Edit Zone"))
            {
                //display editor window
                BossAimZoneWindow window = BossAimZoneWindow.ShowWindow();
                window.SetBaseValues(target);
            }

            EditorGUI.indentLevel = ident;

            EditorGUI.EndProperty();
        }
    }
}
