using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.StreetHunt
{
    [CanEditMultipleObjects]
    //[CustomEditor(typeof(SnakeBoss))]
    public class SnakeBossInspector : Editor
    {
        SerializedProperty zone;

        void OnEnable()
        {
            zone = serializedObject.FindProperty("zone");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(zone);

            serializedObject.ApplyModifiedProperties();
        }
    }

}