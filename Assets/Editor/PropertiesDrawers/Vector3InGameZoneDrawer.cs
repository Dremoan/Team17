using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.BallDash
{
    [CustomPropertyDrawer(typeof(Vector3InGameZone))]
    public class Vector3InGameZoneDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Vector3 target = (Vector3)fieldInfo.GetValue(property.serializedObject.targetObject);

            EditorGUI.BeginProperty(position, label, property);

            int ident = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect vectorRect = new Rect(position.x + 205, position.y, position.width * 0.61f, 16);
            Rect buttonRect = new Rect(position.x, position.y + 18, position.width, 16);
            EditorGUI.LabelField(position, label);
            GUI.enabled = false;
            EditorGUI.Vector3Field(vectorRect, "", target);
            GUI.enabled = true;

            if (GUI.Button(buttonRect, "Test"))
            {
                //display editor window
            }

            EditorGUI.indentLevel = ident;

            EditorGUI.EndProperty();
        }
    }
}
