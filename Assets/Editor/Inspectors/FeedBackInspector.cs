using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.BallDash
{
    [CustomEditor(typeof(FeedBack))]
    [CanEditMultipleObjects]
    public class FeedBackInspector : Editor
    {
        private FeedBack feedBack { get => target as FeedBack; }

        public override void OnInspectorGUI()
        { 

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("particles"), new GUIContent("Use particles"));

            if(feedBack.Particles)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("particlesSystems"), true);
            }
            EditorGUILayout.LabelField("");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("shake"), new GUIContent("Use shake"));

            if(feedBack.Shake)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useSpecificTransform"));
                if(feedBack.UseSpecificTransform)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("specificTransformToShake"), true);
                }
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shakeAmplitude"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shakeTime"));
            }

            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);

        }
    }
}
