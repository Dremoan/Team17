using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Team17.StreetHunt
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private TimelinesTutorial[] timelinesFolders;
        [SerializeField] private Animator canvasAnimator;
        [SerializeField] private Animator tutorialCanvasAnimator;

        private int indexTimeline = 0;

        public void Start()
        {
            canvasAnimator.Play("FadeOut");
        }


        private IEnumerator TimelineEnding()
        {
            yield return new WaitForSeconds(timelinesFolders[indexTimeline].TimeToEnd);
            if (indexTimeline < 3)
            {
                indexTimeline++;
            }
            canvasAnimator.Play("FadeInOpaque");
        }

        public void LaunchIndexTimeline()
        {
            timelinesFolders[indexTimeline].Timeline.Play();
        }

        public void EnableTraining()
        {
            tutorialCanvasAnimator.Play("FadeOutOpaque");
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
