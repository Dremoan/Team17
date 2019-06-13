using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;

    [Header("Input Manager")]
    [Space(20)]
    [SerializeField] bool fingerOnScreen;

    private Vector2 fingerDownPosition; //position start
    private Vector2 fingerUpPosition; //position end

    [SerializeField]
    private bool detectSwipeOnlyAfterRelease = false;
    [SerializeField]
    private bool detectTapOnlyAfterRelease = false;

    public float detectTapTime = 0.5f;
    public float detectHoldTime = 0.3f;

    [SerializeField]
    private float minDistanceForSwipe = 20f;

    //timing variables
    public float timeOnScreen;
    public float previousTimeOnScreen;

    //Delegates
    public event Action<SwipeDirection> OnSwipe = delegate { };
    public event Action<TapPosition> OnTapPosition = delegate { };
    public event Action OnTap = delegate { };
    public event Action OnHold = delegate { };

    public enum SwipeDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum TapPosition
    {
        Right,
        Left
    }

    public void Awake()
    {
        inputManager = this;
    }

    public void Start()
    {

    }

    public void Update()
    {
        TouchInput();
        TouchTimer();

        PositionTap();
    }

    public bool TapOnScreen()
    {
        if (Input.touchCount == 0)
        {
            return false;
        }


        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("TAP");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool FingerOnScreen
    {
        get
        {
            return fingerOnScreen;
        }
        set
        {

            if (value == true)
            {

            }
            else
            {
                StopTimer();
            }

            fingerOnScreen = value;
        }
    }

    public void TouchInput()
    {
        if (Input.touches.Length > 0)
        {
            FingerOnScreen = true;
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPosition = touch.position;
                fingerDownPosition = touch.position;
            }

            if (!detectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (!detectTapOnlyAfterRelease && touch.phase == TouchPhase.Stationary)
            {
                fingerDownPosition = touch.position;
                DetectTap();
            }

            if (touch.phase == TouchPhase.Stationary)
            {
                DetectHold();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;

                if (SwipeDistanceCheckMet())
                {
                    if (detectSwipeOnlyAfterRelease) Swipe();
                }
                else if (detectTapOnlyAfterRelease)
                {
                    Tap();
                }
            }
        }
        else if (FingerOnScreen)
        {
            FingerOnScreen = false;
        }
    }

    private void DetectHold()
    {
        if (timeOnScreen >= detectHoldTime)
        {
            Debug.Log("HOLD FINGER ON SCREEN");
            OnHold();
        }
    }

    void TouchTimer()
    {
        if (FingerOnScreen)
        {
            timeOnScreen += Time.unscaledDeltaTime;
        }
    }

    void StopTimer()
    {
        previousTimeOnScreen = timeOnScreen;
        timeOnScreen = 0;
    }

    void DetectTap()
    {
        if (timeOnScreen >= detectTapTime)
        {
            Tap();
        }
    }

    private void Tap()
    {
        Debug.Log("TAP ON SCREEN");
        OnTap();
    }

    void PositionTap()
    {
        if (Input.touches.Length > 0)
        {
            FingerOnScreen = true;
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                int limit = Screen.width / 2;
                int posX = Mathf.RoundToInt(touch.position.x);

                if (posX >= limit)
                {
                    SendTapPosition(TapPosition.Right);
                }
                else
                {
                    SendTapPosition(TapPosition.Left);
                }
            }

        }
    }

    void SendTapPosition(TapPosition tapPosition)
    {
        OnTapPosition(tapPosition);
    }

    #region Swipe functions

    private void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            Swipe();
        }
    }

    private void Swipe()
    {
        if (IsVerticalSwipe())
        {
            SwipeDirection direction;

            if (fingerDownPosition.y - fingerUpPosition.y > 0)
            {
                direction = SwipeDirection.Up;
            }
            else
            {
                direction = SwipeDirection.Down;
            }

            SendSwipe(direction);

            Debug.Log("SWIPE " + direction);
        }
        else
        {
            SwipeDirection direction;

            if (fingerDownPosition.x - fingerUpPosition.x > 0)
            {
                direction = SwipeDirection.Right;
            }
            else
            {
                direction = SwipeDirection.Left;
            }

            SendSwipe(direction);

            Debug.Log("SWIPE " + direction);
        }
        fingerUpPosition = fingerDownPosition;
    }

    private bool SwipeDistanceCheckMet()
    {
        if (VerticalMovementDistance() > minDistanceForSwipe || HorizontalMovementDistance() > minDistanceForSwipe)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private float VerticalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y);
    }

    private float HorizontalMovementDistance()
    {
        return Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x);
    }

    public void SendSwipe(SwipeDirection _direction)
    {
        OnSwipe(_direction);
    }

    void DebugSwipe()
    {

    }

    #endregion
}
