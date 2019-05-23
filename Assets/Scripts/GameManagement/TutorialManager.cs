﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.StreetHunt
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TimelinesTutorial[] timelinesFolders;
        [SerializeField] private Animator tutorialCanvasAnimator;

        private int indexTimeline = 0;
        

        public void LaunchIndexTimeline()
        {
            timelinesFolders[indexTimeline].Timeline.Play();
        }


        

        [System.Serializable]
        public class TimelinesTutorial
        {
            [SerializeField] private string timelineName;
            [SerializeField] private PlayableDirector timeline;
            [SerializeField] private float timeToEnd;

            public PlayableDirector Timeline { get => timeline; set => timeline = value; }
            public float TimeToEnd { get => timeToEnd; set => timeToEnd = value; }
        }

    }
}
