using System;
using UnityEngine;

public class TimerController : MonoBehaviour
{
    private int timer;
    private int timerID;
    private int turnDuration = 60;
    private int minuts = 5;

    [SerializeField] UIController uiController;

    #region SINGLE PLAYER

    private void Start()
    {
        StartNewTimer();
    }

    public void StartNewTimer()
    {
        timer = turnDuration;
        uiController.UpdateTimerText($"{minuts}:00");

        minuts--;
        timerID = LeanTween
            .value(gameObject, timer, 0, timer)
            .setOnUpdate(TimerHandlerOnUpdate)
            .setOnComplete(OnCompleteTimer)
            .id;

    }

    private void TimerHandlerOnUpdate(float value)
    {

        if ((int)value >= 10)
            uiController.UpdateTimerText($"{minuts}:{(int)value}");

        else
            uiController.UpdateTimerText($"{minuts}:0{(int)value}");
    }

    private void OnCompleteTimer()
    {
        if (minuts == 0)
        {

            OnTimerCompleted?.Invoke(this, new OnTimerCompletedEventHandler { });
        }

        else
        {

            StartNewTimer();
        }


    }

    public void StopTimer()
    {
        LeanTween.cancel(timerID, false);
    }

    public event EventHandler<OnTimerCompletedEventHandler> OnTimerCompleted;

    public class OnTimerCompletedEventHandler : EventArgs
    {

    }
    #endregion
}
