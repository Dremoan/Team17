using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Team17.StreetHunt
{
    public class MainMenuManager : Entity
    {
        [SerializeField] private MainMenuSaveLoader saveLoader;
        [SerializeField] private Animator compositionAnimator;
        [SerializeField] private TransitionEvents[] transitionEvents;
        [SerializeField] private UiSceneManagement sceneManager;
        [SerializeField] private Animator selectionMenuAnimator;
        [SerializeField] private int indexLevel;
        private int indexSwipe;

        public int IndexSwipe { get => indexSwipe; set => indexSwipe = value; }

        protected override void Start()
        {
            base.Start();
            SetLevelIndex(0);
            saveLoader.DisplayDataFromSave(indexSwipe);
            InputManager.inputManager.OnSwipe += SwipeRight;
            InputManager.inputManager.OnSwipe += SwipeLeft;
        }

        public void PickRandomCompositionAnim(int MaxRange)
        {
            compositionAnimator.SetInteger("IndexToPick", Random.Range(0, MaxRange));
        }

        public void SetLevelIndex(int newIndexLevel)
        {
            indexLevel = newIndexLevel;
        }

        public void CallOpenLevelMenu(float timeToOpen)
        {
            StartCoroutine(OpenLevelMenu(timeToOpen));
        }

        public void CallCloseLevelMenu(float timeToClose)
        {
            StartCoroutine(CloseLevelMenu(timeToClose));
        }

        public void LaunchSelectedLevel()
        {
            sceneManager.LoadSceneIndex(indexLevel);
        }

        public void SwipeRight(InputManager.SwipeDirection direction)
        {
            if (direction == InputManager.SwipeDirection.Left)
            {
                IndexSwipe++;
                if(indexSwipe > 2)
                {
                    indexSwipe = 2;
                }
                saveLoader.DisplayDataFromSave(indexSwipe);
                selectionMenuAnimator.SetInteger("IndexSelectionMenu", indexSwipe);
            }
            else
            {
                return;
            }
        }

        public void SwipeLeft(InputManager.SwipeDirection direction)
        {
            if (direction == InputManager.SwipeDirection.Right)
            {
                IndexSwipe--;
                if (indexSwipe < 0)
                {
                    indexSwipe = 0;
                }
                saveLoader.DisplayDataFromSave(indexSwipe);
                selectionMenuAnimator.SetInteger("IndexSelectionMenu", indexSwipe);
            }
            else
            {
                return;
            }
        }
        

        IEnumerator OpenLevelMenu(float openingDelay)
        {
            yield return new WaitForSeconds(openingDelay);
            transitionEvents[0].EventTransition[0].Invoke();
            yield return null;
            transitionEvents[0].EventTransition[1].Invoke();
        }

        IEnumerator CloseLevelMenu(float closingDelay)
        {
            yield return new WaitForSeconds(closingDelay);
            transitionEvents[1].EventTransition[0].Invoke();
            yield return null;
            transitionEvents[1].EventTransition[1].Invoke();
        }
    }

    [System.Serializable]
    public class TransitionEvents
    {
        [SerializeField] private string nameTransition;
        [SerializeField] private UnityEvent[] eventTransition;

        public UnityEvent[] EventTransition { get => eventTransition; set => eventTransition = value; }
    }
}
