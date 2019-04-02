using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.BallDash
{
    [CustomEditor(typeof(BossAimZone))]
    [CanEditMultipleObjects]
    public class BossAimZoneInspector : Editor
    {
        private BossAimZone aimZone { get => target as BossAimZone; }

        private float maxGameX = 9.8f;
        private float minGameX = -9.9f;
        private float maxGameY = 10.4f;
        private float minGameY = -0.4f;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("zoneCenter"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topLeft"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("topRight"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("botLeft"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("botRight"));
            GUI.enabled = true;

            if(GUILayout.Button("Edit Zone"))
            {
                BossAimZoneWindow window = BossAimZoneWindow.ShowWindow();
                window.SetBaseValues(aimZone);
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}
