﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages several timers at the same time.
/// </summary>
public class TimersCalculator : MonoBehaviour
{
    private List<Timer> timers = new List<Timer>();
    private float speed = 1f;
    private bool stopped = false;
    private int givenTimers = 0;

    private void Update()
    {
        TimersManager();
    }

    private void TimersManager()
    {
        if (stopped) return;
        for (int i = 0; i < timers.Count; i++)
        {
            Timer t = timers[i];
            if (t.Calculated)
            {
                t.Inc += Time.deltaTime * speed;
                t.TimeLeft = t.MaxTime - t.Inc;
                if (t.Inc > t.MaxTime)
                {
                    t.EndFunction.Invoke();
                    timers.Remove(timers[i]);
                }
                else
                {
                    timers[i] = t;
                }
            }
        }
    }

    /// <summary>
    /// Add and launch a new timer. Returns an index to access the timer if needed later.
    /// </summary>
    /// <param name="time">The time goal of the timer</param>
    /// <param name="end">The function the timer will call when it ends</param>
    /// <returns></returns>
    public int LaunchNewTimer(float time, System.Action end)
    {
        timers.Add(new Timer(time, end, givenTimers));
        givenTimers++;
        return givenTimers - 1;
    }

    /// <summary>
    /// Stop a timer without deleting it.
    /// </summary>
    /// <param name="i">Index of the timer to stop.</param>
    /// <param name="stop">True will stop the timer, false will restart it where it was stopped.</param>
    public void StopTimer(int i, bool stop)
    {
        CheckForListExistance();
        Timer t = GetTimerFromUserIndex(i);
        if (t.UserIndex == -1) return;
        t.Calculated = !stop;
        int index = GetTimerIndexFromUserIndex(i);
        if (index != -1)
        {
            timers[index] = t;
        }
    }

    /// <summary>
    /// Add time to the time limit of a timer.
    /// </summary>
    /// <param name="i">Index of the timer to add time to.</param>
    /// <param name="addedTime">Time to add</param>
    public void AddTime(int i, float addedTime)
    {
        CheckForListExistance();
        Timer t = GetTimerFromUserIndex(i);
        if (t.UserIndex == -1) return;
        t.MaxTime += addedTime;
        int index = GetTimerIndexFromUserIndex(i);
        if (index != -1)
        {
            timers[index] = t;
        }
    }

    /// <summary>
    /// Invoke the end method of a timer, then delete it.
    /// </summary>
    /// <param name="i">Index of the timer to shortcut</param>
    public void ShortCutTimer(int i)
    {
        CheckForListExistance();
        Timer t = GetTimerFromUserIndex(i);
        if (t.UserIndex == -1) return;
        t.EndFunction.Invoke();
        int index = GetTimerIndexFromUserIndex(i);
        if (index != -1) timers.Remove(timers[index]);
    }

    /// <summary>
    /// Delete a timer without calling its end method.
    /// </summary>
    /// <param name="i"></param>
    public void DeleteTimer(int i)
    {
        CheckForListExistance();
        Timer t = GetTimerFromUserIndex(i);
        if (t.UserIndex == -1)
        {
            return;
        }
        int index = GetTimerIndexFromUserIndex(i);
        if (index != -1)
        {
            timers.Remove(timers[index]);
        }
    }

    private bool CheckForListExistance()
    {
        if (timers.Count < 1)
        {
            return false;
        }
        return true;
    }

    public Timer GetTimerFromUserIndex(int u)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (timers[i].UserIndex == u)
            {
                return timers[i];
            }
        }
        Timer t = new Timer(0, null, -1);
        return t;
    }

    private int GetTimerIndexFromUserIndex(int userIndex)
    {
        for (int i = 0; i < timers.Count; i++)
        {
            if (timers[i].UserIndex == userIndex)
            {
                return i;
            }
        }
        return -1;
    }

    public float Speed { get => speed; set => speed = value; }
    public bool Stopped { get => stopped; set => stopped = value; }
}

public struct Timer
{
    private float maxTime;
    private float inc;
    private float timeLeft;
    private bool calculated;
    private int userIndex;
    private System.Action endFunction;

    public Timer(float time, System.Action action, int index)
    {
        maxTime = time;
        inc = 0;
        timeLeft = time;
        calculated = true;
        userIndex = index;
        endFunction = action;
    }

    public float Inc { get => inc; set => inc = value; }
    public Action EndFunction { get => endFunction; }
    public float MaxTime { get => maxTime; set => maxTime = value; }
    public bool Calculated { get => calculated; set => calculated = value; }
    public int UserIndex { get => userIndex; }
    public float TimeLeft { get => timeLeft; set => timeLeft = value; }
}