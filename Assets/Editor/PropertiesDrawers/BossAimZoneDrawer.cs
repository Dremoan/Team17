﻿using System.Collections;
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
            EditorGUI.BeginProperty(position, label, property);

            int indent = EditorGUI.indentLevel;
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
            EditorGUI.PropertyField(centerRect, property.FindPropertyRelative("zoneCenter"), true);
            EditorGUI.PropertyField(topRightRect, property.FindPropertyRelative("topRight"), true);
            EditorGUI.PropertyField(topLeftRect, property.FindPropertyRelative("topLeft"), true);
            EditorGUI.PropertyField(botRightRect, property.FindPropertyRelative("botRight"), true);
            EditorGUI.PropertyField(botLeftRect, property.FindPropertyRelative("botLeft"), true);
            GUI.enabled = true;

            BossAimZone target = new BossAimZone();
            //fieldInfo.GetValue(property.FindPropertyRelative("zoneCenter"));
            

            if (GUI.Button(buttonRect, "Edit Zone"))
            {
                BossAimZoneWindow window = BossAimZoneWindow.ShowWindow();
                window.SetBaseValues(target);
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}