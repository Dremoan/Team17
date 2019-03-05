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
            return EditorGUIUtility.singleLineHeight * 2 + 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            BossAimZone target = (BossAimZone)fieldInfo.GetValue(property.serializedObject.targetObject);

            EditorGUI.BeginProperty(position, label, property);

            int ident = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect vectorRect = new Rect(position.x + 205, position.y, position.width * 0.4f, 16);
            Rect buttonRect = new Rect(position.x + 455, position.y, position.width * 0.15f, 16);
            EditorGUI.LabelField(position, label);
            GUI.enabled = false;
            EditorGUI.Vector3Field(vectorRect, "", target.ZoneCenter);
            GUI.enabled = true;

            if (GUI.Button(buttonRect, "Edit Zone"))
            {
                //display editor window
                BossAimZoneWindow window = BossAimZoneWindow.ShowWindow();
                window.SetBaseValues(target.ZoneCenter, 2);
            }

            EditorGUI.indentLevel = ident;

            EditorGUI.EndProperty();
        }
    }
}
