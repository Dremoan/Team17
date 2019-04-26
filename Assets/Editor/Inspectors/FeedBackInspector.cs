﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Team17.StreetHunt
{
    [CustomEditor(typeof(FeedBack))]
    [CanEditMultipleObjects]
    public class FeedBackInspector : Editor
    {
        private FeedBack feedBack { get => target as FeedBack; }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("Base parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnStart"), new GUIContent("Play on start"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("looping"), new GUIContent("Looping"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hardFollowingTransform"));
            if(feedBack.HardFollowingTransform)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("transformToHardFollow"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("tpOnTransformOnPlay"));
            if(feedBack.TpOnTransformOnPlay)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("transformToTPToOnPlay"));
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Particles parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("particles"), new GUIContent("Use particles"));
            if(feedBack.Particles)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("particlesSystems"), true);
                if(GUILayout.Button("Find Particles"))
                {
                    feedBack.ParticlesSystems = feedBack.gameObject.GetComponentsInChildren<ParticleSystem>(true);
                }
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Trails parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("trails"), new GUIContent("Use trails"));
            if(feedBack.Trails)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("trailRenderers"), true);
                if (GUILayout.Button("Find Trails"))
                {
                    feedBack.TrailRenderers =  feedBack.gameObject.GetComponentsInChildren<TrailRenderer>(true);
                }
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Shake parameters", EditorStyles.boldLabel);
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
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Rumble parameters", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("rumble"), new GUIContent("Use rumble"));
            if(feedBack.Rumble)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rumbleTime"));
            }


            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);

        }
    }
}
